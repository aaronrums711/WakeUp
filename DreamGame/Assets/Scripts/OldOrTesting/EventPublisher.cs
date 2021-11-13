using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventPublisher : MonoBehaviour
{
	/*****************
	CreateDate: 
	Functionality: this script ONLY fires off the event when space is pressed.  It does not know and does not which methods are subscribed to this event, if any!
	Notes:
	Dependencies:
	******************/
	
	public event EventHandler OnSpacePressed;
	

    void Start()
    {
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (OnSpacePressed!= null) OnSpacePressed(this, EventArgs.Empty);
			//alternative using the null conditional operator
			// OnSpacePressed?.Invoke(this, EventArgs.Empty);
		}
    }


	// private void Testing_OnSpacePressed(object sender, EventArgs e)  //this can be assigned to the OnSpacePressed event because it  matches the signature of the event
	// {
	// 	print("this was called through the OnSpacePressed event!");
	// }
	

}
