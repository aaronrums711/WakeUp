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
	public KnockEmDownWaveManager waveManager;
	public Transform targetParent;
	
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

	void Start()
	{
		waveManager = parentMiniGame.GetComponentInChildren<KnockEmDownWaveManager>();
		targetParent = GameObject.Find("KnockDownTargets").GetComponent<Transform>();
	}
	

	[ContextMenu("StopMiniGame() testing only")] 
	public void StopMiniGame()
	{
		parentMiniGame.isActive = false;
		waveManager.StopAllCoroutines();
		//setting isActive to false will stop all of the targets from shrinking, then the RestartMiniGame()
		//method will restart each one. 
	}

	[ContextMenu("RestartMiniGame() testing only")] 
	public void RestartMiniGame()
	{
		int targetsInPlay = targetParent.childCount;
		parentMiniGame.isActive = true;

		StartCoroutine(waveManager.SpawnWave(waveManager.minWaveAmount, waveManager.maxWaveAmount-targetsInPlay));

		for (int i = 0; i<targetParent.childCount; i++)
		{
			Transform targetTrans = targetParent.GetChild(i).GetComponent<Transform>();
			float shrinkRate = targetParent.GetChild(i).GetComponent<KnockEmDownTarget>().initialShrinkRate;
			StartCoroutine(targetParent.GetChild(i).GetComponent<KnockEmDownTarget>().Shrink(targetTrans,shrinkRate));
		}
	}
}
