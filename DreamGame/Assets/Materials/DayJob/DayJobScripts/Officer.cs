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

	//just a little convenience method.  moves this object, then returns true if the distance between start and end is small enough, else returns false
	public bool MoveToPoint(Vector3 startPos, Vector3 endPos, float speed, float distanceThreshold)
	{
		this.transform.position = Vector3.MoveTowards(startPos, endPos, speed * Time.deltaTime);
		if (Vector3.Distance(startPos, endPos) < distanceThreshold)
		{return true;}
		else {return false;}
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
			movementCoroutine = StartCoroutine(MoveThroughPoints(pointsAlongPath, base.movementSpeed, 0.1f));

			if (nextScheduleIndex == schedule.itemsInSchedule-1) //placing this here means that the unsubscribe will happen as soon as the unit starts moving to it's final location, NOT when it's gotten there.  Not sure if this would ever be a problem, but just heads up
			{
				Clock.halfHourTimer -= ClockEventListner;
			}
		}
	}

	public IEnumerator MoveThroughPoints(List<Vector3> pointsAlongPath, float speed, float distanceThreshold)
	{
		for (int i = 0; i <pointsAlongPath.Count; i++) 
		{
			while (MoveToPoint(this.transform.position, pointsAlongPath[i],  speed, distanceThreshold) == false)
			{
				yield return null;
			}
			//this doesn't seem to be needed now, but keeping it here for future
			// if (i == pointsAlongPath.Count-1)
			// {
			// 	print("final position set");
			// 	this.transform.position = pointsAlongPath[i];
			// }
		}
	}


}
