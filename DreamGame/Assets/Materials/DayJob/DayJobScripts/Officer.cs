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
	
	
	void Awake()
    {
    
    }
	
    void Start()
    {
        schedule.ConstructSchedule(schedule.times, schedule.destinations);
		// schedule.PrintFinalSchedule();
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
	public IEnumerator MoveThroughSchedule(Schedule _schedule, float speed, float distanceThreshold)
	{
		int startingScheduleIndex = GetNextOnSchedule(schedule);
		for (int i = startingScheduleIndex; i < schedule.itemsInSchedule; i++)
		{
			Vector3 waypoint = _schedule.schedule[i].Value.transform.position;
			DateTime time = _schedule.schedule[i].Key;
			while (moveToPoint(this.transform.position, waypoint,  speed, distanceThreshold) == false)
			{
				yield return null;
				if (checkTime(_schedule.schedule[i].Key))
				{
					break;
				}
			}
		}
	}

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

	public int GetNextOnSchedule(Schedule _schedule)
	{
		int scheduleNumber = 0;
		for (int i = 0; i < _schedule.itemsInSchedule; i++)
		{
			if (Clock.gameDateTime > _schedule.schedule[i].Key)  //if the gameDateTime > this item in the schedule, skip it, because it's already passed
			{
				continue;
			}
			else 
			{
				scheduleNumber = i;
				break;
			}
		}
		return scheduleNumber;
		
	}



}
