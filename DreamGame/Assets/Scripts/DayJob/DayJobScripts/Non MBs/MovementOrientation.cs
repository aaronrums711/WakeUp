using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new movementOrientation", menuName = "MovementOrientation")]
public class MovementOrientation:ScriptableObject
{
	/*****************
	CreateDate: 	2/17/22
	Functionality:	this is a data structure that contains some Vector3s for which direction will correspond to visually moving right
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public string description;
	public int order;
	public string position;
	public int Yrotation;
	public Vector3 rightWorldDirection;
	public Vector3 cameraViewingDirection;
	public Vector3 leftWorldDirection
	{
		get{return -rightWorldDirection;}
	}

	public MovementOrientation nextMovementOrientation;
	public MovementOrientation previousMovementOrientation;
}


