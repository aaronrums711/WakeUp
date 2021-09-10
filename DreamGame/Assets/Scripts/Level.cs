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
	public DifficultyParams difficultyParams;

	[Tooltip("for TimeCrunch, this will be between 2 an 4.  For Rotation, probably 4-8.  For MaintainTheGame/Sprint, 7+")]  
	public int numGames;

	[Tooltip("should be slightly less than 1 to make games easier, slighly more than 1 to make them harder")]  
	public int UniversalDragMultiplier;

	public int totalLevelTimeLimitSeconds;
	
	public int lagBetweenGamesSeconds;

	


}
