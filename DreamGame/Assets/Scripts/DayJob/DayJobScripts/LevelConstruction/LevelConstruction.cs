using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelConstruction : MonoBehaviour
{
	/*****************
	CreateDate: 	3/11/2022
	Functionality:	this script assists with DayJob level creation.  It can spawn walls along all the passages, delete them, etc.
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public List<PassageV2> allPassages {get{ return  GetAllPassages();}}
	
	[Range(1, 10)]public int wallHeight;
	public _Grid grid;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
    void Start()
    {
        grid = FindObjectOfType<_Grid>();
    }


	private List<PassageV2> GetAllPassages()
	{
		List<PassageV2> allPassages = GameObject.FindObjectsOfType<PassageV2>().ToList();
		return allPassages;
	}


	[ContextMenu("SpawnWallsOnAllPassages()")]
	public void SpawnWallsOnAllPassages()
	{
		if (grid.nodeGrid == null) {grid.CreateGrid();}
		AssignGridtoPassages();
		foreach (PassageV2 p in allPassages)
		{
			p.SpawnWallTiles(p, p.numWallTilesHigh);
		}
	}

	[ContextMenu("DestroyAllWalls()")]
	public void DestroyAllWalls()
	{
		List<PassageWall> walls = GameObject.FindObjectsOfType<PassageWall>().ToList();
		foreach (PassageWall wall in walls)
		{
			DestroyImmediate(wall.gameObject);
		}
	}

	[ContextMenu("AssignGridtoPassages()")]
	public void AssignGridtoPassages()
	{
		foreach (PassageV2 p in GetAllPassages())
		{
			if (p.grid == null)
			{
				p.grid = grid;
			}
		}
	}

	[ContextMenu("Reset()")]
	private void Reset()
	{
		DestroyAllWalls();
		SpawnWallsOnAllPassages();
		CreateAllOpenings();
	}

	[ContextMenu("CreateAllOpenings()")]
	public void CreateAllOpenings()
	{
		List<PassageOpening> openings = GameObject.FindObjectsOfType<PassageOpening>().ToList();
		print("openings found: " + openings.Count);
		foreach (PassageOpening p in openings)
		{
			p.CreateOpening();
		}
	}

	
}
