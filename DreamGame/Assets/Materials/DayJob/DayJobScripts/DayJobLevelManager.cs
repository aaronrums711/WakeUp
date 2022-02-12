using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


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
	public static List<PathfindingLocation> pfLocations;

	
	void Awake()
    {
		pfLocations = GameObject.FindObjectsOfType<PathfindingLocation>().ToList<PathfindingLocation>();
		StartCoroutine(Clock.UpdateGameClockContinuously(new DateTime(2022,09,01,09,01,01), 400, 1, false, DateTime.MinValue));
    }
	
    void Start()
    {
        
    }

    void Update()
    {

    }



}
