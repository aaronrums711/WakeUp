using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new schedule", menuName = "schedule")]
public class Schedule : ScriptableObject
{
	/*****************
	CreateDate: 	2/11/22
	Functionality:	an instance of this object should store a list of Vector2s (serving as Hour Minute) and strings which correspond to locationNames of allthe PathfindingLocation objects on the map
	Notes:			currently the ConstructScheule method assumes that a schedule will never stretch into a new day (or year/Month), and therefore is just uses the Clock class to get the year, month and day
	Dependencies:
	******************/
	
	public int itemsInSchedule {get {return schedule.Count;}}
	
	public List<KeyValuePair<DateTime, PathfindingLocation>> schedule = new List<KeyValuePair<DateTime, PathfindingLocation>>();


	//these lists need to be made so that new Schedule objects can easily be made in the editor. Dictionaries (and OrderedDictionaries) don't show in the inspector. 
	
	[Tooltip("hour, minute")]
	[SerializeField] public List<Vector2> times;  //we are using vector3 to conveniently use three ints to represent hour and minute.  The Year and Month just come from the Clock class. This should make it easier to construct schedules in the editor
	[SerializeField] public List<String> destinations;
	

	public void ConstructSchedule(List<Vector2> _times, List<String> _destinations)
	{
		if(!ValidateSchedule(_times, _destinations))
		{
			Debug.LogError("there is something wrong with the this schedule: " + this.name);
			return;
		}

		List<String> pathfindingLocationNames = new List<string>();
		int indexCounter = 0;
		foreach (String _locationName in destinations)
		{
			foreach (PathfindingLocation pfl in DayJobLevelManager.pfLocations)
			{
				if (_locationName == pfl.locationName)
				{
					DateTime dt = new DateTime(Clock.gameDateTime.Year, Clock.gameDateTime.Month, Clock.gameDateTime.Day, (int)_times[indexCounter].x, (int)_times[indexCounter].y, 0);
					schedule.Insert(indexCounter, new KeyValuePair<DateTime, PathfindingLocation>(dt, pfl));
					indexCounter++;
					break;
				}
			}
		} 
	}


	public bool ValidateSchedule(List<Vector2> _times, List<String> _destinations)
	{
		bool scheduleIsValid;
		DateTime tempDT = DateTime.MinValue;
		if (_destinations.Count != _times.Count)
		{
			Debug.LogError("the Times and Destinations lists don't have an equal number of items!  Something is wrong!");
			scheduleIsValid = false;
			return scheduleIsValid;
		}

		for (int i =0; i < _times.Count; i++)
		{
			DateTime dt = new DateTime(Clock.gameDateTime.Year, Clock.gameDateTime.Month,  Clock.gameDateTime.Day,(int)_times[i].x, (int)_times[i].y, 0);
			if (dt < tempDT)
			{
				Debug.LogError("the dates are not in ascending order!");
				scheduleIsValid = false;
				return scheduleIsValid;
			}
			tempDT = dt;
		}

		
		foreach (String _locationName in destinations)
		{
			bool tempBool = false;
			foreach (PathfindingLocation pfl in DayJobLevelManager.pfLocations)
			{
				if (_locationName == pfl.locationName)
				{
					tempBool = true;
					break;
				}
			}
			if (tempBool == false) //after each _locationName, check the temp variable.  If it never got set to true, something is wrong
			{
				Debug.LogError("one of the location names isn't valid!");
				scheduleIsValid = false;
				return scheduleIsValid;
			}
		} 

		scheduleIsValid = true;
		return scheduleIsValid;

		//add another check to make sure each item in the list of strings is indeed in the DayJobLevelManager.pfLocations
	}

	public void PrintFinalSchedule()
	{
		Debug.Log("total items in schedule: " + schedule.Count);
		for (int i =0; i < schedule.Count; i++)
		{
			Debug.Log("Item " + i + ": " + schedule[i].Key + "   "  + schedule[i].Value.locationName);
		}
	}



	
}
