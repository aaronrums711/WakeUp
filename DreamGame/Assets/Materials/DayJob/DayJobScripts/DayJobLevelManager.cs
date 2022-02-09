using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Clock.InitializeGameClock(new System.DateTime(2022,09,01,09,01,01), 10, 3);  ///september 1st, 9:00 AM
		StartCoroutine(Clock.UpdateGameClockContinuously());
		StartCoroutine(DisplayGameTime());
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
			yield return new WaitForSeconds(Clock.realLifeUpdateInterval);
		}
	}

}
