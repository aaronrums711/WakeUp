using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Officer : NPC
{
	/*****************
	CreateDate: 	2/12/22
	Functionality:	the class for all officers in a DayJob level
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public Schedule schedule;
	public MovementState state = MovementState.notStartedPath;


	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
    void Start()
    {
		pathfinding = GameObject.Find("Astar").GetComponent<Pathfinding_Heap>();
        schedule.ConstructSchedule(schedule.times, schedule.destinations);
		Clock.halfHourTimer += ClockEventListner;

    }

    void Update()
    {
        
    }

	/**
	need to check the time, then execute moveToPoint, but also need to check if the time for the next point has occurred.

	Also, design decision.  If the time for next schedule item comes before the previous waypoint is reached, should the unit stop and switch waypoints, or 
	finish the current path?  

	For now, they are stopping.  
	**/
	// public IEnumerator MoveThroughSchedule(Schedule _schedule, float speed, float distanceThreshold)
	// {
	// 	state = MovementState.followingPath;
	// 	int startingScheduleIndex = GetNextOnSchedule(schedule);

	// 	for (int i = startingScheduleIndex; i < schedule.itemsInSchedule; i++)
	// 	{
	// 		List<Vector3> pointsAlongPath = pathfinding.FindPath(this.transform.position, _schedule.schedule[i].Value.transform.position);
	// 		DateTime time = _schedule.schedule[i].Key;

	// 		for (int i2 = 0; i2 < pointsAlongPath.Count; i2++)
	// 		{
	// 			while (moveToPoint(this.transform.position, pointsAlongPath[i2],  speed, distanceThreshold) == false)
	// 			{
	// 				yield return null;
	// 			}
	// 			if (checkTime(_schedule.schedule[i].Key))
	// 			{
	// 				break;
	// 			}
	// 		}
	// 	}
	// }

	//just a little convenience method.  moves this object, then returns true if the distance between start and end is small enough, else returns false
	public bool moveToPoint(Vector3 startPos, Vector3 endPos, float speed, float distanceThreshold)
	{
		this.transform.position = Vector3.MoveTowards(startPos, endPos, speed * Time.deltaTime);
		if (Vector3.Distance(startPos, endPos) < distanceThreshold)
		{return true;}
		else {return false;}
	}

	public bool checkTime(DateTime timeToCheck)
	{
		if ((Clock.gameDateTime - timeToCheck).Seconds > Clock.timeSpeed*2)  //the multiplier just gives an extra window for this to return true
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	//you will often just pass in the Clock.gameDateTime here, but you may also pass in a future date if you want to find out what the next item on _schedule is at that time
	public int GetNextOnSchedule(Schedule _schedule, DateTime timeToCompare)
	{
		int nextScheduleIndex = 0;
		for (int i = 0; i < _schedule.itemsInSchedule; i++)
		{
			if (timeToCompare > _schedule.schedule[i].Key)  //if the gameDateTime > this item in the schedule, skip it, because it's already passed
			{
				continue;
			}
			else 
			{
				nextScheduleIndex = i;
				break;
			}
		}
		if (nextScheduleIndex == _schedule.itemsInSchedule)
		{
			Clock.halfHourTimer -= ClockEventListner;
			print("schedule finished, event unsubscribed");		
		}
		return nextScheduleIndex;

		
	}


	public void ClockEventListner(DateTime inGameDT)
	{
		int currentHour = inGameDT.Hour;
		int currentMinute = inGameDT.Minute;
		int nextScheduleIndex = GetNextOnSchedule(schedule, inGameDT);

		int scheduleHour = schedule.schedule[nextScheduleIndex].Key.Hour;
		int scheduleMinute = schedule.schedule[nextScheduleIndex].Key.Minute;

		if (currentHour == scheduleHour && currentMinute == scheduleMinute)
		{
			if (movementCoroutine != null)  {StopCoroutine(movementCoroutine);}

			List<Vector3> pointsAlongPath = pathfinding.FindPath(this.transform.position, schedule.schedule[nextScheduleIndex].Value.transform.position);
			movementCoroutine = StartCoroutine(moveToPoint2(pointsAlongPath, base.movementSpeed, 0.25f));
		}
	}

	//loop through each vector3, move towards it until distance < distanceThreshold, then move to next item in list
	public IEnumerator moveToPoint2(List<Vector3> pointsAlongPath, float speed, float distanceThreshold)
	{
		for (int i = 0; i <pointsAlongPath.Count; i++) 
		{
			while (moveToPoint(this.transform.position, pointsAlongPath[i],  speed, distanceThreshold) == false)
			{
				yield return null;
			}
		}
	}


}
