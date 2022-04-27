using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
	public Vector3 Midpoint {get{return GetPassageMidpoint();}}
	public int passageWidth = 2;   //this is the passage width, but it's not really exact.  
	[SerializeField] private int passageNodeWidth;   //remove this from inspector later
	public int numWallTilesHigh = 3;
	List<PassageOpening> openings = new List<PassageOpening>();
	

	//////////////////////////////State
	public bool isWallsConstructed;
	public bool forwardEndCap;
	public bool rearEndCap;
	public MovementOrientation movementOrientation;
	

	//////////////////////////////Cached Component References
	[HideInInspector] public _Grid grid;
	public GameObject passageOpeningPrefab;
	public GameObject passageWallPrefab;
	private Transform wallParent; 
	private Transform openingParent;
	private Transform forwardEndCapParent;
	private Transform RearEndCapParent;
	public Transform levelCenter;
	public List<MovementOrientation> allMO;
	
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
		openingParent = Utils.SearchByNameFromParent("Openings", this.transform);
		forwardEndCapParent = Utils.SearchByNameFromParent("ForwardEndCaps", this.transform);
		RearEndCapParent  = Utils.SearchByNameFromParent("RearEndCaps", this.transform);
		levelCenter = GameObject.Find("LevelCenter").transform;
		AssignMoveOrientation();
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
		opening.transform.parent = openingParent;
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
			PassageEnd forwardEnd = ends[0];  //defualt, to avoid "new..." warning
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
					
					GameObject endCapWall = Instantiate(passageWallPrefab, spawnPos, Quaternion.identity, forwardEndCapParent);
					endCapWall.transform.forward = forwardEnd.transform.forward;
					spawnPos +=  new Vector3(0, grid.nodeRadius*2, 0);
					
				}
				leftCornerPos += forwardEnd.transform.right * grid.nodeRadius*2;
			}
		}
		else if (!forwardEndCap)
		{
			Utils.DestroyAllChildren(forwardEndCapParent, true);
		}


		if (rearEndCap)
		{
			PassageEnd rearEnd = ends[0];  //defualt, to avoid "new..." warning
			foreach (PassageEnd end in ends)
			{
				if (end.isRear)
				{
					rearEnd = end;
				}
			}

			Vector3 leftCornerPos = rearEnd.transform.position + new Vector3(0, grid.nodeRadius, 0) + ((rearEnd.transform.forward * -1) * grid.nodeRadius) + (rearEnd.transform.right * passageNodeWidth/2 ) + ((rearEnd.transform.right * -1) * grid.nodeRadius);
			for (int a = 0; a < passageNodeWidth; a ++)
			{
				Vector3 spawnPos = leftCornerPos;
				for (int i = 0; i < numWallTilesHigh; i++)
				{
					
					GameObject endCapWall = Instantiate(passageWallPrefab, spawnPos, Quaternion.identity, forwardEndCapParent);
					endCapWall.transform.forward = rearEnd.transform.forward;
					spawnPos +=  new Vector3(0, grid.nodeRadius*2, 0);
				}
				leftCornerPos += (rearEnd.transform.right * -1)* grid.nodeRadius*2;
			}
		}

		else if (!rearEndCap)
		{
			Utils.DestroyAllChildren(RearEndCapParent, true);
		}
	}


	public static PassageV2 FindObjectBetweenPassages(string layerName, List<PassageV2> passagesToIgnore)
	{
		Transform passageParent = GameObject.Find("Passages").transform;
		List<PassageV2> allPassages = passageParent.transform.GetComponentsInChildren<PassageV2>().ToList();
		foreach (PassageV2 p in passagesToIgnore)
		{
			allPassages.Remove(p);
		}
		int layerIndex = LayerMask.NameToLayer(layerName);
		int layerMask = (1 << layerIndex);
		PassageV2 _passage = allPassages[0];
		bool isPlayerHit = false;

		if (layerIndex == -1)
		{
			Debug.LogError("layer not found, returning first passage found");
		}
		else
		{
			
			foreach (PassageV2 passage in allPassages)
			{
				List<Vector3> endPoints = passage.GetEndPositionsExtended();
				endPoints[0] += new Vector3(0,1,0);
				endPoints[1] += new Vector3(0,1,0);
				if (Physics.Linecast(endPoints[0], endPoints[1], out RaycastHit hitInfo, layerMask))
				{
					Debug.DrawLine(endPoints[0], endPoints[1], Color.red, 5f);
					_passage = passage;
					isPlayerHit = true;
					break;
				}	
				else
				{
					Debug.DrawLine(endPoints[0], endPoints[1], Color.black, 5f);
				}
			}
		}
		if(isPlayerHit == false) { Debug.LogWarning("player was not found");}
		return _passage;
	}

	public List<Vector3> GetEndPositionsExtended()
	{
		Vector3 passageForward = ends[0].transform.forward;
		
		Vector3 end1 = endPositions[0] + passageForward * passageWidth*3;
		Vector3 end2 = endPositions[1] + (passageForward* -1 ) * passageWidth*3;

		List<Vector3> endsExtended = new List<Vector3>() {end1, end2};
		return endsExtended;
	}

	private Vector3 GetPassageMidpoint()
	{
		Vector3 center = Vector3.Lerp(endPositions[0], endPositions[1], 0.5f);
		return center;
	}

	/**
	if the hallway is traveling in the same direction as the LevelCenter forward, 
		then the camera is going to be facing LevelCenter.right or negative right  (perpendicular to the direction of the hallway)
	
	right or negative right will be determined by if the center of the hallway is to the right or to the left of the LevelCenter
	**/

	[ContextMenu("AssignMoveOrientation()")]
	public void AssignMoveOrientation()
	{
		Vector3 passageDirectionBase = endPositions[0] - endPositions[1];
		//rounding and getting Abs value so it doesn't matter which order the end positions are in
		Vector3 passageDirection = new Vector3(Mathf.Abs(Mathf.Round(passageDirectionBase.x)), Mathf.Abs(Mathf.Round(passageDirectionBase.y))  , Mathf.Abs(Mathf.Round(passageDirectionBase.z)));
		passageDirection = passageDirection.normalized;

		if (passageDirection == levelCenter.forward)
		{
			//camera movement will be either 270 or 90.  This hallway should be parallel to the levelCenter.forward
			Vector3 midpoint = new Vector3 (this.Midpoint.x, 0, this.Midpoint.z);
			//scoot up the leveCenter to be equal on the Z axis.  now it will be easy to compare the two
			Vector3 levelCenterAdjusted = new Vector3(levelCenter.position.x, 0, midpoint.z);
			Vector3 midpointToCenter = (levelCenterAdjusted - midpoint).normalized;
														// Utils.SpawnPrimativeAtPoints(new List<Vector3>() {midpoint, levelCenterAdjusted}, PrimitiveType.Sphere);
														// print("direction from passage midpoint to adjusted center:  " + midpointToCenter);

			List<MovementOrientation> newMO = 
				(from mo in allMO
				where mo.cameraViewingDirection == midpointToCenter
				select mo).ToList();

			if (newMO.Count > 1)
			{
				Debug.Log("something is wrong, this query should only ever return one MovementOrientation");
			}
			movementOrientation = newMO[0];
		}
		else 
		{
			//camera movement will be either 0 or 180.  this hallway should be perpendicular to the levelCenter.forward
			Vector3 midpoint = new Vector3 (this.Midpoint.x, 0, this.Midpoint.z);
			Vector3 levelCenterAdjusted = new Vector3(midpoint.x, 0, levelCenter.position.z);
			Vector3 midpointToCenter = (levelCenterAdjusted - midpoint).normalized;
			List<MovementOrientation> newMO = 
				(from mo in allMO
				where mo.cameraViewingDirection == midpointToCenter
				select mo).ToList();

			if (newMO.Count > 1)
			{
				Debug.Log("something is wrong, this query should only ever return one MovementOrientation");
			}
			movementOrientation = newMO[0];
		}
	}


}
