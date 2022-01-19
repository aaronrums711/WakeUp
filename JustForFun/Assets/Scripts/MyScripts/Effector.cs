using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector : MonoBehaviour
{
	/*****************
	CreateDate: 	11/18/22
	Functionality:	parent class from which the effects will derive
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public List<GameObject> availableItems;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
	void Awake()
    {
        
    }
	
    void Start()
    {
        
    }

    void Update()
    {
        
    }

	public GameObject AddItem(GameObject go)
	{
		availableItems.Add(go);
		return go;
	}

	public GameObject RemoveItem(GameObject go)
	{
		availableItems.Remove(go);
		return go;
	}
}
