using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new diff params", menuName = "difficulty params")]
public class DifficultyParams : ScriptableObject
{
	/*****************
	CreateDate: 	9/9/21
	Functionality:	an instance of this will be slotted into a mini game object.  Each mini game may need different parameters to achieve the same level of difficulty, 
					so it can't go in the level object.  But it will use the GeneralDifficulty field of the Level object to determine which parameters to use. 
	Notes:
	Dependencies:
	******************/
	
	[Tooltip("this grows as difficulty increases, and should be used on things that need to be faster or happen  more frequently as difficulty increases")]  
	public float scaleUpMultiplier;

	[Tooltip("this shrinks as difficulty increases, and should be used on things that need to be slower or happen less often as difficulty increases")]  
	public float scaleDownMultiplier;

	public DifficultyDescription difficultyDescription;
}


