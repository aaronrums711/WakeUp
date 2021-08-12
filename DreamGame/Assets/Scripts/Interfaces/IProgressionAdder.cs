using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgressionAdder
{
	/*****************
	CreateDate: 8/12/21
	Functionality: any script that actually calls the parentMiniGame.AddProgression method should implement this. 
	Notes:  
	Dependencies:
	******************/
	
	public void AddMiniGameProgress();
}
