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

	public float slowDownChangeRate;
	public  float speedUpChangeRate;
	public float speedChangeDuration;

	[Range(0.01f,0.1f)]
	public float speedUpChangeRateAddition;  //for Pool, I think we need to add a bit more speed than we took away, to compensate. That's what this does.  It should be very small, between 0.01 and 0.1 maybe 

	[Range(1f,20f)]
	public float finalCueBallPush;

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
		speedUpChangeRate = 1 + (1 - slowDownChangeRate) + speedUpChangeRateAddition; //this will ensure that it will return to speed at the same rate as it slowed

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(SlowDownMiniGame(speedChangeDuration, slowDownChangeRate));
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			StartCoroutine(BringBackToSpeed( speedChangeDuration, speedUpChangeRate));
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

	
	public IEnumerator SlowDownMiniGame(float duration, float changeRate)
	{	
		poolStick.isSlowed = true;
		float poolStickRotateSpeed = poolStickInitialRotateSpeed;
		cueBallInitialVelocity = cueBall.thisRB.velocity;
		cueBallInitialMagnitude = cueBall.thisRB.velocity.sqrMagnitude;

		Vector2 cueBallVelocity = cueBallInitialVelocity;
		float startTime = Time.time;
		float totalTime = duration;
		float elapsed = 0f;
		spawner.CancelInvoke("AttemptToSpawnTargets");
		while (elapsed<  totalTime)
		{
			poolStickRotateSpeed *= changeRate;
			poolStick.rotationSpeed = poolStickRotateSpeed;

			cueBallVelocity = cueBall.thisRB.velocity;
			cueBallVelocity *= changeRate;
			cueBall.thisRB.velocity = cueBallVelocity;
			elapsed = Time.time-startTime;

			yield return new WaitForFixedUpdate();
		}
		cueBall.thisRB.drag = 0; //to prevent it from slowing down more during this slow down effect

	}



	public IEnumerator BringBackToSpeed( float duration, float changeRate)
	{
		// float changeRate = 1 + (1 - slowDownEffectChangeRate); //this will ensure that it will return to speed at the same rate as it slowed
		float poolStickRotateSpeed = poolStick.rotationSpeed;
		Vector2 cueBallVelocity;

		float startTime = Time.time;
		float totalTime = duration;
		float elapsed = 0f;

		while (elapsed<  totalTime)
		{
			cueBall.thisRB.drag = cueBallInitialLinearDrag;
			poolStickRotateSpeed *= changeRate;
			poolStick.rotationSpeed = poolStickRotateSpeed;

			cueBallVelocity = cueBall.thisRB.velocity;
			cueBallVelocity *= changeRate;
			cueBall.thisRB.velocity = cueBallVelocity;
			elapsed = Time.time-startTime;
			yield return new WaitForFixedUpdate();
		}
		poolStick.rotationSpeed = poolStickInitialRotateSpeed;
		cueBallVelocity = cueBall.thisRB.velocity;
		cueBallVelocity *= finalCueBallPush;  //this gives it a final push to compensate for the lost ground of slow effect.  the finalCueBallPush is a somewhat arbitrary ball-park figure. 
		cueBall.thisRB.velocity = cueBallVelocity;
		
		poolStick.isSlowed = false;
		spawner.InvokeRepeating("AttemptToSpawnTargets", 5f, 10f);

		
	}

}
