using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
	/*****************
	CreateDate: 	2/12/22
	Functionality:	the parent class for all NPCs
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public Coroutine movementCoroutine;
	public float movementSpeed;
	public Pathfinding_Heap pathfinding;

	
	//////////////////////////////State
	public bool canMove;
	public bool loopPath;

	//////////////////////////////Cached Component References
	
}
