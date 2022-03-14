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
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public bool left;
	public bool right;
	public int openingWidth;
	public Passage attachedPassage;
	[Range(0f, 1f)] public float posAlongPath;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
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
		CreateOpening();
    }

	public void CreateOpening()
	{
		float raycastLength = attachedPassage.passageWidth * (attachedPassage.grid.nodeRadius *2) +2;  //+2 is just a failsafe
		foreach (Vector3 V in GetDirections())
		{
			if (Physics.Raycast(this.transform.position,  V ,out RaycastHit hitInfo, raycastLength))
			{
				if (hitInfo.transform.TryGetComponent<PassageWall> (out PassageWall hitWall))  //if we hit a wall, destroy the above tiles as well based on the wall heigh
				{
					Vector3 initialHitPos = hitInfo.transform.position;
					DestroyImmediate(hitWall.gameObject);
					float boxRadius = attachedPassage.grid.nodeRadius/2;
					for (int i =1; i < attachedPassage.numWallTilesHigh; i++)
					{
						print(initialHitPos);
						Vector3 additionVector = new Vector3(0, (attachedPassage.grid.nodeRadius*2) * i, 0);
						Collider[] colls = Physics.OverlapBox(initialHitPos + additionVector, new Vector3(boxRadius, boxRadius, boxRadius));
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

		
	}

	private void SetPosition()
	{
		if (attachedPassage != null)
		{
			List<Vector3> ends = attachedPassage.Ends;
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
}
