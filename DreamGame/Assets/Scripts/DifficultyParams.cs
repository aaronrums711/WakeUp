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
	
	[Tooltip("less than 1 will add less progression for each succesful action, more than 1 will add more.  Should be used after all other calculations")]  
	public float universalProgressionMultiplier; 

	[Tooltip("should be slightly less than 1 to make games easier, slighly more than 1 to make them harder")]  
	public float universalDragMultiplier;

	[Tooltip("should be applied to most things that moves or spawn automatically.  Not everything is made harder by getting faster, sometimes the opposite")]  
	public float universalSpeedMultiplier;

	public DifficultyDescription difficulty= DifficultyDescription.easy;

}

public enum DifficultyDescription {easy, medium, hard, endGame};
