using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DayJobLevelManager : MonoBehaviour
{
	/*****************
	CreateDate: 	2/9/22	
	Functionality:	manages the DayJob levels
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
	void Awake()
    {
		StartCoroutine(Clock.UpdateGameClockContinuously(new DateTime(2022,09,01,09,01,01), 400, 1, false, DateTime.MaxValue));
		StartCoroutine(DisplayGameTime());
		Clock.halfHourTimer += PrintEventDate;
    }
	
    void Start()
    {
        
    }

    void Update()
    {

    }

	private IEnumerator DisplayGameTime()
	{
		while (true)
		{
			print("current game time: " + Clock.gameDateTime);
			// print("currentHour: " + Clock.gameHour);
			// print("currentMinute: " + Clock.gameMinuteWithinHour);
			yield return new WaitForSeconds(Clock.realLifeUpdateInterval);
		}
	}

	private void PrintEventDate(DateTime dt)
	{
		print("event called for: " + dt);
	}

}
