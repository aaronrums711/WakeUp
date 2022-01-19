using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	/*****************
	CreateDate: 	01/18/22
	Functionality:	
	Notes:			this class will just add itself to the list on all Effects
	Dependencies:
	******************/
	
	//////////////////////////////Config
	private Collider thisCollider;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
    void Start()
    {
        thisCollider = GetComponent<Collider>();
    }

    void Update()
    {
        
    }
}
