using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoppable 
{
	/*****************
	CreateDate: 8/26
	Functionality: 	these methods will visually freeze and restart games. 
					some use cases for this will be mid-level story choices, or level types where games are swapped in repeatedly for one another,
						or where games are removed from play even if they aren't finished. 

					each implementation should set isActive = false or true
	Notes:	in the name of script independance, these methods should probably be implemented in their own script for each game. 
			something like "miniGameNameStopStarter"
	Dependencies:
	******************/
	
	public void StopMiniGame();
	public void RestartMiniGame();
}
