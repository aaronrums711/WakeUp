using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
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
	public int numWallTilesHigh;
	public BoxCollider thisCollider;
	public GameObject passageWallPrefab;  //VGIU
	public Vector3 rearVector {get { return GetRearVector(this);}}
	public List<PassageWall> walls;
	public Transform wallParent;  //VGIU, should be set in prefab

	
	//////////////////////////////State
	public bool isWallsConstructed;
	
	//////////////////////////////Cached Component References
	[SerializeField] private _Grid grid;
	
	
    void Start()
    {
		print("start called");
		this.transform.localScale = new Vector3(0.3f, 0.3f, this.transform.localScale.z); //this makes it a skinny rectangle if it's not already
        this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);  //snaps this to the floor when it's dragged in as a prefab
		
		thisCollider = GetComponent<BoxCollider>();
		grid = FindObjectOfType<_Grid>();

    }

	void OnMouseDown() //use this to snap the scale again if it's incorrect
	{
		print("bound: " + thisCollider.bounds.ToString());
		SnapRotation(); 
		SnapPosition();
		SnapScale();
	}

	// void Update()
	// {
	// 	if (Input.GetKeyDown(KeyCode.Space))
	// 	{
	// 		SpawnWallTiles(this, 3);
	// 		GetRearVector(this);
	// 	}
	// }


	//snaps the Z scale so that each end is in the middle of a node. 
	public void SnapScale()
	{
		List<Vector3> newEnds2 = GetEndNodePositions( thisCollider.bounds);
		float distance = Vector3.Distance(newEnds2[0], newEnds2[1]);
		this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y,  distance );//distance - (grid.nodeRadius*2));  // add (- (grid.nodeRadius*2)) if you want to make the ends of the passage  snap to the end of the node, not the middle of them. 
	}

	//snaps the rotation to either 0 or 90, since that's all we want for this application. 
	public void SnapRotation()
	{
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
		float newRot = Mathf.Round(currentRot/90) * 90;  	//this formula was taken from https://answers.unity.com/questions/21909/rounding-rotation-to-nearest-90-degrees.html.  
															//I don't get why it works but it does, and it handles negative values better
		this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.x, newRot, this.transform.localRotation.z);
	}


	//snaps this object to the nearest grid node
	public void SnapPosition()
	{
		Vector3 newPos = grid.NodeFromWorldPoint(this.transform.position).worldPosition;
		this.transform.position = newPos;
	}


	//gets the node positions at each of the ends, and returns a list with the two Vector3s.   The two nodes must be equi-distant from the center.  If they aren't then the nearest one is scooted up one node width and we try again
	public List<Vector3>  GetEndNodePositions(Bounds bounds)
	{
		List<Vector3> ends = GetEnds(bounds);
		List<Vector3> newEnds = new List<Vector3>();
		foreach(Vector3 V in ends)
		{
			Vector3 equivalentNodePos = grid.NodeFromWorldPoint(V).worldPosition;
			newEnds.Add(equivalentNodePos);
		}

		// print("initial node positions");
		// print(newEnds[0] + "   "  + newEnds[1]  );

		float minDistance = 1000;	
		float maxDistance = -1000;

		float distance1 = Vector3.Distance(bounds.center, newEnds[0]);
		float distance2 = Vector3.Distance(bounds.center, newEnds[1]);
		Vector3 closerVector = new Vector3();
		Vector3 fartherVector= new Vector3();
		int indexToReplace = 0;

		if (Mathf.Abs(distance1 - distance2)  >  0.2f ) 
		{
			int iterator=-1;
			// print("distances not equal, their difference is " + Mathf.Abs(distance1 - distance2));
			foreach (Vector3 V in newEnds)
			{
				iterator++;
				float distance = Vector3.Distance(bounds.center, V);
				if (distance < minDistance) 
				{
					minDistance = distance;
					closerVector = V; 	//correctionVector will be the closer of the two ends
					indexToReplace = iterator;
				}
				if (distance > maxDistance)
				{
					maxDistance = distance;
					fartherVector = V;
				}

			}
			Vector3 directionToCloserVector = (closerVector - bounds.center).normalized;
			Vector3 alteredCloserVector = grid.NodeFromWorldPoint(closerVector + (directionToCloserVector * grid.nodeRadius*2)).worldPosition;   
			List<Vector3> comparisonList = new List<Vector3>() {alteredCloserVector, fartherVector};
			// print("new node positions");
			// print(comparisonList[1] + "   "  + comparisonList[0]  );

			return comparisonList;
		}
		else 
		{
			// print("the distances are about the same, the initially found node are correct");
			// print("final node positions");
			// print(newEnds[0] + "   "  + newEnds[1]  );
			return newEnds;
		}
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
	private List<GameObject> SpawnPrimativeAtPoints(List<Vector3> points, PrimitiveType type)
	{
		List<GameObject> goList  = new List<GameObject>();
		for (int i = 0; i < points.Count; i++)
		{
			GameObject go = GameObject.CreatePrimitive(type);
			go.transform.localScale *= 0.5f;
			go.transform.position = points[i];
			goList.Add(go);
		}
		return goList;
	}



	//spawns walls along the forward axis of a passage
	public void SpawnWallTiles(Passage passage, int numTilesHigh)
	{
		// Vector3 adjustedTileScale = passageWallPrefab.transform.localScale;
		// adjustedTileScale.x = grid.nodeRadius*2; 	//adjusting these so that they will be one grid node wide(x) and tall(y)
		// adjustedTileScale.y = grid.nodeRadius*2;
		// passageWallPrefab.transform.localScale = adjustedTileScale;


		List <Vector3> ends = GetEnds(passage.thisCollider.bounds);  //no need to call GetEndNodePositions() because this already should be snapped
		float lengthOfPassage = Vector3.Distance(ends[0], ends[1]);
		int numTiles = Mathf.RoundToInt(lengthOfPassage/(grid.nodeRadius*2));

		Vector3 wallStart1 = rearVector + this.transform.right * ((grid.nodeRadius * 2) * passageWidth)  + new Vector3(0f,grid.nodeRadius,0f);
		Vector3 wallStart2 = rearVector - this.transform.right * ((grid.nodeRadius * 2) * passageWidth) + new Vector3(0f,grid.nodeRadius,0f);

		List<Vector3> wallStarts = new List<Vector3>() { wallStart1,wallStart2};

		List<Vector3> passageWallRotations = new List<Vector3>() {passage.transform.right*-1, passage.transform.right };

		for (int a = 0; a < wallStarts.Count; a++)
		{
			Vector3 initPos = wallStarts[a];
			Vector3 horizontalPos = initPos;
			Vector3 finalPos = new Vector3();
			Vector3 rotation = passageWallRotations[a];
			
			for (int b = 0; b < numTiles+1; b++)
			{
				finalPos =  horizontalPos;
				for (int c = 0; c < numTilesHigh; c++)
				{
					GameObject go = GameObject.Instantiate(passageWallPrefab, finalPos, Quaternion.identity, wallParent);
					go.transform.forward = rotation;
					finalPos += new Vector3(0f,grid.nodeRadius*2,0f);
				}
				horizontalPos += this.transform.forward * (grid.nodeRadius*2);
			}
		}
		isWallsConstructed = true;


		/**
		steps: 
			calculate the 2 Vector3s where the walls will start. These will each be passageWidth nodes away from the Passage itself.  
				the walls will always be placed on the local left and right of the passage, and they will be spawned along the Passage's forward direction
			numTiles =  calculate the number of tiles needed to span the length of the passage;  This should be the same as the nodes between.  You could get the distance between the ends and then use division and divide by nodeRadius/2
			foreach side of the passage : this will always be two, and the width will be determined by passageWidth;
				
					for (int i=0; i < numTiles; i+)
						for (int i=0; i < NumTilesHigh; i+)    //numTilesHigh will probably be between 2 and...5 maybe
		**/
	}


	public Vector3 GetRearVector(Passage passage)
	{
		Vector3 forwardDirection = this.transform.forward;
		List<Vector3> ends = GetEnds(thisCollider.bounds);
		Vector3 rearVector = new Vector3(999,999,999);

		//need to calculate the normalized vector from each end pointing to the center.  Whichever one is pointing the same way as the forward directiong of the passage is the rear vector

		foreach (Vector3 V in ends)
		{
			Vector3 endToCenter = (passage.thisCollider.bounds.center - V).normalized;
			if (endToCenter == forwardDirection)
			{
				rearVector = V;
				
			}
		}
		if (rearVector.x == 999)
		{
			Debug.LogError("logic is wrong, no rear vector is assigned");
		}
		return rearVector;
	}





}
