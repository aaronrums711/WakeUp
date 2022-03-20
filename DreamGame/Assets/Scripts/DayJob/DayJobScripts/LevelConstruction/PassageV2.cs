using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PassageV2 : MonoBehaviour
{
	/*****************
	CreateDate: 	3/18/22
	Functionality:	repesents one passage.  Draws a gizmo ray between the two PassageEnd parents
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public List<PassageEnd> ends;
	public List<Vector3> endPositions {get{ return GetEndPositions();}}
	[SerializeField] private GameObject endPrefab;
	public GameObject passageWallPrefab;
	public int passageWidth = 2;   //this is the passage width, IN GRID NODES, not world space
	public Transform wallParentObject;
	public int numWallTilesHigh = 3;
	public Vector3 passageDirection;
	
	//////////////////////////////State
	public bool isWallsConstructed;
	
	//////////////////////////////Cached Component References
	[SerializeField] public _Grid grid;
	
	void OnDrawGizmos()
	{
		if (ends.Count == 2)
		{
			Gizmos.DrawLine(endPositions[0], endPositions[1]);
		}				
		
	}
	
	
    void Start()
    {
		grid = FindObjectOfType<_Grid>();
		for(int i = 0; i < this.transform.childCount; i++)
		{
			if (this.transform.GetChild(i).name == "Walls")
			{
				wallParentObject = this.transform.GetChild(i).gameObject.transform;
			}
		}

    }

    void Update()
    {
		SetEndRotations();
    }

	private List<Vector3> GetEndPositions()
	{
		List<Vector3> endPositions = new List<Vector3>();
		for (int i = 0; i < ends.Count; i++)
		{
			endPositions.Add(ends[i].transform.position);
		}
		return endPositions;
	}

	private void SpawnEnd()
	{
		if (ends.Count < 2)
		{
			for (int i = ends.Count; i < 2; i++)
			{
				GameObject.Instantiate(endPrefab, this.transform.position, Quaternion.identity, this.transform);
			}
		}
	}

	private void SetPosToMidpoint()
	{
		this.transform.position = Vector3.Lerp(endPositions[0], endPositions[1], 0.5f);
	}

	private void SetEndRotations()
	{
		Vector3 target = endPositions[0] - endPositions[1];
		foreach (PassageEnd end in ends)
		{
			end.transform.forward = target;
		}
		ends[1].isRear = true;

	}


	/**
	UPON RETURN:  get the wall spwaning to work on this new PassageV2 object.  shouldn't be too hard
	**/

	public void SpawnWallTiles(PassageV2 passage, int numTilesHigh)
	{
		Vector3 rearVector = GetRearPassageVector();
		List <Vector3> ends = endPositions;  
		float lengthOfPassage = Vector3.Distance(ends[0], ends[1]);
		int numTiles = Mathf.RoundToInt(lengthOfPassage/(grid.nodeRadius*2));
																											//bumps the wall up slightly		//moves the walls over so they fall exactly on the grid lines
		Vector3 wallStart1 = rearVector + this.ends[0].transform.right * ((grid.nodeRadius * 2) * passageWidth)  + new Vector3(0f,grid.nodeRadius,0f) + (this.ends[0].transform.right* grid.nodeRadius);
		Vector3 wallStart2 = rearVector - this.ends[0].transform.right * ((grid.nodeRadius * 2) * passageWidth) + new Vector3(0f,grid.nodeRadius,0f) - (this.ends[0].transform.right* grid.nodeRadius);

		List<Vector3> wallStarts = new List<Vector3>() { wallStart2,wallStart1};

																	//it doesn't matter which PassageEnd we use to get this rotation because they both face the same direction
		List<Vector3> passageWallRotations = new List<Vector3>() {this.ends[0].transform.right*-1, this.ends[0].transform.right };

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
					GameObject go = GameObject.Instantiate(passageWallPrefab, finalPos, Quaternion.identity, this.transform);
					go.transform.forward = rotation;
					finalPos += new Vector3(0f,grid.nodeRadius*2,0f);
				}
				horizontalPos += this.ends[0].transform.forward * (grid.nodeRadius*2);
			}
		}
		isWallsConstructed = true;

	}

	[ContextMenu("SpawnWallTilesV2")]
	public void SpawnWallsFromMenu()
	{
		SpawnWallTiles(this, numWallTilesHigh);

	}


	private Vector3 GetRearPassageVector()
	{
		Vector3 rearVector = new Vector3();
		foreach (PassageEnd end in ends)
		{
			if (end.isRear == true)
			{
				rearVector = end.transform.position;
			}
		}
		return rearVector;
	}
}
