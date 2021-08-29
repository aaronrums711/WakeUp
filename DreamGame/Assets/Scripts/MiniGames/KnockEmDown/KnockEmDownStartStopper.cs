using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockEmDownStartStopper : MiniGameElement, IStoppable 
{
	/*****************
	CreateDate: 	8/29/21
	Functionality:	implements the two methods in IStoppable
	Notes:	
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	/**
	StopMiniGame()
		set isActive = false;
		freeze targets on screen
		stop more targets from getting spawned


	RestartMiniGame()
		set isActive = true
		current targets resume shrinking
		targets spawn again as usual
	**/
	


	public void StopMiniGame()
	{
		return;
	}

	
	public void RestartMiniGame()
	{
		return;
	}
}
