using UnityEngine;
using System;

public class EventSub : MonoBehaviour
{
	/****
	this class ONLY fires off the event when 
	***/


	public EventPublisher del;

	
    void Start()
    {
    	del =  GetComponent<EventPublisher>();
		del.OnSpacePressed += NewFunction;
    }

	private void NewFunction(object sender, EventArgs a)
	{
		print("called from the EventSub class");
	}
}
