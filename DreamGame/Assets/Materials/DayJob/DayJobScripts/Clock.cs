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
	public static float timeSpeed;
	private static DateTime realLifeStartTime;

	public static DateTime startDateTime;
	public static DateTime gameDateTime;
	public static int gameYear;
	public static int gameMonth;
	public static int gameDay;
	public static int gameHour;
	public static int gameMinuteWithinHour;
	

	

	static void UpdateValues()
	{
		gameYear = gameDateTime.Year;
		gameMonth = gameDateTime.Month;
		gameDay = gameDateTime.Day;
		gameHour = gameDateTime.Hour;
		gameMinuteWithinHour = gameDateTime.Minute;
	}

	//this can be used like a constructor; 
	public static void InitializeGameClock(DateTime gameStartTime)
	{
		realLifeStartTime = DateTime.Now;
		gameDateTime = gameStartTime;
	}

	/**
	UPON RETURN:  use a TimeSpan elapsed to determine the difference between the DateTime.Now and the realLifeStartTime. 
	Then multiply the seconds by timeSpeed, and  gameDateTime += elapsed;

	this should pass the time realistically, but at a faster speed governed by timeSpeed;
	**/
}
