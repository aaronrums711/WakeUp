using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppedStopStarter : MiniGameElement, IStoppable
{
	/*****************
	CreateDate: 8/26/21 
	Functionality: 
	Notes: 		contains the implementations for the methods in IStoppable
	Dependencies:
	******************/
	
	//TO DO:
	/**
	for Stop()
		get all targets on scene and turn their velocity to 0
			but save their velocity to start it again later

		set isActive = false
			this will nullify player input

		StopAllCoroutines on the spawner script

	for Restart()
		get all targets and turn their velocity to what it was before
		set isActive = true
		call StartCoroutine(ChoppedTargetSpawner.GenerateWave()) 
	**/

	//////////////////////////////Config
	private float gravityScaleAtStop;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	private ChoppedTargetSpawner spawner;
	
	void Start()
	{
		spawner = parentMiniGame.GetComponentInChildren<ChoppedTargetSpawner>();
	}

	[ContextMenu("StopMiniGame()")]
	public void StopMiniGame()
	{

		parentMiniGame.isActive = false;
		foreach (ChoppedTarget target in spawner.allTargets )
		{
			Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
			target.velocityAtStop = rb.velocity;
			rb.velocity = Vector2.zero;
			gravityScaleAtStop = rb.gravityScale;  //we're setting ths variable 4 times, even though it'll be the same very time
			rb.gravityScale = 0;
		}

		spawner.StopAllCoroutines();
	}

	[ContextMenu("RestartMiniGame()")]
	public void RestartMiniGame()
	{
		parentMiniGame.isActive = true;
		foreach (ChoppedTarget target in spawner.allTargets )
		{
			Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
			rb.velocity = target.velocityAtStop;
			rb.gravityScale = gravityScaleAtStop;
		}
		StartCoroutine(spawner.GenerateWave());
	}
}
