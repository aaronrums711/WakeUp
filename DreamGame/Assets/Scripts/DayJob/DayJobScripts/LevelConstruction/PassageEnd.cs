using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageEnd : MonoBehaviour
{
	/*****************
	CreateDate: 	9-14-2022
	Functionality:	this represents one end of a passage.  
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
    void Start()
    {
		AssignSelfToParent();
    }

    void Update()
    {
        
    }

	public void AssignSelfToParent()
	{
		if (this.transform.parent.TryGetComponent<PassageV2>(out PassageV2 parentPassage))
		{
			parentPassage.ends.Add(this);
		}
		else
		{
			Debug.LogError("this PassageEnd doesn't have a Passage parent!");
		}
	}
}
