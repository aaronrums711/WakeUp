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

	[Range(0.01f,2)]
	public float YSpacing;

	[Range(0.01f,2)]
	public float XSpacing;

	[Range(3, 100)]
	public int numColumns;

	[Range(3, 100)]
	public int numRows;

	[Range(0.1f, 1.5f)]
	public float scaleMultiplier;

	private Vector3 lastSpawn;
	

	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
    void Start()
    {
        lastSpawn = startingPosition.position;
		DeleteExistingItems();
        SpawnItems();
    }

	public void SpawnItems()
	{

		float spawnX = startingPosition.position.x;
		float spawnY = startingPosition.position.y;

		itemToSpawn.transform.localScale = new Vector3(1,1,1);  //set it back, because the scale change persist into the editor
		itemToSpawn.transform.localScale *= scaleMultiplier;



		for (int x = 0; x < numRows; x++)
		{

			for (int y=0; y < numColumns; y++)
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
				
				Instantiate(itemToSpawn, spawnPos, itemToSpawn.transform.rotation, objectParent);
				lastSpawn = spawnPos;
				spawnX += XSpacing;
				
			}
			spawnX =startingPosition.position.x;  //reset it, because its a new row

			spawnY += YSpacing;
		}


	}

	public void DeleteExistingItems()
	{
		// destroy any stuff that was made in debug
		for(int i = 0; i < objectParent.childCount; i++)
		{
			Destroy(objectParent.GetChild(i).gameObject);
		}
	}

	[ContextMenu("Respawn")]
	public void DeleteAndRespawn()
	{
		lastSpawn = startingPosition.position;
		DeleteExistingItems();
        SpawnItems();
	}

}
