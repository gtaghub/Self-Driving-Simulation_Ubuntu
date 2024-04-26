using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBaske.Sensors.Grid;
using MBaske.Sensors.Util;
using UnityEngine.Rendering;

public class LidarShapeViewer : MonoBehaviour
{
    private Quaternion[,] m_Wireframe;
    private static readonly Color s_WireColorA = new Color(0f, 0.5f, 1f, 0.3f);
    private static readonly Color s_WireColorB = new Color(0f, 0.5f, 1f, 0.1f);

    [SerializeField] List<GTSLidar> _listLidarSensor;

    void Start()
    {
        RenderPipelineManager.endContextRendering += OnEndContextRendering;
    }

    void OnEndContextRendering(ScriptableRenderContext context, List<Camera> cameras)
    {
        for (int i = 0; i < _listLidarSensor.Count; i++)
        {
            RenderLines(_listLidarSensor[i].GridSensor);
        }
    }

    void OnDestroy()
    {
        RenderPipelineManager.endContextRendering -= OnEndContextRendering;
    }

    //void OnRenderObject()
    //{
    //    RenderLines();
    //}

    private void RenderLines(GridSensorComponent3D sensor)
    {
        GeomUtil3D.CalcGridRotations(sensor.CellArc,
             sensor.LonLatRect, sensor.GridShape,
             ref m_Wireframe);

        EditorUtil.GLMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(sensor.transform.localToWorldMatrix);

        Vector3 min = Vector3.forward * sensor.MinDistance;
        Vector3 max = Vector3.forward * sensor.MaxDistance;

        // Grid Cells

        int nLon = sensor.GridShape.Width;
        int nLat = sensor.GridShape.Height;

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

        if (sensor.LatAngleSouth < 90)
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

        if (sensor.LatAngleNorth < 90)
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

        if (sensor.LonAngle < 180)
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
