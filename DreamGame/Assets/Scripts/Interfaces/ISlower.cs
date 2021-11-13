using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlower 
{
	/*****************
	CreateDate: 11/13/21
	Functionality:  the methods in this interface will be called whever it's necessary to slow down or speed up an individual mini game. 
	Notes:			the initial usage at the moment will when spawning new games in a TimeCrunch level.  or for 
					on screen text to be displayed during the game.  This is different than slowing down activity for difficulty becausee it will also effect 
					visual aesthetic elements, like targets growing or shrinking 
	Dependencies:
	******************/
	
	public IEnumerator SlowDownMiniGame(float endRate, float changeRate);
	public IEnumerator BringBackToSpeed(float changeRate);  //no endRate is needed because the logic just uses whatever the initial values were when the SlowDownMiniGame method was called
}
