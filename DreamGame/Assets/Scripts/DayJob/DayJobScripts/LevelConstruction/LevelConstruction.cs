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
	public List<Passage> allPassages {get{ return  GetAllPassages();}}
	
	[Range(1, 10)]public int wallHeight;
	public _Grid grid;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
    void Start()
    {
        grid = FindObjectOfType<_Grid>();
    }

    void Update()
    {
        
    }

	private List<Passage> GetAllPassages()
	{
		List<Passage> allPassages = GameObject.FindObjectsOfType<Passage>().ToList();
		print("passage count: " + allPassages.Count);
		return allPassages;
	}

	[ContextMenu("AllignAllPassages()")]
	public void AllignAllPassages()
	{
		if (grid.nodeGrid == null) {grid.CreateGrid();}
		AssignGridtoPassages();
		foreach (Passage p in GetAllPassages())
		{
			p.SnapRotation();
			p.SnapPosition();
			p.SnapScale();
			p.SnapScale();
		}
	}

	[ContextMenu("SpawnWallsOnAllPassages()")]
	public void SpawnWallsOnAllPassages()
	{
		if (grid.nodeGrid == null) {grid.CreateGrid();}
		AssignGridtoPassages();
		foreach (Passage p in GetAllPassages())
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

	public void AssignGridtoPassages()
	{
		foreach (Passage p in GetAllPassages())
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
		// yield return new WaitForSeconds(0.5f);   //these WaitForSeconds dont work out of play mode
		AllignAllPassages();
		// yield return new WaitForSeconds(1f);
		AllignAllPassages();
		// yield return new WaitForSeconds(1f);
		SpawnWallsOnAllPassages();
		// yield return new WaitForSeconds(1f);
	}

	
	// public void CallReset()
	// {
	// 	StartCoroutine(Reset());
	// }
}
