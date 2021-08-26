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
        GetParentMiniGame();  //I have not idea why, but this is not getting called automatically like it's supposed to, so I'm calling it here. 
    }
	
    void Start()
    {
		spawner = GameObject.Find("LaunchPoints").GetComponent<ChoppedTargetSpawner>();
    }

    void Update()
    {
		if (Input.GetKeyDown(parentMiniGame.keyForThisGame) && parentMiniGame.isActive == true)
		{
			if (spawner.allTargets.Count > 0)
			{
				OneKeyPlay();
			}
			if (spawner.allTargets.Count <= 0)
			{
				parentMiniGame.AddProgress(parentMiniGame.baseProgressionChunk * -1);
			}
		}
    }


	public void OneKeyPlay()
	{
		spawner.allTargets[0].CallHandleHit();
	}
}
