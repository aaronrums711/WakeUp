using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroyerStopStarter : MiniGameElement, IStoppable
{
	/*****************
	CreateDate: 	8/29/21
	Functionality:	implements the methods in IStoppable
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	/**
	StopMiniGame()
		set isActive = false
			this will nullify player input
		score stops adding
			this is by setting isActive = false


		freeze laser where it currrently is
		stop center rotator
		particle effect must be frozen

	**/
	

	public void StopMiniGame()
	{
		parentMiniGame.isActive = false;
	}

	public void RestartMiniGame()
	{
		return;
	}
}
