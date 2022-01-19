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

	public Item testItem;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	


    void Start()
    {
		thisTransform = GetComponent<Transform>();
		Item.triggerExit += CallRotateBack;
    }

    void Update()
    {
		SlowlyRotate();
    }

	//simple, immediate rotation.  We want something slow
	public void SnapRotate()
	{
		for (int i = 0; i< availableItems.Count; i++)
		{
			availableItems[i].transform.LookAt(this.transform);
		}
	}

	public void SlowlyRotate()
	{
		for (int i = 0; i< availableItems.Count; i++)
		{
			Vector3 lookAtDirection = this.transform.position - availableItems[i].transform.position;
			Quaternion lookAtRotation = Quaternion.LookRotation(lookAtDirection, Vector3.up);

			availableItems[i].transform.rotation = Quaternion.RotateTowards(availableItems[i].transform.rotation, lookAtRotation, speed * Time.deltaTime);
		}
	}

	public IEnumerator RotateItemBackToStart(Item item)
	{
		while (item.transform.rotation != item.initRotation && !availableItems.Contains(item))
		{
			item.transform.rotation = Quaternion.RotateTowards(item.transform.rotation, item.initRotation, speed * Time.deltaTime);
			yield return null;
		}	
	}

	[ContextMenu("rotate back")]
	public Item CallRotateBack(Item item)
	{
		StartCoroutine(RotateItemBackToStart(item));
		return item;
	}

}
