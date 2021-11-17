using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroyerStopStarter : MiniGameElement, IStoppable, ISlower
{
	/*****************
	CreateDate: 	8/29/21
	Functionality:	implements the methods in IStoppable
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public float SlowEffectEndRate;
	public float slowEffectChangeRate;
	private float InitPSVelocityOverLifeTimeSpeed;
	private float InitPSEmissionRateOverTime;
	private float InitRotatorBaseSpeed;
	private float InitRotatorBaseSpeedPlusRandom;

	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	private CenterTargetRotator rotator;
	private LaserDestroyerRingChanger ringChanger;
	private LaserDestroyerInputManager inputManager;
	private ParticleSystem ps;




	/**
	StopMiniGame()
		set isActive = false
			this will nullify player input
		score stops adding
			this is by setting isActive = false


		freeze laser where it currrently is
		stop center rotator
		particle effect must be frozen
		the ring changer must be stopped

	**/
	
	void Start()
	{
		rotator = parentMiniGame.GetComponentInChildren<CenterTargetRotator>();
		ringChanger = parentMiniGame.GetComponentInChildren<LaserDestroyerRingChanger>();
		inputManager = parentMiniGame.GetComponentInChildren<LaserDestroyerInputManager>();
		ps = parentMiniGame.GetComponentInChildren<ParticleSystem>();
		InitPSVelocityOverLifeTimeSpeed = ps.limitVelocityOverLifetime.limit.constant;
		InitPSEmissionRateOverTime = ps.emission.rateOverTime.constant;
		InitRotatorBaseSpeed = rotator.baseRotSpeed.z;
		InitRotatorBaseSpeedPlusRandom  = rotator.rotSpeedPlusRandom.z;

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(SlowDownMiniGame(SlowEffectEndRate, slowEffectChangeRate));
		}
	}

	[ContextMenu("StopMiniGame()")]
	public void StopMiniGame()
	{
		parentMiniGame.isActive = false;
		rotator.transform.rotation = Quaternion.Euler(0,0,rotator.targetRotation);  //we can't stop RotateChunk() in the middle of it's rotation, because it needs to be only on increments of 45 degrees
																					//so when this method is called, we are just setting the rotation to whatever it's final rotation was going to be for that chunk
		rotator.StopAllCoroutines();
		ringChanger.StopAllCoroutines();
		ringChanger.thisAnimator.enabled = false;

		inputManager.currentActiveEmitter.StopAllCoroutines();
		ps.Pause();

	}

	[ContextMenu("RestartMiniGame()")]
	public void RestartMiniGame()
	{
		parentMiniGame.isActive = true;
		StartCoroutine(rotator.ContinuallyRotate());
		ringChanger.thisAnimator.enabled = true;
		StartCoroutine(ringChanger.ContinuallyMovePanels());
		inputManager.currentActiveEmitter.CallInitialLaserCast();
	}

	public IEnumerator SlowDownMiniGame(float endRate, float changeRate)
	{
		print("slow method called");
		 var limitVelocityModule = ps.limitVelocityOverLifetime;
		 var emissionModule = ps.emission;
		float startTime = Time.time;
		float totalTime = 1f;
		float elapsed = 0f;
		while (elapsed<  totalTime)
		{
			float newValue1 = InitPSVelocityOverLifeTimeSpeed * changeRate;
			limitVelocityModule.limit = newValue1;

			float newValue2 = InitPSEmissionRateOverTime * changeRate;
			emissionModule.rateOverTime = newValue2;

			float newValue3 = InitRotatorBaseSpeed * changeRate;
			rotator.baseRotSpeed = new Vector3(0,0, newValue3);

			float newValue4 = InitRotatorBaseSpeedPlusRandom * changeRate;
			rotator.rotSpeedPlusRandom = new Vector3(0,0, newValue4);

			elapsed = Time.time-startTime;
			yield return null;

		}
		print("slow method ended");
	}

	public IEnumerator BringBackToSpeed(float changeRate)
	{
		yield return null;
	}
}
