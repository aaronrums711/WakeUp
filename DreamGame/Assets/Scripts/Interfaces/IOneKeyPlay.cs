using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOneKeyPlay 
{
	/*****************
	CreateDate: 8/8/2021
	Functionality: this interface should be implemented by all classes that take in the player input for a mini game
	Notes:  the OneKeyPlay method should be the method that is called after player input.  Meaning, it DOES NOT include the check for player input. 
			this way, it can be called from another script, and it'll get executed without input
	Dependencies:
	******************/

	
	void OneKeyPlay();
}
