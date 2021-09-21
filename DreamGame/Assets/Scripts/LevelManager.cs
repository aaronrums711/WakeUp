using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	/*****************
	CreateDate: 	9/21/21	
	Functionality:	observes all of the mini games in the level, as well as the level timer, and determines if/when the level is beaten. 
	Notes:			
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public Level thisLevel;
	public int gamesNeededToWin;
	public int currentGamesWon;
	// public List<MiniGame> allMinigames;
	public MiniGame[] miniGames;

	//////////////////////////////State
	public bool isLevelComplete = false;
	public bool isLevelWon = false;
	
	//////////////////////////////Cached Component References
	
	
    void Start()
    {
         gamesNeededToWin = thisLevel.minGamesNeededToWin;
		 miniGames = FindObjectsOfType(typeof(MiniGame), true);
    }

    void Update()
    {
        
    }

	// public List<MiniGame> GetAllMiniGamesInLevel()
	// {
	// 	// MiniGame[] miniGames =  new MiniGame[thisLevel.totalGamesInLevel];
		
	// 	miniGames = FindObjectsOfType(typeof(MiniGame), true);
	// }



}
