using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffector : MonoBehaviour
{
	/*****************
	CreateDate: 	11/18/22
	Functionality:	parent class from which the effects will derive
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public List<Item> availableItems;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	


	public Item AddItem(Item go)
	{
		availableItems.Add(go);
		return go;
	}

	public Item RemoveItem(Item go)
	{
		availableItems.Remove(go);
		return go;
	}
}
