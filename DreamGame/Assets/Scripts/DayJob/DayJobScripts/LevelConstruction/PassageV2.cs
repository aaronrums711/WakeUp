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
	public int passageWidth = 2;   //this is the passage width, but it's not really exact.  
	[SerializeField] private int passageNodeWidth;   //remove this from inspector later
	public int numWallTilesHigh = 3;
	List<PassageOpening> openings = new List<PassageOpening>();

	//////////////////////////////State
	public bool isWallsConstructed;
	public bool forwardEndCap;
	public bool rearEndCap;
	
	//////////////////////////////Cached Component References
	public _Grid grid;
	public GameObject passageOpeningPrefab;
	// [SerializeField] private GameObject endPrefab;
	private Transform wallParent; 
	public GameObject passageWallPrefab;
	
	void OnDrawGizmos()
	{
		if (ends.Count == 2)
		{
			Gizmos.DrawLine(endPositions[0], endPositions[1]);
		}				
		
	}
	
	
    void Start()
    {
		// this.transform.parent = GameObject.Find("Passages").transform;  //
		this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
		grid = FindObjectOfType<_Grid>();
		wallParent = Utils.SearchByNameFromParent("Walls", this.transform);

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

	private void SetEndRotations()
	{
		Vector3 target = endPositions[0] - endPositions[1];
		foreach (PassageEnd end in ends)
		{
			end.transform.forward = target;
		}
		ends[1].isRear = true;

	}

	public void SpawnWallTiles(PassageV2 passage, int numTilesHigh)
	{
		if (this.isWallsConstructed)
		{
			if (wallParent != null)
			{
				Utils.DestroyAllChildren(wallParent, true);
				isWallsConstructed = false;
			}
		}
		Vector3 rearVector = GetRearPassageVector();
		List <Vector3> ends = endPositions;  
		float lengthOfPassage = Vector3.Distance(ends[0], ends[1]);
		int numTiles = Mathf.RoundToInt(lengthOfPassage/(grid.nodeRadius*2));
																													//bumps the wall up slightly		//moves the walls over so they fall exactly on the grid lines
		Vector3 wallStart1 = rearVector + this.ends[0].transform.right * ((grid.nodeRadius * 2) * passageWidth)  + new Vector3(0f,grid.nodeRadius,0f) + (this.ends[0].transform.right* grid.nodeRadius);
		Vector3 wallStart2 = rearVector - this.ends[0].transform.right * ((grid.nodeRadius * 2) * passageWidth) + new Vector3(0f,grid.nodeRadius,0f) - (this.ends[0].transform.right* grid.nodeRadius);
		passageNodeWidth = Mathf.RoundToInt(Vector3.Distance(wallStart1, wallStart2) / (grid.nodeRadius*2));
		

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
					GameObject go = GameObject.Instantiate(passageWallPrefab, finalPos, Quaternion.identity, wallParent);
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


	[ContextMenu("CreateOpening()")]
	public GameObject CreateOpening()
	{
		GameObject go = Instantiate(passageOpeningPrefab);
		PassageOpening opening = go.GetComponent<PassageOpening>();
		opening.attachedPassage = this;
		openings.Add(opening);
		opening.transform.parent = this.transform;
		opening.transform.forward = ends[0].transform.forward;
		opening.openingWidth = 2;
		
		return go;
	}


	/***
		need functionality of two toggles, one for each end of the passage. 
			when toggled on, walls will spawn at the end of the hall to cap it off.  When toggled off, they will be destroyed

		if the passageEnd.isRear, then move back have a unit and spawn there
			if not, move forward half a unit


		if its the forwardEnd, to get the corner position...
			scoot it local forward  half a unit   (forwardEnd.transform.forward * grid.nodeRadius)
			scoot it up half a unit 				+ new Vector3(0, grid.nodeRadius, 0);
			then move it local left * passageNodeWidth/2
			then move it local right * grid.nodeRadius

	****/

	[ContextMenu("SpawnEndCap()")]
	public void SpawnEndCap()
	{
		if (forwardEndCap)
		{
			PassageEnd forwardEnd = new PassageEnd();
			foreach (PassageEnd end in ends)
			{
				if (!end.isRear)
				{
					forwardEnd = end;
				}
			}
			Vector3 leftCornerPos = forwardEnd.transform.position + new Vector3(0, grid.nodeRadius, 0) + (forwardEnd.transform.forward * grid.nodeRadius) + ((forwardEnd.transform.right * -1) * passageNodeWidth/2 ) + (forwardEnd.transform.right * grid.nodeRadius);
			
			//just spawn one column vertically to start
			for (int a = 0; a < passageNodeWidth; a ++)
			{
				Vector3 spawnPos = leftCornerPos;
				for (int i = 0; i < numWallTilesHigh; i++)
				{
					
					GameObject endCapWall = Instantiate(passageWallPrefab, spawnPos, Quaternion.identity, wallParent);
					endCapWall.transform.forward = forwardEnd.transform.forward;
					spawnPos +=  new Vector3(0, grid.nodeRadius*2, 0);
					
				}
				leftCornerPos += forwardEnd.transform.right * grid.nodeRadius*2;
			}
		}
		else if (!forwardEndCap)
		{
			//delete all 
		}
		if (rearEndCap)
		{
			//spawn rear end cap
		}
	}
}
