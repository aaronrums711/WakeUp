using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerBarUpDown : MonoBehaviour
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
	public Vector3 startingPosVec;  //VGIU
	public Vector3 startingScaleVec; //VGIU
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	public Transform thisTransform;
	public MiniGame parentMiniGame;
	
	void Start()
	{
		if (parentMiniGame == null)
		{
			parentMiniGame = GetComponent<MiniGame>(); 
    		if (parentMiniGame == null)
			{
				parentMiniGame = GetComponentInParent<MiniGame>();
			}
				if (parentMiniGame == null)
				{
					parentMiniGame = GetComponentInParent<Transform>().GetComponentInParent<MiniGame>();  //for scripts on a child's child
				}
		}
		thisTransform = this.transform;
		thisTransform.localPosition = startingPosVec;
		thisTransform.localScale = startingScaleVec;
		startingScale = startingScaleVec.x;
		startingPos = startingPosVec.x;
		// print("initial local pos: " + startingPosVec);
	}
    void Update()
    {
        ExtendRectangle();
    }


	
	public void ExtendRectangle()
	{
		Vector3 currentScale = thisTransform.localScale;
		float newScale = Mathf.Lerp(startingScale, maxScale, parentMiniGame.completionPercent);
		currentScale.x = newScale;
		thisTransform.localScale = currentScale;

		Vector3 currentPos = thisTransform.localPosition;
		float newPos = Mathf.Lerp(startingPos, maxPos, parentMiniGame.completionPercent);
		currentPos.x = newPos;
		thisTransform.localPosition = currentPos;

	}
}
