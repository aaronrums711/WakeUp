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
	[SerializeField, Range(0.05f, 0.9f), Tooltip("lower value = more drastic slow down")]
	float slowDownEffectEndRate;

	[SerializeField, Range(0.5f, 0.99f), Tooltip("lower value = faster rate of slow down")]
	float slowDownEffectChangeRate;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	private ChoppedTargetSpawner spawner;
	


	void Start()
	{
		spawner = parentMiniGame.GetComponentInChildren<ChoppedTargetSpawner>();
	}

	void Update()
	{
        if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(SlowDownMiniGame(slowDownEffectEndRate, slowDownEffectChangeRate));
		}
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


	public IEnumerator SlowDownMiniGame(float endRate, float changeRate)
	{
		spawner.StopAllCoroutines();
		yield return new WaitForFixedUpdate(); //juuust to be safe, yielding one physics update
		List <Rigidbody2D> targetRBs =  new List <Rigidbody2D>();

		foreach (ChoppedTarget target in spawner.allTargets )
		{
			Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
			targetRBs.Add(rb);
			rb.gravityScale = 0f;
			rb.mass = 0.2f;
		}
		float rbVelocityForTest = targetRBs[0].velocity.sqrMagnitude * endRate;  	//we only need to use one list element for evaluating in the loop, because they are all being changed at the same rate
																					//so the [0] here is arbitrary
		print("target slowed magnitude =  " + rbVelocityForTest);
		float totalTime = 1f;
		float startTime = Time.time;
		float elapsed = 0f;
		while (elapsed < totalTime/**targetRBs[0].velocity.sqrMagnitude > rbVelocityForTest**/)
		{

			// foreach (ChoppedTarget target in spawner.allTargets ) //need to recalculate this list every loop, because a target could get destroyed in between
			// {
			// 	Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
			// 	targetRBs.Add(rb);
			// }
			for (int i = 0; i < spawner.allTargets.Count; i++)
			{
				Vector2 RBVelocity = spawner.allTargets[i].rb.velocity;
				RBVelocity *= changeRate;
				spawner.allTargets[i].rb.velocity = RBVelocity;
				elapsed = Time.time - startTime;
				yield return new WaitForFixedUpdate();
				// print("magnitude in loop : " + targetRBs[0].velocity.sqrMagnitude);
			}
		
		}
		print("slow down complete!");
	}

	public IEnumerator BringBackToSpeed(float changeRate)
	{
		yield return new WaitForFixedUpdate();;
	}

}
