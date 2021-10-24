using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	/*****************
	CreateDate: 	9/21/21	
	Functionality:	observes all of the mini games in the level, as well as the level timer, and determines if/when the level is beaten.   There will be children of this class that handle level-type specific tasks, that are NOT universal between all level types
	Notes:			I think all level manager scripts will be on this one game object at all time, but only one will be active to match the level that is being played. 
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public Level thisLevel;
	public int gamesNeededToWin;
	public int currentGamesWon;
	// public List<MiniGame> allMinigames;
	public MiniGame[] miniGames;
	private int executionInterval= 5;

	//////////////////////////////State
	public bool isLevelFinished = false;
	public bool isLevelWon = false;
	
	//////////////////////////////Cached Component References
	
	
    void Start()
    {
        gamesNeededToWin = thisLevel.minGamesNeededToWin;
	 	miniGames = FindObjectsOfType<MiniGame>(true);
    }

    void Update()
    {
		if (Time.frameCount % executionInterval == 0) //only execute it every executionInterval frames.  This probably isn't that necessary...but we'll see
		{
			CheckAllMiniGames();
		}
    }

	public void CheckAllMiniGames()
	{
		int gamesCompleted = 0;
		foreach (MiniGame mg in miniGames)
		{
			if (mg.isComplete)
			{
				gamesCompleted++;
			}
		}
		currentGamesWon = gamesCompleted;

		if (currentGamesWon >= gamesNeededToWin)
		{
			isLevelWon = true;
			isLevelFinished = true;
			print("you've beaten the level, congratulations!");
		}


	}

	public void StopAllMiniGames()
	{
		return;
		/**
		this needs to correctly stop and start all active mini games(mini games that are currently being played)  in the scene using the IStoppable interface members.   
		this will be done for pausing, briefly when a mini game is added, and potentially for other future reasons as well
		**/
	}

}
