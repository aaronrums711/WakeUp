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