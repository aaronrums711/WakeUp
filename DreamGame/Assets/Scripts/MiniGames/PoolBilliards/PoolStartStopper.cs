using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolStartStopper : MiniGameElement, IStoppable, ISlower
{
	/*****************
	CreateDate: 	9/3/21
	Functionality:	implements the methods in IStoppable and ISlower methods (ISlower addded 11/13/21)
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	private float poolStickInitialRotateSpeed;
	Vector2 cueBallInitialVelocity;
	float  cueBallInitialMagnitude;

	[SerializeField, Range(0.05f, 0.9f), Tooltip("lower value = more drastic slow down")]
	float slowDownEffectEndRate;

	[SerializeField, Range(0.5f, 0.99f), Tooltip("lower value = faster rate of slow down")]
	float slowDownEffectChangeRate;
	private float speedUpChangeRate;

	private float cueBallInitialLinearDrag;

	
	//////////////////////////////Cached Component References
	private PoolTargetSpawner spawner;
	[SerializeField] private CueBall cueBall;
	[SerializeField] private PoolStick poolStick;

	
    void Start()
    {
        spawner = parentMiniGame.GetComponentInChildren<PoolTargetSpawner>();
		cueBall =  parentMiniGame.GetComponentInChildren<CueBall>();
		poolStick =  parentMiniGame.GetComponentInChildren<PoolStick>();
		poolStickInitialRotateSpeed = poolStick.rotationSpeed;
		cueBallInitialLinearDrag = cueBall.thisRB.drag;
		speedUpChangeRate = 1 + (1 - slowDownEffectChangeRate); //this will ensure that it will return to speed at the same rate as it slowed

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(SlowDownMiniGame(slowDownEffectEndRate, slowDownEffectChangeRate));
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			StartCoroutine(BringBackToSpeed( 1f, speedUpChangeRate));
		}
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

	//goal here is to quickly but smoothly change the speed of a few items.  End Rate should be between 0 and 1.  Close to 0 will mean objects will come to an almost complete stop
	//close to 1 will mean they will only slow down a little bit.  Think of endRate as a normalized value that represents the what the slowed speed will be as a percent of the initial speed
	//changeRate will make the slow down happen faster.  A lower change rate will equal a faster change
	
	public IEnumerator SlowDownMiniGame(float endRate, float changeRate)
	{	
		poolStick.isSlowed = true;
		float poolStickSlowedRotateSpeed = poolStickInitialRotateSpeed * endRate;
		float poolStickRotateSpeed = poolStickInitialRotateSpeed;
		cueBallInitialVelocity = cueBall.thisRB.velocity;
		cueBallInitialMagnitude = cueBall.thisRB.velocity.sqrMagnitude;

		Vector2 cueBallVelocity = cueBallInitialVelocity;

		spawner.CancelInvoke("AttemptToSpawnTargets");
		while (poolStickRotateSpeed >= poolStickSlowedRotateSpeed)
		{
			poolStickRotateSpeed *= changeRate;
			poolStick.rotationSpeed = poolStickRotateSpeed;

			cueBallVelocity = cueBall.thisRB.velocity;
			cueBallVelocity *= changeRate;
			cueBall.thisRB.velocity = cueBallVelocity;

			yield return new WaitForFixedUpdate();
		}
		cueBall.thisRB.drag = 0; //to prevent it from slowing down more during this slow down effect

	}



	public IEnumerator BringBackToSpeed( float duration, float changeRate)
	{
		// float changeRate = 1 + (1 - slowDownEffectChangeRate); //this will ensure that it will return to speed at the same rate as it slowed
		float poolStickRotateSpeed = poolStick.rotationSpeed;
		float sqrMagnitude = 0;
		float targetSqrMagnitude = cueBallInitialMagnitude * 0.85f;  //0.85 is arbitrary...but it doesn't make sense for the cue ball to be set back to it's full previous speed, because drag would still effect it
		Vector2 cueBallVelocity;
		while (poolStickRotateSpeed < poolStickInitialRotateSpeed || sqrMagnitude < cueBallInitialMagnitude)
		{
			cueBall.thisRB.drag = cueBallInitialLinearDrag;
			poolStickRotateSpeed *= changeRate;
			poolStick.rotationSpeed = poolStickRotateSpeed;

			cueBallVelocity = cueBall.thisRB.velocity;
			cueBallVelocity *= changeRate;
			sqrMagnitude = cueBallVelocity.sqrMagnitude;
			cueBall.thisRB.velocity = cueBallVelocity;
			
			yield return new WaitForFixedUpdate();
		}
		poolStick.rotationSpeed = poolStickInitialRotateSpeed;
		
		poolStick.isSlowed = false;
		spawner.InvokeRepeating("AttemptToSpawnTargets", 5f, 10f);

		
	}

}
