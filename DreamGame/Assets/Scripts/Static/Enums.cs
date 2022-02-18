using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	/*****************
	CreateDate: 	9/9/21
	Functionality:	just stores some enums
	Notes:			all Level scriptable objects should have a LevelType attributes.  All DifficultyParams should have a DifficultyDescription
	Dependencies:
	******************/

public enum LevelType
{
	TimeCrunch, Sprint, MaintainTheGame, Rotation, Doubles
};

public enum DifficultyDescription
{
	easy, medium, hard, endGame
};


public enum QuicknessToComplete
{
	slow, average, fast, veryFast
};

public enum SpeedDescription
{
	slow, average, fast, veryFast
};

public enum skills
{
	precision, rhythm, speed, attention, strength
}

//this can be used to label  mini games so they can be used for different purposes.  Not exactly sure if it'll ever be implemented, but might as well create it. 
//every MiniGame object will have one of these.  
public enum InherentComplexity
{
	simple, average, complex
}


//used by the Officer class
public enum MovementState
{
	notStartedPath, followingPath, waitingAtWaypoint, finishedAllWaypoints
	//an officer will start at notStartedPath, and flip between followingPath and waitingAtWaypoint as they go through and wait at different waypoints, then finally go to finishedAllWaypoints
};
