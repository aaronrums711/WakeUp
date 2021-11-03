using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerBarLeftRight : MonoBehaviour
{
	/*****************
	CreateDate: 	11/15/21
	Functionality:	handles the movement of the inner bars of the play areas so that they grow as the mini game progression grows. 
	Notes:			this could potentially be a child of the miniGameElement class...but it has no other functionality, so I'm not gonna do that
	Dependencies:	needs a reference to a MiniGame
	******************/
	
	//////////////////////////////Config
	public float maxPos;
	public float maxScale;
	public float startingPos;
	public float startingScale;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	public Transform thisTransform;
	public MiniGame parentMiniGame;
	
	void Start()
	{
		thisTransform = this.transform;
		startingScale = this.transform.localScale.y;
		startingPos = this.transform.position.y;
	}
    void Update()
    {
        ExtendRectangle();
    }


	
	public void ExtendRectangle()
	{
		Vector3 currentScale = thisTransform.localScale;
		float newScale = Mathf.Lerp(startingScale, maxScale, parentMiniGame.completionPercent);
		currentScale.y = newScale;
		thisTransform.localScale = currentScale;

		Vector3 currentPos = thisTransform.position;
		float newPos = Mathf.Lerp(startingPos, maxPos, parentMiniGame.completionPercent);
		currentPos.y = newPos;
		thisTransform.position = currentPos;

	}
}
