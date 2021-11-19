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
	public float SlowEffectDuration;
	public float slowEffectChangeRate;
	private float InitPSVelocityOverLifeTimeSpeed;
	private float InitPSEmissionRateOverTime;
	private float InitRotatorBaseSpeed;
	private float InitRotatorBaseSpeedPlusRandom;
	private float NewPSVelocityOverLifeTimeSpeed;
	private float NewPSEmissionRateOverTime;
	private float NewRotatorBaseSpeed;
	private float NewRotatorBaseSpeedPlusRandom;
	private float initLaserInitializationMultipler;
	private float initLaserReductionMultiplier;
	private float initBaseProgression;

	public float speedUpChangeRate;

	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	private CenterTargetRotator rotator;
	private LaserDestroyerRingChanger ringChanger;
	private LaserDestroyerInputManager inputManager;
	private ParticleSystem ps;
	private List<LaserEmitter> allLaserEmitters;




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
		initBaseProgression = parentMiniGame.baseProgression;

		NewPSVelocityOverLifeTimeSpeed =InitPSVelocityOverLifeTimeSpeed;
		NewPSEmissionRateOverTime = InitPSEmissionRateOverTime;
		NewRotatorBaseSpeed = InitRotatorBaseSpeed;
		NewRotatorBaseSpeedPlusRandom = InitRotatorBaseSpeedPlusRandom;

		speedUpChangeRate = 1 + (1-slowEffectChangeRate);





	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(SlowDownMiniGame(SlowEffectDuration, slowEffectChangeRate));
		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
			StartCoroutine(BringBackToSpeed(SlowEffectDuration, speedUpChangeRate ));
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

	public IEnumerator SlowDownMiniGame(float duration, float changeRate)
	{
		print("slow method called");
		initLaserInitializationMultipler = inputManager.allLaserEmitters[0].laserInitializationMultiplier;
		initLaserReductionMultiplier = inputManager.allLaserEmitters[0].laserReductionMultiplier;

		int itCount = 0;
		 var limitVelocityModule = ps.limitVelocityOverLifetime;
		 var emissionModule = ps.emission;
		float startTime = Time.time;
		float totalTime = duration;
		float elapsed = 0f;
		while (elapsed<  totalTime)
		{
			NewPSVelocityOverLifeTimeSpeed *= changeRate;
			limitVelocityModule.limit = NewPSVelocityOverLifeTimeSpeed;

			NewPSEmissionRateOverTime *= changeRate;
			emissionModule.rateOverTime = NewPSEmissionRateOverTime;

			NewRotatorBaseSpeed *= changeRate;
			rotator.baseRotSpeed = new Vector3(0,0, NewRotatorBaseSpeed);

			NewRotatorBaseSpeedPlusRandom *= changeRate;
			rotator.rotSpeedPlusRandom = new Vector3(0,0, NewRotatorBaseSpeedPlusRandom);

			foreach (LaserEmitter emitter in inputManager.allLaserEmitters)
			{
				emitter.laserInitializationMultiplier *= changeRate;
				emitter.laserReductionMultiplier *= changeRate;
			}

			parentMiniGame.baseProgression *=changeRate;

			elapsed = Time.time-startTime;
			print("iteration count : " + itCount);
			itCount ++;
			yield return null;

		}
		print("emission module end constant: " + emissionModule.rateOverTime.constant);
		print("slow method ended");
	}

	public IEnumerator BringBackToSpeed(float duration, float changeRate)
	{

		var limitVelocityModule = ps.limitVelocityOverLifetime;
		var emissionModule = ps.emission;
		float startTime = Time.time;
		float totalTime = duration;
		float elapsed = 0f;
		while (elapsed<  totalTime)
		{
			NewPSVelocityOverLifeTimeSpeed *= changeRate;
			limitVelocityModule.limit = NewPSVelocityOverLifeTimeSpeed;

			NewPSEmissionRateOverTime *= changeRate;
			emissionModule.rateOverTime = NewPSEmissionRateOverTime;

			NewRotatorBaseSpeed *= changeRate;
			rotator.baseRotSpeed = new Vector3(0,0, NewRotatorBaseSpeed);

			NewRotatorBaseSpeedPlusRandom *= changeRate;
			rotator.rotSpeedPlusRandom = new Vector3(0,0, NewRotatorBaseSpeedPlusRandom);

			foreach (LaserEmitter emitter in inputManager.allLaserEmitters)
			{
				emitter.laserInitializationMultiplier *= changeRate;
				emitter.laserReductionMultiplier *= changeRate;
			}
			parentMiniGame.baseProgression *=changeRate;

			elapsed = Time.time-startTime;
			yield return null;
		}
		parentMiniGame.baseProgression = initBaseProgression;
	}
}
