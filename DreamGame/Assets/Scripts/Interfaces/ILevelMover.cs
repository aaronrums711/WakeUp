using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelMover
{
	/*****************
	CreateDate: 	10/24/21
	Functionality:	this will be implemented by all children of the LevelManager class. 
					Each child will implement the method in a different way based off of the level type that it is manager. 
	Notes:
	Dependencies:
	******************/


	public void MoveLevelForward();
}
