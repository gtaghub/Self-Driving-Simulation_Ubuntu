using UnityEngine;
using System.Collections;


public class MovingCameraScript : MonoBehaviour {

	Transform cachedTransform;
	Vector3 initialPos;
	Vector3 finalPos;
	float lerpValue = 0;
	float step = 0.03f;
	
	public float distance;

	// Use this for initialization
	void Start () {

		cachedTransform = transform;
		initialPos = cachedTransform.localPosition;
		finalPos = initialPos;
		finalPos.x += distance;

	}
	
	// Update is called once per frame
	void Update () {
		MoveCamera();
	}

	void MoveCamera ()
	{
		cachedTransform.position = Vector3.Lerp (initialPos, finalPos, lerpValue);
		lerpValue += step;
		if ((lerpValue >= 1) || (lerpValue <= 0)) {
			step = -step;
		} 
	}
	
}
