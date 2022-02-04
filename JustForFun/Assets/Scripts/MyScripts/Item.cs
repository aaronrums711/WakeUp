using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Item : MonoBehaviour
{
	/*****************
	CreateDate: 	01/18/22
	Functionality:	
	Notes:			this class will just add and remove itself to the list on all Effects
	Dependencies:
	******************/
	
	//////////////////////////////Config
	private Collider thisCollider;
	[SerializeField] public Quaternion initRotation;

	public static Func< Item, Item> triggerEnter;
	public static Func< Item, Item> triggerExit;
	 
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
    void Start()
    {
        thisCollider = GetComponent<Collider>();
		initRotation = this.transform.rotation;
    }

    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent(typeof(ItemEffector), out Component eff))
		{
			eff.GetComponent<ItemEffector>().AddItem(this);
			if (triggerEnter != null) 
			{
				triggerEnter(this);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{

		if (other.gameObject.TryGetComponent(typeof(ItemEffector), out Component eff))
		{
			eff.GetComponent<ItemEffector>().RemoveItem(this);
			
			
			if (triggerExit != null) 
			{
				triggerExit(this);
			}
		}

	}

}
