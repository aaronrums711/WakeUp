using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolStartStopper : MiniGameElement, IStoppable
{
	/*****************
	CreateDate: 	9/3/21
	Functionality:	implements the methods in IStoppable
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	private PoolTargetSpawner spawner;
	
	
    void Start()
    {
        spawner = parentMiniGame.GetComponentInChildren<PoolTargetSpawner>();
    }

    void Update()
    {
        
    }

	[ContextMenu("StopMiniGame()")]
	public void StopMiniGame()
	{
		parentMiniGame.isActive = false;
		spawner.CancelInvoke("AttemptToSpawnTargets");
	}

	[ContextMenu("RestartMiniGame()")]
	public void RestartMiniGame()
	{
		parentMiniGame.isActive = true;;
		spawner.InvokeRepeating("AttemptToSpawnTargets", 5f, 10f);
	}
}
