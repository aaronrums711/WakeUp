using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new progression params", menuName = "progression params")]
public class ProgressionParams : ScriptableObject
{
	/*****************
	CreateDate: 	9/9/21
	Functionality:	an instance of this object will be slotted into to every mini game. This should be able to be re-used accross all mini games, so there
					should only be 1 for each difficulty level.  
	Notes:			this object controls how much progression is recieved for each action, or how much the progression drags automatically
	Dependencies:
	******************/
	
	[Tooltip("less than 1 will add less progression for each succesful action, more than 1 will add more.  Should be used after all other calculations, use this for per second mini game progression")]  
	public float universalProgressionMultiplier; 

	[Tooltip("less than 1 will add less progression for each succesful action, more than 1 will add more.  Use this in cases where the progression is added in chunks, not per second")]  
	public float universalProgressionChunkMultiplier; 

	[Tooltip("should be slightly less than 1 to make games easier, slighly more than 1 to make them harder")]  
	public float universalDragMultiplier;
	public DifficultyDescription difficultyDescription;

}


