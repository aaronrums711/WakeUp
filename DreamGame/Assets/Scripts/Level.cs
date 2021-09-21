using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new level", menuName = "level")]
public class Level : ScriptableObject
{
	/*****************
	CreateDate: 	9/3/21
	Functionality:	contains methods and attributes for each available level option. 
	Notes:			The best way to implement this is for the MiniGame in each mini game to take it in, then have the MiniGame class use these variables
					Then, all the subcomponents will look to the MiniGame class. 
	Dependencies:
	******************/
	
	public LevelType thisLevelType = LevelType.TimeCrunch;
	[Tooltip("this is the high level difficulty for this level.  It should determine what parameters the mini games belonging to this level will use.")] 
	public DifficultyDescription generalDifficulty;

	public bool usesTotalLevelTimerLimit;

	[Tooltip("for TimeCrunch, this will be between 2 an 4.  For Rotation, probably 3-5.  For MaintainTheGame/Sprint, 7+")]  
	public int totalGamesInLevel;

	public int gamesAtOneTime;

	[Tooltip("not all level types require all games in a level to be completed in order to beat the level.")]  
	public int minGamesNeededToWin;

	public int totalLevelTimeLimitSeconds;
	public int lagBetweenGamesSeconds;
	public bool gamesStopIfCompleted;
	public bool gamesStopIfFailed;


	[Tooltip("misc multipliers will undoubtedly be leveltype specific, so putting them here makes more sense, rather than in the DifficultyParams")]  
	public float miscMultiplier = 1;
	public float secondaryMiscMultiplier = 1;

	[Header("Time Crunch")]
	public float timeCrunchVar;

	[Header("Doubles")]
	public int timerForEachPairSeconds;
	public int pairsNeededToWin;

	[Header("Sprint")]
	public float sprintVar;

	[Header("MaintainTheGame")]
	public ProgressionParams secondaryGamesDiffucltyParams;

	[Header("Rotation")]
	public float totalSkips;
	public float availableSkips;
}
