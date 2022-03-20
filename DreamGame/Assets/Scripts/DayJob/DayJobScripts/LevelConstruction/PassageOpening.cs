using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteAlways]
public class PassageOpening : MonoBehaviour
{
	/*****************
	CreateDate: 	3/12/2022
	Functionality:	this class creates openings in walls as it slides up and down the forward direction of a passage
	Notes:			as of 3/13, this only creates an opening in the exact column that is hit by the raycast.  The ability to destroy multiple columns of wall tiles could definitely be implemented...I'm just tired
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public bool left;
	public bool right;
	public int openingWidth = 2;
	public PassageV2 attachedPassage;
	[Range(0f, 1f)] public float posAlongPath;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	void OnDrawGizmosSelected()
	{
		Vector3 additionVector = new Vector3(0, 1, 0);
		foreach (Vector3 V in GetDirections())
		{
			Debug.DrawRay(this.transform.position + additionVector, V * 3, Color.black, 0.1f);
		}
	}
	
	void Awake()
    {
        
    }
	
    void Start()
    {
        SetPosition();
    }

    void Update()
    {
		SetPosition();
		// CreateOpening();
    }

	[ContextMenu("CreateOpening()")]
	public  void CreateOpening()
	{
		float raycastLength = attachedPassage.passageWidth * (attachedPassage.grid.nodeRadius *2) +2;  //+2 is just a failsafe
		foreach (Vector3 V in GetDirections())
		{
			if (Physics.Raycast(this.transform.position,  V ,out RaycastHit hitInfo, raycastLength))
			{
				if (hitInfo.transform.TryGetComponent<PassageWall> (out PassageWall hitWall))  //if we hit a wall, destroy the above tiles as well based on the wall heigh
				{
					Vector3 initialHitPos = hitInfo.transform.position;
					List<Vector3> columnsToCheck = new List<Vector3>();
					Vector3 horizontalAdditionVector = new Vector3((attachedPassage.grid.nodeRadius*2), 0, 0);
					columnsToCheck.Add(initialHitPos);

					for (int a = 1; a < openingWidth; a++)
					{
						columnsToCheck.Add(this.transform.forward * ((attachedPassage.grid.nodeRadius*2) * a));
					}

					// DestroyImmediate(hitWall.gameObject);
					// float boxRadius = attachedPassage.grid.nodeRadius/2;
					// for (int i =1; i < attachedPassage.numWallTilesHigh; i++)
					// {
					// 	Vector3 additionVector = new Vector3(0, (attachedPassage.grid.nodeRadius*2) * i, 0);
					// 	Collider[] colls = Physics.OverlapBox(initialHitPos + additionVector, new Vector3(boxRadius, boxRadius, boxRadius));
					// 	if (colls.Length > 0)  //the count should really ever only be 1 or 0
					// 	{
					// 		foreach (Collider coll in colls )
					// 		{
					// 			DestroyImmediate(coll.gameObject);
					// 		}
					// 	}
					// }
					// DestroyAbove(columnsToCheck);
					CreateOpeningFromHit(initialHitPos, openingWidth, attachedPassage.numWallTilesHigh);

				}
			}
		}
	}

	private void SetPosition()
	{
		if (attachedPassage != null)
		{
			List<Vector3> ends = attachedPassage.endPositions;
			this.transform.position = Vector3.Lerp(ends[0], ends[1], posAlongPath);
		}
	}

	public List<Vector3> GetDirections()
	{
		List<Vector3> directions = new List<Vector3>();
		if (left == true)
		{
			directions.Add(-this.transform.right);
		}
		if (right == true)
		{
			directions.Add(this.transform.right);
		}
		return directions;
	}

	//this is just a convenience method to tidy up the CreateOpening() method
	//for each of the bottom positions, for each wallHeight, check Physics.Overlap to check if a wall tile is there, and if it is, destroy it
	public void CreateOpeningFromHit(Vector3 initialPosition, int horizontal, int vertical)
	{		
		float boxRadius = attachedPassage.grid.nodeRadius/2;  //we want this to be small enough to only detect one wall tile, and not have any chance of accidentally detecting 2 or more
		float multiplier = attachedPassage.grid.nodeRadius*2;

		for (int a = 0; a < horizontal; a++)
		{
			Vector3 horizontalPos = initialPosition + (this.transform.forward * (multiplier * a));

			for (int b = 0; b < vertical; b++)
			{
				Vector3 verticalPos = horizontalPos + new Vector3(0,multiplier * b, 0);
				Collider[] colls = Physics.OverlapBox(verticalPos, new Vector3(boxRadius, boxRadius, boxRadius));

				if (colls.Length > 0)  //the count should really ever only be 1 or 0
				{
					foreach (Collider coll in colls )
					{
						DestroyImmediate(coll.gameObject);
					}
				}
			}
		}

	}
}
