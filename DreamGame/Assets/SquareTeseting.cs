using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTeseting : MonoBehaviour
{
	/*****************
	CreateDate: 
	Functionality:
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	[Range(0.01f, 1f)] public float sizer = 0f;
	public float maxPos;
	public float maxScale;
	public float startingPos;
	public float startingScale;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	public Transform thisTransform;
	
	void Start()
	{
		thisTransform = this.transform;
		startingScale = this.transform.localScale.y;
		startingPos = this.transform.position.y;
	}


    void Update()
    {
        ExtendRectangle();
    }

	public void ExtendRectangle()
	{
		Vector3 currentScale = thisTransform.localScale;
		float newScale = Mathf.Lerp(startingScale, maxScale, sizer);
		currentScale.y = newScale;
		thisTransform.localScale = currentScale;

		Vector3 currentPos = thisTransform.position;
		float newPos = Mathf.Lerp(startingPos, maxPos, sizer);
		currentPos.y = newPos;
		thisTransform.position = currentPos;

	}
}
