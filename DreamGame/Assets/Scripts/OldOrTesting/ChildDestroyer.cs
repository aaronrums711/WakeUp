using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildDestroyer : MiniGameElement
{
	/*****************
	CreateDate: 
	Functionality:
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
		{
			GetComponent<ColorLerpTest>().AllSRs.Remove(this.transform.GetChild(0).GetComponent<SpriteRenderer>());
			Destroy(this.transform.GetChild(0).gameObject);
		}   
    }

	

}
