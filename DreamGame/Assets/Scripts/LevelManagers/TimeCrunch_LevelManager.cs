using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCrunch_LevelManager : LevelManager, ILevelMover
{
	/*****************
	CreateDate: 	10/24/21
	Functionality:	handles mini game spawning and other tasks that are specific to the Time Crunch level type
	Notes:			there will be one child level manager for each level type.  Each child will implement the same interface, but imlement the method differently of course, depending on the needs of that level type. 
	Dependencies:	LevelManager
	******************/
	
	//////////////////////////////Config
	public LevelType managingLevelType;
	private float lagBetweenGames; 
	public List<Vector3> miniGameSpawnPoints; //VGIU

	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
	void Awake()
    {
        
    }
	
    void Start()
    {
		lagBetweenGames = thisLevel.lagBetweenGamesSeconds;
        LoadMiniGames(thisLevel.totalGamesInLevel);
		StartCoroutine(SpawnGames(lagBetweenGames, thisLevel.totalGamesInLevel));
    }

    void Update()
    {
        
    }

	public void MoveLevelForward()
	{
		return;
	}

	private IEnumerator SpawnGames(float secondsBetween, int numGames)
	{
		float startTime = Time.time;
		Instantiate(gamesForThisLevel[0], miniGameSpawnPoints[0], Quaternion.identity);
		float timeElapsed = 0f;
		for (int i = 1; i <= thisLevel.totalGamesInLevel; i++)
		{
			while(timeElapsed < secondsBetween)
			{
				yield return new WaitForSeconds(1f);
				timeElapsed += 1f;
			}
			timeElapsed = 0;
			MiniGame newMiniGame = Instantiate(gamesForThisLevel[i], miniGameSpawnPoints[i], Quaternion.identity);
			newMiniGame.gameObject.SetActive(true);
			newMiniGame.orderInLevel = i;
		}
		
		/**
		spawn first game immediately
		for each remaining game...
			wait secondsBetween amount of seconds
			spawn game
			set transform
			set orderInLevel for that game
			enable it
		**/
		yield return new WaitForSeconds(1f);
	}


	
}

