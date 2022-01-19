using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotator : ItemEffector
{
	/*****************
	CreateDate: 	
	Functionality:
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	private Transform thisTransform;
	public float speed;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	


    void Start()
    {
		thisTransform = GetComponent<Transform>();
    }

    void Update()
    {
		// RotateAllItemsInRange();
		RotateAllAvailableItems();
    }

	//simple, immediate rotation.  We want something slow
	public void RotateAllAvailableItems()
	{
		for (int i = 0; i< availableItems.Count; i++)
		{
			availableItems[i].transform.LookAt(this.transform);
		}
	}

	public void RotateAllItemsInRange()
	{
		for (int i = 0; i< availableItems.Count; i++)
		{
			availableItems[i].transform.rotation = Quaternion.RotateTowards(availableItems[i].transform.rotation, this.transform.rotation, speed * Time.deltaTime);
		}

	}
}
