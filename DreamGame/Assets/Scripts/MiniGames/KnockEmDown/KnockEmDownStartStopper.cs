using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockEmDownStartStopper : MiniGameElement, IStoppable , ISlower
{
	/*****************
	CreateDate: 	8/29/21
	Functionality:	implements the two methods in IStoppable
	Notes:	
	Dependencies:
	******************/
	
	//////////////////////////////Config
	private float slowEffectEndRate = 0.2f;
	private float slowEffectChangeRate = 1f;  //for this implementation of the ISlower methods, we ARE NOT USING THIS.  there's no need to slowly lerp anything for this mini game 
	
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

	SlowDownMiniGame()
		stop all coroutines.  no further targets should be spawned during this effect
		set all shrink rates to X percent of their normal
		stop the shrink coroutine on each target, and re-call it, setting the shrink rate to the new one

	BringBackToSpeed()
		set all shrink rates back to normal
		spawn another wave, taking into account how many are already on the screen
	
	**/

	void Start()
	{
		waveManager = parentMiniGame.GetComponentInChildren<KnockEmDownWaveManager>();
		targetParent = GameObject.Find("KnockDownTargets").GetComponent<Transform>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(SlowDownMiniGame(slowEffectEndRate, slowEffectChangeRate));
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			StartCoroutine(BringBackToSpeed(0f));
		}
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

		StartCoroutine(waveManager.SpawnWave(waveManager.minWaveAmount, waveManager.maxWaveAmount-targetsInPlay, waveManager.minTimeBetweenEachWave, waveManager.maxTimeBetweenEachWave));

		for (int i = 0; i<targetParent.childCount; i++)
		{
			Transform targetTrans = targetParent.GetChild(i).GetComponent<Transform>();
			float shrinkRate = targetParent.GetChild(i).GetComponent<KnockEmDownTarget>().initialShrinkRate;
			targetParent.GetChild(i).GetComponent<KnockEmDownTarget>().RestartInitialCoroutine(targetParent.GetChild(i).GetComponent<Transform>(), shrinkRate);
		}
	}


	public IEnumerator SlowDownMiniGame(float endRate, float changeRate)
	{
		print("slow down method called");
		waveManager.StopAllCoroutines();
		if (targetParent.childCount > 0)
		{
			float newShrinkRate = targetParent.GetChild(0).GetComponent<KnockEmDownTarget>().initialShrinkRate * endRate;

			for (int i = 0; i<targetParent.childCount; i++)
			{
				// StopCoroutine(targetParent.GetChild(i).GetComponent<KnockEmDownTarget>().initialCoroutine);
				targetParent.GetChild(i).GetComponent<KnockEmDownTarget>().StopInitialCoroutine();
				Transform targetTrans = targetParent.GetChild(i).GetComponent<Transform>();
				StartCoroutine(targetParent.GetChild(i).GetComponent<KnockEmDownTarget>().Shrink(targetTrans,newShrinkRate));
				print("iteration num : " + i);
			}
		}
		yield return null;
	}

	public IEnumerator BringBackToSpeed(float changeRate) //this implementation doesn't utilize this change, because we're not slowly lerping stuff like we are in the others. 
	{
		int targetsInPlay = targetParent.childCount;
		StartCoroutine(waveManager.SpawnWave(waveManager.minWaveAmount, waveManager.maxWaveAmount-targetsInPlay, waveManager.minTimeBetweenEachWave, waveManager.maxTimeBetweenEachWave));
		
		if (targetParent.childCount > 0)
		{
			float shrinkRate = targetParent.GetChild(0).GetComponent<KnockEmDownTarget>().initialShrinkRate;
			for (int i = 0; i<targetParent.childCount; i++)
			{
				targetParent.GetChild(i).GetComponent<KnockEmDownTarget>().StopInitialCoroutine();
				yield return null; //just to be safe.  Don't know if stopping and starting coroutines right after eachother on the same object will cause weirdness
				targetParent.GetChild(i).GetComponent<KnockEmDownTarget>().RestartInitialCoroutine(targetParent.GetChild(i).GetComponent<Transform>(), shrinkRate);
			}
		}

		yield return null;
	}
}
