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
	void OnMouseDown()
	{
		print("bound: " + thisCollider.bounds.ToString());
		List <Vector3> ends = GetEnds(this.thisCollider.bounds);

		for (int i = 0; i < ends.Count; i++)
		{
			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			go.transform.position = ends[i];
		}

	}

	private List<Vector3> GetEnds(Bounds bounds)
	{
		List<Vector3> ends = new List<Vector3>();
		float maxExtent=-100;
		int vectorIndex = -1;
		Vector3 extents = bounds.extents;
		for (int i=0; i < 3; i++)
		{
			if (extents[i] > maxExtent)
			{
				maxExtent = extents[i];
				vectorIndex = i;
			}
		}

		switch(vectorIndex)  //it should only be x or z, 0 or 2 of the Vector3
		{
			case 0:  //x
				Vector3 Xend1 = bounds.center + new Vector3(bounds.extents.x, 0,0);
				Vector3 Xend2 = bounds.center - new Vector3(bounds.extents.x, 0,0);
				ends.Add(Xend1);
				ends.Add(Xend2);
				return ends;
			case 1:
				Debug.LogError("something is wrong, the Y component should not get chosen as the max, it should only be X or Z");
				return ends;
			case 2:
				Vector3 Yend1 = bounds.center + new Vector3(0, 0, bounds.extents.y);
				Vector3 Yend2 = bounds.center - new Vector3(0, 0, bounds.extents.y);
				ends.Add(Yend1);
				ends.Add(Yend2);
				return ends;
			default: 
				Debug.LogError("something is wrong, there was no suitable VectorIndex chosen");
				return ends;
			
		}
		
	}


	/**
	TODO:
		snap this object to the grid
		snap length to grid on each side
		generate planes on node wide along the width of the
	**/

	/**
	remember, the extents.x/y/z is the length from the center to one end of the bounds box
	when rotation is 90, check extents.X
	when rotation is 0, check extents.z

		OR, just use the MAX extent.  since the shape will always be a skinny rectangle, one component of the vector3 will always be much larger than the other 2
	**/
}
