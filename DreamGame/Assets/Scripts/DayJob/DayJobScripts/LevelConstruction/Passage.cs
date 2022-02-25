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
		this.transform.localScale = new Vector3(0.3f, 0.3f, this.transform.localScale.z); //this makes it a skinny rectangle if it's not already
        thisCollider = GetComponent<BoxCollider>();
		SnapRotation(); 
		SnapPosition();
		SnapScale();
		
    }

	//snaps the Z scale so that each end is in the middle of a node. 
	private void SnapScale()
	{
		List<Vector3> newEnds = GetEndNodePositions( thisCollider.bounds);
		float distance = Vector3.Distance(newEnds[0], newEnds[1]);
		this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, distance);  // add (- (grid.nodeRadius*2)) if you want to make the ends of the passage  snap to the end of the node, not the middle of them. 
	}

	//snaps the rotation to either 0 or 90, since that's all we want for this application. 
	public void SnapRotation()
	{
		List<int> rotationConstraints = new List<int> () {0,90};
		int constrainedRotation = 0;
		float minDistance = 500;
		float currentRot = this.transform.localRotation.eulerAngles.y;
		print("currentRot: "+ currentRot);
		for (int i = 0; i < rotationConstraints.Count; i ++)
		{
			if (Mathf.Abs(currentRot-rotationConstraints[i]) < minDistance)
			{
				minDistance = currentRot-rotationConstraints[i];
				constrainedRotation = rotationConstraints[i];
				print("iteration " + i + " minDistance is " + minDistance);
			}
		}
		print("rotation snapping to Y: " + constrainedRotation);

		float newRot = Mathf.Round(currentRot/90) * 90;  	//this formula was taken from https://answers.unity.com/questions/21909/rounding-rotation-to-nearest-90-degrees.html.  
															//I don't get why it works but it does, and it handles negative values better
		this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.x, newRot, this.transform.localRotation.z);
	}


	//snaps this object to the nearest grid node
	private void SnapPosition()
	{
		Vector3 newPos = grid.NodeFromWorldPoint(this.transform.position).worldPosition;
		this.transform.position = newPos;
	}

	//gets the node positions at each of the ends, and returns a list with the two Vector3s
	public List<Vector3>  GetEndNodePositions(Bounds bounds)
	{
		List<Vector3> ends = GetEnds(bounds);
		List<Vector3> newEnds = new List<Vector3>();
		foreach(Vector3 V in ends)
		{
			Vector3 equivalentNodePos = grid.NodeFromWorldPoint(V).worldPosition;
			newEnds.Add(equivalentNodePos);
		}
		return newEnds;
	}

	//gets the ends of the bounds of a bounds.  For this application, the assumption is that we will only be dealing with long rectangles that are scaled on the 
	//Z axis and just rotated as needed
	//remember that bounds.extents returns a vector3 for the length that the bounds box stretches from the center of the bounds box, in WORLD space, NOT relative to rotation
	private List<Vector3> GetEnds(Bounds bounds)
	{
		List<Vector3> ends = new List<Vector3>();
		float maxExtent= float.MinValue;
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
				Vector3 Yend1 = bounds.center + new Vector3(0, 0, bounds.extents.z);
				Vector3 Yend2 = bounds.center - new Vector3(0, 0, bounds.extents.z);
				ends.Add(Yend1);
				ends.Add(Yend2);
				return ends;
			default: 
				Debug.LogError("something is wrong, there was no suitable VectorIndex chosen");
				return ends;
		}
	}

	//spwans a sphere at each end for visual clarity that the ends are being calculated correctly
	private void SpawnSpheresAtEnds()
	{
		List <Vector3> ends = GetEnds(this.thisCollider.bounds);
		for (int i = 0; i < ends.Count; i++)
		{
			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			go.transform.position = ends[i];
		}
	}

	void OnMouseDown()
	{
		print("bound: " + thisCollider.bounds.ToString());
		Start();
		
	}
}
