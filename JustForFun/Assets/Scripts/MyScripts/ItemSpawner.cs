using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
	/*****************
	CreateDate: 	1/18/22
	Functionality:
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public Transform objectParent;
	public GameObject itemToSpawn;
	public Transform startingPosition;
	public float YSpacing;
	public float XSpacing;
	public int numColumns;
	public int numRows;

	private Vector3 lastSpawn;

	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
	void Awake()
    {
		lastSpawn = startingPosition.position;
        SpawnItems();
    }
	
    void Start()
    {
        
    }

    void Update()
    {
        
    }

	public void SpawnItems()
	{
		//destroy any stuff that was made in debug
		for(int i = 0; i < objectParent.childCount; i++)
		{
			Destroy(objectParent.GetChild(i));
		}


		for (int x = 0; x < numColumns; x++)
		{
			for (int y=0; y < numRows; y++)
			{
				Vector3 spawnPos;
				if (y == 0)
				{
					spawnPos = startingPosition.position;
				}
				else 
				{
					float newX = lastSpawn.x + XSpacing;
					float newY = lastSpawn.y + YSpacing;
					spawnPos = new Vector3(newX, newY, 0);
				}
				Instantiate(itemToSpawn, spawnPos, Quaternion.identity);
				
			}
		}


	}
}
