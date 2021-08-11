using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroyerRingChanger : MiniGameElement
{
	/*****************
	CreateDate: 7/31/21
	Functionality: this only triggers animations.  these animations shift around the sizes of the protecive ring edges, for some added fun
	******************/
	
	//////////////////////////////Config
    private const string anim1 = "MovePanels1";
	private const string anim2 = "MovePanels2";
    private const string anim3 = "MovePanels3";


	//////////////////////////////State
	
	//////////////////////////////Cached Component References
    private Animator thisAnimator;
	
	
	void Awake()
    {
        GetParentMiniGame();
    }
	
    void Start()
    {
        thisAnimator = GetComponentInParent<Animator>();
        thisAnimator.Play(anim2);
        StartCoroutine(ContinuallyMovePanels());
    }


    public IEnumerator ContinuallyMovePanels()
    {
        int animToPlay = Random.Range(1,4);
        if (animToPlay == 1)
        {
            thisAnimator.Play(anim1);
        }
        else if (animToPlay == 2)
        {
            thisAnimator.Play(anim2);
        }
        else if (animToPlay == 3)
        {
            thisAnimator.Play(anim3);
        }
        yield return new WaitForSeconds(12f);  //this is longer than 10 seconds which is the length of the longest of these anims.  This way they won't overlap
        StartCoroutine(ContinuallyMovePanels());
    }
}
