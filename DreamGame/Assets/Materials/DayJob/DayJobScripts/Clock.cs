using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Clock 
{
	/*****************
	CreateDate: 	2/8/22
	Functionality:	this class functions as a real life clock.  It will be used primarily in a 9-5 work day context, but it should be versatile enough to 
					be used in other scenarios where it's necessary to model time passing in a semi-realistic way

	Notes:			this class is self updating, but it NEEDS to be kicked off by another class. 
	Dependencies:	
	******************/
	
	
	//////////////////////////////State
	[Range(1f,30f), Tooltip("this determines how fast the in-game time will pass")]
	public static int realLifeUpdateInterval;  	//how frequently, in real life seconds, will the game clock be updated
	public static float timeSpeed;				
	private static DateTime initializedAtRealTime;
	private static DateTime lastUpdateRealTime;

	public static DateTime gameDateTime;
	public static int gameYear;
	public static int gameMonth;
	public static int gameDay;
	public static int gameHour;
	public static int gameMinuteWithinHour;
	public static bool gameClockOn = false;
	


	//this can be used like a constructor; 
	public static void InitializeGameClock(DateTime gameStartTime, float _timeSpeed, int updateInterval)
	{
		timeSpeed = _timeSpeed;
		initializedAtRealTime = DateTime.Now;
		lastUpdateRealTime = DateTime.Now;
		realLifeUpdateInterval = updateInterval;

		gameDateTime = gameStartTime;
		gameClockOn = true;
	}


	private static void UpdateValues(DateTime dt)
	{
		gameYear = dt.Year;
		gameMonth = dt.Month;
		gameDay = dt.Day;
		gameHour = dt.Hour;
		gameMinuteWithinHour = dt.Minute;
	}

	private static DateTime UpdateGameClock()
	{
		TimeSpan ts = DateTime.Now - lastUpdateRealTime;


		//UPON RETURN; there is something wrong with these two lines. without them, it's properly returning the elapsed time of the gameStartTime.  But with them, the time is not moving
		int newSeconds = (int)(ts.Seconds * timeSpeed);
		ts = new TimeSpan(0,0,newSeconds);  //this constr is hours, minutes seconds, 

		gameDateTime += ts;
		lastUpdateRealTime = DateTime.Now;
		UpdateValues(gameDateTime);
		return gameDateTime;
	}


	public  static IEnumerator  UpdateGameClockContinuously()
	{
		while (gameClockOn)
		{
			UpdateGameClock();
			yield return realLifeUpdateInterval; 
		}
		
	}

	public static bool TurnOffClock()
	{
		gameClockOn = false;
		return gameClockOn;
	}

}
