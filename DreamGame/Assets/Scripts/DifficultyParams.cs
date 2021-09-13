using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new diff params", menuName = "difficulty params")]
public class DifficultyParams : ScriptableObject
{
	/*****************
	CreateDate: 	9/9/21
	Functionality:	an instance of this will be slotted into a Level object, to, together, create the level type and it's difficulty. 
					this should contain parameters that effect the difficulty of all mini games, regardles of level type or what mini game it is. 
					Things like UniversalDragAddition, ProgressAddition/Subtraction, UniversalSpeed etc. 
	Notes:
	Dependencies:
	******************/
	
	[Tooltip("should be applied to most things that moves or spawn automatically.  Not everything is made harder by getting faster, sometimes the opposite")]  
	public float universalSpeedMultiplier;

	[Tooltip("use this in the that a single game needs 2 different speed multipliers, to make one thing faster and another slower, for example")]  
	public float secondaryUniversalSpeedMultiplier;

	public DifficultyDescription difficulty;
	public QuicknessToComplete quicknessToComplete;
	public SpeedDescription speedDescription;
}


