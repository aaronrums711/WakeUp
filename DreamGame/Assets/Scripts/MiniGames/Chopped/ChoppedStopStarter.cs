using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppedStopStarter : MiniGameElement, IStoppable
{
	/*****************
	CreateDate: 8/26/21 
	Functionality: 
	Notes: 		contains the implementations for the methods in IStoppable
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
    void Start()
    {
        
    }

    void Update()
    {
        
    }


	public void StopMiniGame()
	{
		parentMiniGame.isActive = false;
		return;
	}
	public void RestartMiniGame()
	{
		return;
	}
}
