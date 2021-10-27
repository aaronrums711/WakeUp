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
	public List<Vector3> miniGameSpawnPoints4; //VGIU. each of these represents the locations of mini games for levels with 4, 3 and 2 total games
	public List<Vector3> miniGameSpawnPoints3; //VGIU
	public List<Vector3> miniGameSpawnPoints2; //VGIU

	public Transform activeMiniGameParent;

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
		activeMiniGameParent = GameObject.Find("ActiveMiniGames").GetComponent<Transform>();
		if (activeMiniGameParent == null)
		{Debug.LogError("ActiveMiniGame parent not found, that's an issue");}

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
		MiniGame firstMiniGame = Instantiate(gamesForThisLevel[0], miniGameSpawnPoints4[0], Quaternion.identity, activeMiniGameParent);
		firstMiniGame.gameObject.SetActive(true);
		firstMiniGame.orderInLevel = 1;

		float timeElapsed = 0f;
		for (int i = 1; i < thisLevel.totalGamesInLevel; i++)
		{
			while(timeElapsed < secondsBetween)
			{
				yield return new WaitForSeconds(1f);
				timeElapsed += 1f;
			}
			timeElapsed = 0;
			MiniGame newMiniGame = Instantiate(gamesForThisLevel[i], miniGameSpawnPoints4[i], Quaternion.identity, activeMiniGameParent);
			newMiniGame.gameObject.SetActive(true);
			newMiniGame.orderInLevel = i+1;
		}

		print("all games have been spawned");
		
		/**
		spawn first game immediately
		for each remaining game...
			wait secondsBetween amount of seconds
			spawn game
			set transform
			set orderInLevel for that game
			enable it
			move camera


		later on there needs to be more polish to this, all games pausing, the new game appearing, a countdown with visual effect, then all games restarting

		**/
	}


	
}

