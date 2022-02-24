using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passage : MonoBehaviour
{
	/*****************
	CreateDate: 	2/23/22	
	Functionality:	this script will be used to create DayJob levels.  
						When placed in the world, the object it is on should snap to the grid, then floors and walls will be built around it. 
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public int passageWidth;   //this is the passage width, IN GRID NODES, not world space
	public BoxCollider thisCollider;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	[SerializeField] private _Grid grid;
	
	
    void Start()
    {
        thisCollider = GetComponent<BoxCollider>();
		Constrain(); 
    }

    void Update()
    {
        
    }


	private void SnapToGrid()
	{

	}

	public void Constrain()
	{
		this.transform.localScale = new Vector3(0.5f, 0.5f, this.transform.localScale.z); //this makes it a skinny rectangle if it's not already
		List<int> rotationConstraints = new List<int> () {0,90};
		int constrainedRotation = 0;
		float minDistance = 500;
		float currentRot = this.transform.localRotation.eulerAngles.y;
		for (int i = 0; i < rotationConstraints.Count; i ++)
		{
			if (Mathf.Abs(currentRot-rotationConstraints[i]) < minDistance)
			{
				minDistance = currentRot-rotationConstraints[i];
				constrainedRotation = rotationConstraints[i];
			}
		}
		this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.x, constrainedRotation, this.transform.localRotation.z);
	}


	/**
	TODO:
		snap this object to the grid
		generate planes on node wide along the width of the
	**/
}
