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
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
	void Awake()
    {
        
    }
	
    void Start()
    {
        
    }

    void Update()
    {
        
    }

	private List<Passage> GetAllPassages()
	{
		List<Passage> allPassages = GameObject.FindObjectsOfType<Passage>().ToList();
		return allPassages;
	}

	[ContextMenu("AllignAllPassages()")]
	public void AllignAllPassages()
	{
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
		foreach (Passage p in GetAllPassages())
		{
			p.SpawnWallTiles(p, wallHeight);
		}
	}

	[ContextMenu("DestroyAllWalls()")]
	public void DestroyAllWalls()
	{
		// if(GetAllPassages().Any())
		// {
		// 	foreach (Passage p in GetAllPassages())
		// 	{
		// 		print("child count: " + p.wallParent.childCount);
		// 		foreach (Transform child in p.wallParent)
		// 		{
		// 			DestroyImmediate(child.gameObject);
		// 		}
		// 	}
		// }

		List<PassageWall> walls = GameObject.FindObjectsOfType<PassageWall>().ToList();
		foreach (PassageWall wall in walls)
		{
			DestroyImmediate(wall.gameObject);
		}



	}

}
