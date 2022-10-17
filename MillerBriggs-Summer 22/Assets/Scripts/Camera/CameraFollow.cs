using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public float speed = 5f; 
	public GameObject target;
	public Vector3 offset;
	Vector3 targetPos, newPos;
	private Camera myCamera; 
	// Use this for initialization
	void Start()
	{
		myCamera = GetComponent<Camera>(); 
		targetPos = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (target)
		{
			Vector3 posNoZ = transform.position + offset;
			Vector3 targetDirection = (target.transform.position - posNoZ);
			float interpVelocity = targetDirection.magnitude * speed;
			targetPos = (transform.position) + (targetDirection.normalized * interpVelocity * Time.deltaTime); 
			newPos = Vector3.Lerp( transform.position, targetPos, 0.25f);

			Vector3 roundPos = new Vector3(RoundToNearestPixel(newPos.x, myCamera), RoundToNearestPixel(newPos.y, myCamera), newPos.z);
			transform.position = roundPos;

		}
	}


	public static float RoundToNearestPixel(float unityUnits, Camera viewingCamera)
	{
		float valueInPixels = (Screen.height / (viewingCamera.orthographicSize * 2)) * unityUnits;
		valueInPixels = Mathf.Round(valueInPixels);
		float adjustedUnityUnits = valueInPixels / (Screen.height / (viewingCamera.orthographicSize * 2));
		return adjustedUnityUnits;
	}
}
