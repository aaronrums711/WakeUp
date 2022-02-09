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
	
	public static int realLifeUpdateInterval;  	//how frequently, in real life seconds, will the game clock be updated

	[Range(1f,30f)]		
	public static int timeSpeed;				//how fast relative to real-time will the game-clock advance				
	private static DateTime initializedAtRealTime;
	public static DateTime gameDateTime;
	public static DateTime gameAutoKillTime;
	public static DateTime shutOffTime;

	public static int gameYear;
	public static int gameMonth;
	public static int gameDay;
	public static int gameHour;
	public static int gameMinuteWithinHour;
	public static bool gameClockOn = false;
	public static bool autoKillAtEndTime = false;

	public static TimeSpan RealTimeSinceStart
	{
		get{return DateTime.Now - initializedAtRealTime;}
	}

	


	//this can be used like a constructor; 
	public static void InitializeGameClock(DateTime _gameDateTime, int _timeSpeed, int _realLifeUpdateInterval)
	{
		timeSpeed = _timeSpeed;
		initializedAtRealTime = DateTime.Now;
		realLifeUpdateInterval = _realLifeUpdateInterval;

		gameDateTime = _gameDateTime;
		gameClockOn = true;
	}

		public static void InitializeGameClock(DateTime gameStartTime, int _timeSpeed, int _realLifeUpdateInterval, DateTime _endDateTime)
	{
		timeSpeed = _timeSpeed;
		initializedAtRealTime = DateTime.Now;
		realLifeUpdateInterval = _realLifeUpdateInterval;

		gameDateTime = gameStartTime;
		gameClockOn = true;
		gameAutoKillTime = _endDateTime;
		autoKillAtEndTime = true;
		
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
		int secondsElapsed = (int)(realLifeUpdateInterval * timeSpeed);  //becasue the UpdateGameClockContinuously() is using realLifeUpdateInterval as its yield, we know that thats how many seconds have elapsed
		TimeSpan ts = new TimeSpan(0,0,secondsElapsed);  //this constr is hours, minutes seconds, 

		gameDateTime += ts;
		UpdateValues(gameDateTime);
		return gameDateTime;
	}


	public static IEnumerator  UpdateGameClockContinuously()
	{
		while (gameClockOn)
		{
			UpdateGameClock();
			if (autoKillAtEndTime)
			{
				if (gameDateTime > gameAutoKillTime)
				{
					TurnOffClock();
				}
			}
			yield return new WaitForSeconds(realLifeUpdateInterval);
		}
	}

	public static bool TurnOffClock()
	{
		gameClockOn = false;
		shutOffTime = DateTime.Now;
		return gameClockOn;
		
	}



}
