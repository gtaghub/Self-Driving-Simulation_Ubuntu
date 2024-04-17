using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BehaviourScript : MonoBehaviour {

	//Character
	public Transform pointToTouch;
	public Transform pointOfPivot;
	public Transform physicalView;
	public Transform footLeft;
	public Transform footRight;
	public Transform pointHandA;
	public Transform pointHandB;
	public Transform pointHandC;

	public Transform floorA;
	public Transform floorB;
	public Transform floorC;

	Transform cachedTransform;

	Vector3 rcInitialPosition;//remote control's
	Quaternion rcInitialRotation;

	// Use this for initialization
	void Start () {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        cachedTransform = transform;
		StartCoroutine(AdjustPosition());
	}

	// Update is called once per frame
	void Update () {
	}

	IEnumerator AdjustPosition ()
	{// place the human on sofa
		
		yield return new WaitForEndOfFrame ();

		Vector3 delta3d = pointToTouch.position - pointOfPivot.position;
		cachedTransform.position += delta3d;

		yield return null;

		Vector3 footsMiddle = (footLeft.position + footRight.position) * 0.5f;

		Vector3 floorPosA = floorA.position;
		Vector3 floorPosB = floorB.position;
		Vector3 floorPosC = floorC.position;
		Vector3 footsMiddlePrj = Vector3.ProjectOnPlane(footsMiddle - floorPosA, Vector3.Cross(floorPosA - floorPosC, floorPosB - floorPosC)) + floorPosA;
		if (!Mathf.Approximately (footsMiddle.y , 0)) {
			Quaternion rotation = Quaternion.FromToRotation(footsMiddle - pointOfPivot.position , footsMiddlePrj - pointOfPivot.position );
			
			cachedTransform.rotation =  rotation * cachedTransform.rotation;
		}
		
	}


}
