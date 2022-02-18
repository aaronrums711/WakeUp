using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
	/*****************
	CreateDate: 	2/3/22
	Functionality:
	Notes:
	Dependencies:
	******************/
	
	
	public bool walkable;
	public Vector3 worldPosition;

	public int Gcost;  	//distance from this node to starting node
	public int Hcost;  	//distance  this node to the destination
						//these are set by the pathFinding class, because they are relative to the start and destination

	public int gridX; //this is the X coord of this node in the larger grid
	public int gridY; //this is the Y coord of this node in the larger grid
	public int heapIndex;

	public Node parentNode;

	public int HeapIndex
	{
		get {return heapIndex;}
		set {heapIndex = value;}
	}

	//we want to return 1 if the calling item has GREATER prioritiy than the nodeToCompare, 0 if they are the same, and -1 if calling item has LESS priority than nodeToCompare
	public int CompareTo(Node nodeToCompare)
	{
		int compare =  Fcost.CompareTo(nodeToCompare.Fcost);
		if (compare == 0)
		{
			compare = Hcost.CompareTo(nodeToCompare.Hcost);
		}
		return -compare;

	}

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
