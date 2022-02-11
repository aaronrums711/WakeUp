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
	public static DateTime shutOffRealTime;			

	public static int gameYear;
	public static int gameMonth;
	public static int gameDay;
	public static int gameHour;
	public static int gameMinuteWithinHour;
	public static bool gameClockOn = false;
	public static bool autoKillAtEndTime = false;

	private static bool eventCalledForThisHour = false;
	private static bool eventCalledForThisHalfHour = true;
	public static int lastHour;

	public static Action<DateTime> halfHourTimer; //this will get called every half hour, or on the update closest to that.  

	public static TimeSpan RealTimeSinceStart
	{
		get{return DateTime.Now - initializedAtRealTime;}
	}

	


	//this can be used like a constructor; 
	public static void InitializeGameClock(DateTime _gameDateTime, int _timeSpeed, int _realLifeUpdateInterval, bool _autoKillAtEndTime, DateTime _endDateTime)
	{
		timeSpeed = _timeSpeed;
		initializedAtRealTime = DateTime.Now;
		realLifeUpdateInterval = _realLifeUpdateInterval;
		gameDateTime = _gameDateTime;
		lastHour = (gameDateTime.AddHours(-1)).Hour;
		gameClockOn = true;

		if (_autoKillAtEndTime)
		{
			gameAutoKillTime = _endDateTime;
			autoKillAtEndTime = _autoKillAtEndTime;
		}
	}

	private static void UpdateValues()
	{
		gameYear = gameDateTime.Year;
		gameMonth = gameDateTime.Month;
		gameDay = gameDateTime.Day;
		gameHour = gameDateTime.Hour;
		gameMinuteWithinHour = gameDateTime.Minute;
	}

	private static DateTime UpdateGameClock()
	{
		int secondsElapsed = (int)(realLifeUpdateInterval * timeSpeed);  //becasue the UpdateGameClockContinuously() is using realLifeUpdateInterval as its yield, we know that thats how many seconds have elapsed
		TimeSpan ts = new TimeSpan(0,0,secondsElapsed);  //this constr is hours, minutes seconds, 

		gameDateTime += ts;
		UpdateValues();
		return gameDateTime;
	}


	//the purpose of these params, only to then be passed in to InitializeGameClock() is so that any manager script only has to call UpdateGameClockContinuously, instead ofInitializeGameClock() AND  UpdateGameClockContinuously().  And I couldn't call StartCoroutine() in InitializegameClock because this isn't a monobehavior
	public static IEnumerator  UpdateGameClockContinuously(DateTime _gameDateTime, int _timeSpeed, int _realLifeUpdateInterval, bool _autoKillAtEndTime, DateTime _endDateTime)
	{
		InitializeGameClock(_gameDateTime, _timeSpeed, _realLifeUpdateInterval, _autoKillAtEndTime, _endDateTime);  
		while (gameClockOn)
		{
			UpdateGameClock();
			CheckToCallEvent();
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
		shutOffRealTime = DateTime.Now;
		return gameClockOn;
	}


	//the DateTime we want to pass into the event is the gameDateTime ROUNDED to the nearest hour or half hour.  
	public static void CheckToCallEvent()
	{
		if (gameDateTime.Hour > lastHour)
		{
			eventCalledForThisHour = false;
			eventCalledForThisHalfHour = false;
			lastHour = gameDateTime.Hour;
		}
		
		if (eventCalledForThisHour == false && gameMinuteWithinHour > 0 && gameMinuteWithinHour < 30)
		{
			if (halfHourTimer != null)
			{
				DateTime dt = new DateTime(gameDateTime.Year,gameDateTime.Month, gameDateTime.Day, gameDateTime.Hour, 0, 0);
				halfHourTimer(dt);
			}
			eventCalledForThisHour = true;
		}

		if (eventCalledForThisHalfHour == false && gameMinuteWithinHour > 30)
		{
			if (halfHourTimer != null)
			{
				DateTime dt = new DateTime(gameDateTime.Year,gameDateTime.Month, gameDateTime.Day, gameDateTime.Hour, 30, 0);
				halfHourTimer(dt);
			}
			eventCalledForThisHalfHour = true;
		}
	}



}
