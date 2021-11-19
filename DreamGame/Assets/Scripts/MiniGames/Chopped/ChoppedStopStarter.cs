using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppedStopStarter : MiniGameElement, IStoppable, ISlower
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

	public float slowDownEffectChangeRate;
	public float speedUpEffectChangeRate;	
	public float speedChangeDuration;

	private float rbVelocityForTest;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	private ChoppedTargetSpawner spawner;

	


	void Start()
	{
		speedUpEffectChangeRate = 1 + (1-slowDownEffectChangeRate);
		spawner = parentMiniGame.GetComponentInChildren<ChoppedTargetSpawner>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(SlowDownMiniGame(speedChangeDuration, slowDownEffectChangeRate));
		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
			StartCoroutine(BringBackToSpeed(speedChangeDuration, speedUpEffectChangeRate ));
		}
	}

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


	public IEnumerator SlowDownMiniGame(float duration, float changeRate)
	{
		spawner.StopAllCoroutines();
		if (spawner.allTargets.Count > 0)
		{
			for (int i = 0; i < spawner.allTargets.Count; i++)
			{
				spawner.allTargets[i].rb.gravityScale = 0.1f;
				spawner.allTargets[i].rb.mass = 0.5f;
				this.gravityScaleAtStop = spawner.allTargets[i].rb.gravityScale; 
			}
		}
		yield return new WaitForFixedUpdate(); //juuust to be safe, yielding one physics update

		float startTime = Time.time;
		float totalTimeSeconds = duration;
		float elapsed = 0;

		if (spawner.allTargets.Count > 0)
		{

			while (elapsed < totalTimeSeconds  /***spawner.allTargets[0].rb.velocity.sqrMagnitude > rbVelocityForTest*/)
				{
					for (int i = 0; i < spawner.allTargets.Count; i++)
					{
						Vector2 RBVelocity = spawner.allTargets[i].rb.velocity;
						RBVelocity *= changeRate;
						spawner.allTargets[i].rb.velocity = RBVelocity;
						yield return new WaitForFixedUpdate();
					}
					elapsed = Time.time - startTime;
					if (spawner.allTargets.Count == 0)
					{
						print("slow down loop broken because all targets destroyed!");
						break;
					}
				}
			print("slow down complete!");
			
		}
	}

	public IEnumerator BringBackToSpeed(float duration, float changeRate)
	{
		StopCoroutine("SlowDownMiniGame");
		if (spawner.allTargets.Count > 0)
		{
			float startTime = Time.time;
			float totalTimeSeconds = duration;
			float elapsed = 0;
			while (elapsed < totalTimeSeconds   /**spawner.allTargets[0].rb.velocity.sqrMagnitude < spawner.allTargets[0].magnitudeAtSpeedChange**/)
			{
				for (int i = 0; i < spawner.allTargets.Count; i++)
				{
					Vector2 RBVelocity = spawner.allTargets[i].rb.velocity;
					RBVelocity *= changeRate;
					spawner.allTargets[i].rb.velocity = RBVelocity;
					yield return new WaitForFixedUpdate();
				}
				elapsed = Time.time - startTime;
				if (spawner.allTargets.Count == 0)
				{
					print("speed up loop broken because all targets destroyed!");
					break;
				}
			}

			for (int i = 0; i < spawner.allTargets.Count; i++)  //set them back exactly to what they were, just in case the above method doesn't do it exactly, since it's based on time elapsed
			{
				spawner.allTargets[i].rb.gravityScale = 1f;
				spawner.allTargets[i].rb.mass =  1f;
			}
		}
		print("speed up method complete!");

		StartCoroutine(spawner.GenerateWave());
	}

}
