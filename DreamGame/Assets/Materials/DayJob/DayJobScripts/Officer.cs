using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public enum movementState
	{
		followingPath
	};


	
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



}
