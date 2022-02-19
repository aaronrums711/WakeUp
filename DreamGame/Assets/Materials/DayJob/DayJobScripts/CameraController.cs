using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	/*****************
	CreateDate: 	2/18/22
	Functionality:	this script just listens for an event from player, and rotates this object
	Notes:
	Dependencies:	this script MUST be on the cinemachine virtual camera!
	******************/
	
	//////////////////////////////Config
	public float rotationSpeed = 3;
	public float totalRotTime = 3;
	[Range(0,1)]
	public float lerper;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
    void Start()
    {
        Player.ChangeOrientation += RotateYSmooth;
    }

    void Update()
    {
        // RotationTest();
    }


	

	public void RotateYSmooth(MovementOrientation MO)
	{
		Quaternion targetRot = Quaternion.Euler(0,MO.Yrotation,0);
		StartCoroutine(RotateSlow(targetRot, totalRotTime));
	}

	public IEnumerator RotateSlow(Quaternion targetRot, float totalTime)
	{
		Quaternion currentRot = this.transform.rotation;
		float elapsed = 0;
		while (elapsed <= totalTime)
		{
			float lerpFactor = LerpCurves.SoftestInSoftOut01  (elapsed/totalTime);
			this.transform.localRotation = Quaternion.Lerp(currentRot, targetRot,lerpFactor);
			elapsed += Time.deltaTime;
			yield return null;
		}
		

	}




	public void RotationTest()
	{
		if (Input.GetKey(KeyCode.Y))
		{
			this.transform.Rotate(0,-rotationSpeed * Time.deltaTime,0);
		}
		else if (Input.GetKey(KeyCode.T))
		{
			this.transform.Rotate(0,rotationSpeed * Time.deltaTime,0);
		}
	}
}
