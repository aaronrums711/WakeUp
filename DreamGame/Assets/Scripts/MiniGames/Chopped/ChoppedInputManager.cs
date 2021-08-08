using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppedInputManager : MiniGameElement, IOneKeyPlay
{
	/*****************
	CreateDate: 8/8/2021
	Functionality: handles the player input for the Chopped mini game. 
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	private ChoppedTargetSpawner spawner;

	
	
	void Awake()
    {
        
    }
	
    void Start()
    {
		spawner = GameObject.Find("LaunchPoints").GetComponent<ChoppedTargetSpawner>();
    }

    void Update()
    {
		// if (Input.GetKeyDown(parentMiniGame.keyForThisGame))
		// {
		// 	OneKeyPlay();
		// }
    }


	public void OneKeyPlay()
	{
		spawner.allTargets[0].handleHit();
	}
}
