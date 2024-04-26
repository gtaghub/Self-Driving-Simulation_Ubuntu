using UnityEngine;
using MBaske.Sensors.Grid;
using MBaske.Sensors.Util;
namespace Adrenak.Tork.Demo 
{
	public class SmoothFollow : MonoBehaviour 
	{
        [SerializeField] GridSensorComponent3D _sensor;

        // The target we are following
        public Transform target;
		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		private float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;

		// Update is called once per frame
		void LateUpdate() {
			// Early out if we don't have a target
			if (!target)
				return;

			// Calculate the current rotation angles
			var wantedRotationAngle = target.eulerAngles.y;
			var wantedHeight = target.position.y + height;

			var currentRotationAngle = transform.eulerAngles.y;
			var currentHeight = transform.position.y;

			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			transform.position = target.position;
			transform.position -= currentRotation * Vector3.forward * distance;

			// Set the height of the camera
			transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

			// Always look at the target
			transform.LookAt(target);
		}

        private Quaternion[,] m_Wireframe;
        private static readonly Color s_WireColorA = new Color(0f, 0.5f, 1f, 0.3f);
        private static readonly Color s_WireColorB = new Color(0f, 0.5f, 1f, 0.1f);

        private void OnPostRender()
        {
            GeomUtil3D.CalcGridRotations(_sensor.CellArc,
                 _sensor.LonLatRect, _sensor.GridShape,
                 ref m_Wireframe);

            EditorUtil.GLMaterial.SetPass(0);

            GL.PushMatrix();
            GL.MultMatrix(_sensor.transform.localToWorldMatrix);

            Vector3 min = Vector3.forward * _sensor.MinDistance;
            Vector3 max = Vector3.forward * _sensor.MaxDistance;

            // Grid Cells

            int nLon = _sensor.GridShape.Width;
            int nLat = _sensor.GridShape.Height;

            for (int iLat = 0; iLat <= nLat; iLat++)
            {
                GL.Begin(GL.LINE_STRIP);
                GL.Color(s_WireColorA);
                for (int iLon = 0; iLon <= nLon; iLon++)
                {
                    var v = m_Wireframe[iLon, iLat] * max;
                    GL.Vertex3(v.x, v.y, v.z);
                }
                GL.End();
            }

            for (int iLon = 0; iLon <= nLon; iLon++)
            {
                GL.Begin(GL.LINE_STRIP);
                GL.Color(s_WireColorA);
                for (int iLat = 0; iLat <= nLat; iLat++)
                {
                    var v = m_Wireframe[iLon, iLat] * max;
                    GL.Vertex3(v.x, v.y, v.z);
                }
                GL.End();
            }

            // Angles

            if (_sensor.LatAngleSouth < 90)
            {
                GL.Begin(GL.LINES);
                GL.Color(s_WireColorB);
                for (int iLon = 0; iLon <= nLon; iLon++)
                {
                    var a = m_Wireframe[iLon, 0] * min;
                    GL.Vertex3(a.x, a.y, a.z);
                    var b = m_Wireframe[iLon, 0] * max;
                    GL.Vertex3(b.x, b.y, b.z);
                }
                GL.End();
            }

            if (_sensor.LatAngleNorth < 90)
            {
                GL.Begin(GL.LINES);
                GL.Color(s_WireColorB);
                for (int iLon = 0; iLon <= nLon; iLon++)
                {
                    var a = m_Wireframe[iLon, nLat] * min;
                    GL.Vertex3(a.x, a.y, a.z);
                    var b = m_Wireframe[iLon, nLat] * max;
                    GL.Vertex3(b.x, b.y, b.z);
                }
                GL.End();
            }

            if (_sensor.LonAngle < 180)
            {
                GL.Begin(GL.LINES);
                GL.Color(s_WireColorB);
                for (int iLat = 0; iLat <= nLat; iLat++)
                {
                    var a = m_Wireframe[0, iLat] * min;
                    GL.Vertex3(a.x, a.y, a.z);
                    var b = m_Wireframe[0, iLat] * max;
                    GL.Vertex3(b.x, b.y, b.z);
                }
                GL.End();

                GL.Begin(GL.LINES);
                GL.Color(s_WireColorB);
                for (int iLat = 0; iLat <= nLat; iLat++)
                {
                    var a = m_Wireframe[nLon, iLat] * min;
                    GL.Vertex3(a.x, a.y, a.z);
                    var b = m_Wireframe[nLon, iLat] * max;
                    GL.Vertex3(b.x, b.y, b.z);
                }
                GL.End();
            }

            GL.PopMatrix();
        }
    }
}