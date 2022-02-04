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

	public int Gcost;  //distance from this node to starting node
	public int Hcost;  //distance  this node to the destination

	public int gridX; //this is the X coord of this node in the larger grid
	public int gridY; //this is the Y coord of this node in the larger grid

	public Node parentNode;

	

	public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
	{
		walkable = _walkable;
		worldPosition = _worldPosition;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int Fcost{ get {return Gcost + Hcost;}}
	
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
