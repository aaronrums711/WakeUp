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
		float spawnX = startingPosition.position.x;
		float spawnY = startingPosition.position.y;

		//destroy any stuff that was made in debug
		for(int i = 0; i < objectParent.childCount; i++)
		{
			Destroy(objectParent.GetChild(i).gameObject);
		}


		for (int x = 0; x < numColumns; x++)
		{

			for (int y=0; y < numRows; y++)
			{
				Vector3 spawnPos;
				if (x == 0 && y == 0) //if its the very first iteration
				{
					spawnPos = startingPosition.position;
				}
				else 
				{
					spawnPos = new Vector3(spawnX, spawnY, 0);
				}
				
				Instantiate(itemToSpawn, spawnPos, Quaternion.identity);
				lastSpawn = spawnPos;
				spawnX += XSpacing;
				
			}

			spawnY += YSpacing;
		}


	}
}
