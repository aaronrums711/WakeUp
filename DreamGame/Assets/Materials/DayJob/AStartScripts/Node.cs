using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
	/*****************
	CreateDate: 	2/3/22
	Functionality:
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	public bool walkable;
	public Vector3 worldPosition;

	public Node(bool _walkable, Vector3 _worldPosition)
	{
		walkable = _walkable;
		worldPosition = _worldPosition;
	}
	
	void Awake()
    {
        
    }
	
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
