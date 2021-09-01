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
	private CenterTargetRotator rotator;
	
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
	
	void Start()
	{
		rotator = parentMiniGame.GetComponentInChildren<CenterTargetRotator>();
	}

	[ContextMenu("StopMiniGame()")]
	public void StopMiniGame()
	{
		parentMiniGame.isActive = false;
		rotator.transform.rotation = Quaternion.Euler(0,0,rotator.targetRotation);  //we can't stop RotateChunk() in the middle of it's rotation, because it needs to be only on increments of 45 degrees
																					//so when this method is called, we are just setting the rotation to whatever it's final rotation was going to be for that chunk
		rotator.StopAllCoroutines();
	}

	[ContextMenu("RestartMiniGame()")]
	public void RestartMiniGame()
	{
		parentMiniGame.isActive = true;
		StartCoroutine(rotator.ContinuallyRotate());
	}
}
