using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System;

[CreateAssetMenu(fileName = "new schedule", menuName = "schedule")]
public class Schedule : ScriptableObject
{
	/*****************
	CreateDate: 	2/11/22
	Functionality:	an instance of this object should store a list of Vectors3 (serving as Day Hour Minute) and strings which correspond to locationNames of allthe PathfindingLocation objects on the map
	Notes:
	Dependencies:
	******************/
	
	public int itemsInSchedule {get {return schedule.Count;}}
	
	private List<KeyValuePair<DateTime, PathfindingLocation>> schedule = new List<KeyValuePair<DateTime, PathfindingLocation>>();


	//these lists need to be made so that new Schedule objects can easily be made in the editor. Dictionaries (and OrderedDictionaries) don't show in the inspector. 
	
	[Tooltip("day, hour, minute")]
	[SerializeField] public List<Vector3> times;  //we are using vector3 to conveniently use three ints to represent day, hour and minute.  The Year and Month just come from the Clock class. This should make it easier to construct schedules in the editor
	[SerializeField] public List<String> destinations;
	


	void Start()
	{
		ConstructSchedule(times, destinations);
	}


	public void ConstructSchedule(List<Vector3> _times, List<String> _destinations)
	{
		if(!ValidateSchedule(_times, _destinations))
		{
			Debug.LogError("there is something wrong with the schedule on this object: " + this.name);
		}

		List<String> pathfindingLocationNames = new List<string>();
		int indexCounter = 0;
		foreach (String _locationName in destinations)
		{
			
			foreach (PathfindingLocation pfl in DayJobLevelManager.pfLocations)
			{
				if (_locationName == pfl.locationName)
				{
					DateTime dt = new DateTime(Clock.gameDateTime.Year, Clock.gameDateTime.Month, (int)_times[indexCounter].x, (int)_times[indexCounter].y, (int)_times[indexCounter].z, 0);
					schedule.Insert(indexCounter, new KeyValuePair<DateTime, PathfindingLocation>(dt, pfl));
					indexCounter++;
					break;
				}
			}
		}


		// for (int i =0; i < _times.Count; i++)
		// {
		// 	DateTime dt = new DateTime(Clock.gameDateTime.Year, Clock.gameDateTime.Month, (int)_times[i].x, (int)_times[i].y, (int)_times[i].z, 0);
		// 	schedule.Insert(i, new KeyValuePair<DateTime, Transform>(dt, _destinations[i]));
		// }
	}


	public bool ValidateSchedule(List<Vector3> _times, List<String> _destinations)
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
			DateTime dt = new DateTime(Clock.gameDateTime.Year, Clock.gameDateTime.Month, (int)_times[i].x, (int)_times[i].y, (int)_times[i].z, 0);
			if (dt < tempDT)
			{
				Debug.LogError("the dates are not in ascending order!");
				scheduleIsValid = false;
				return scheduleIsValid;
			}
			tempDT = dt;
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
			Debug.Log("Item " + i + ": " + schedule[i].Key + "   "  + schedule[i].Value);
		}
	}


	/**
	UPON RETURN:  	switch the second list to be a list of strings, then check against the list from DayJobLevelManager to make sure it exists there.  Then 
					add the position for that PathfindingLocation to schedule
	**/
	
}
