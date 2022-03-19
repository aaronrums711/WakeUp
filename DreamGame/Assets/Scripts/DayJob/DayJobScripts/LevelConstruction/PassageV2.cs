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
	private List<Vector3> endPositions {get{ return GetEndPositions();}}
	[SerializeField] private GameObject endPrefab;
	
	//////////////////////////////State
	public bool isWallsConstructed;
	
	//////////////////////////////Cached Component References
	
	void OnDrawGizmos()
	{
		if (ends.Count == 2)
		{
			Gizmos.DrawLine(endPositions[0], endPositions[1]);
		}				
		
	}
	
	void Awake()
    {
        
    }
	
    void Start()
    {
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
	}




	// 	[ContextMenu("SpawnWallTiles")]
	// public void SpawnWallTiles(Passage passage, int numTilesHigh)
	// {
	// 	// Vector3 adjustedTileScale = passageWallPrefab.transform.localScale;
	// 	// adjustedTileScale.x = grid.nodeRadius*2; 	//adjusting these so that they will be one grid node wide(x) and tall(y)
	// 	// adjustedTileScale.y = grid.nodeRadius*2;
	// 	// passageWallPrefab.transform.localScale = adjustedTileScale;


	// 	List <Vector3> ends = GetEnds(passage.thisCollider.bounds);  //no need to call GetEndNodePositions() because this already should be snapped
	// 	float lengthOfPassage = Vector3.Distance(ends[0], ends[1]);
	// 	int numTiles = Mathf.RoundToInt(lengthOfPassage/(grid.nodeRadius*2));
	// 																										//bumps the wall up slightly		//moves the walls over so they fall exactly on the grid lines
	// 	Vector3 wallStart1 = rearVector + this.transform.right * ((grid.nodeRadius * 2) * passageWidth)  + new Vector3(0f,grid.nodeRadius,0f) + (this.transform.right* grid.nodeRadius);
	// 	Vector3 wallStart2 = rearVector - this.transform.right * ((grid.nodeRadius * 2) * passageWidth) + new Vector3(0f,grid.nodeRadius,0f) - (this.transform.right* grid.nodeRadius);

	// 	List<Vector3> wallStarts = new List<Vector3>() { wallStart1,wallStart2};

	// 	List<Vector3> passageWallRotations = new List<Vector3>() {passage.transform.right*-1, passage.transform.right };

	// 	for (int a = 0; a < wallStarts.Count; a++)
	// 	{
	// 		Vector3 initPos = wallStarts[a];
	// 		Vector3 horizontalPos = initPos;
	// 		Vector3 finalPos = new Vector3();
	// 		Vector3 rotation = passageWallRotations[a];
			
	// 		for (int b = 0; b < numTiles+1; b++)
	// 		{
	// 			finalPos =  horizontalPos;
	// 			for (int c = 0; c < numTilesHigh; c++)
	// 			{
	// 				GameObject go = GameObject.Instantiate(passageWallPrefab, finalPos, Quaternion.identity, wallParent);
	// 				go.transform.forward = rotation;
	// 				finalPos += new Vector3(0f,grid.nodeRadius*2,0f);
	// 			}
	// 			horizontalPos += this.transform.forward * (grid.nodeRadius*2);
	// 		}
	// 	}
	// 	isWallsConstructed = true;


}
