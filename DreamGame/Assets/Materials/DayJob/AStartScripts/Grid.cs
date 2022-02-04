using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
	/*****************
	CreateDate: 
	Functionality:
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	Node[,] nodeGrid;
	public Transform player;
	public LayerMask unwalkableMask;

	public float nodeRadius;
	public Vector2 gridWorldSize;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	public bool markPlayerNode;  //for testing only, it shows exactly what node the player will be assigned to. 


	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (nodeGrid != null)
		{
			Node playerNode = NodeFromWorldPoint(player.position);
			foreach (Node n in nodeGrid)
			{
		
				Gizmos.color = (n.walkable)? Color.white : Color.red;
				if (playerNode == n && markPlayerNode)
				{
					Gizmos.color = Color.cyan;
				}
				Gizmos.DrawCube(n.worldPosition,  Vector3.one * (nodeDiameter - 0.1f)); // minus 0.1 is just for spacing the cubes out 

			}
		}
	}

	
	void Start()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	void CreateGrid()
	{
		nodeGrid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;  //this gives us the bottom left corner of our world...I don't know how. 
		for (int x = 0; x<gridSizeX; x++)
		{
			for (int y = 0; y<gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
				nodeGrid[x, y] = new Node(walkable, worldPoint);  //finally populate nodeGrid with the nodes 
			}
		}
	}

	public Node NodeFromWorldPoint(Vector3 worldPos)
	{
		float percentX = (worldPos.x + gridWorldSize.x/2) / gridWorldSize.x; //this is a value between 0 and 1 for how far along the passed in vector is on the X axis
		float percentY = (worldPos.z + gridWorldSize.y/2) / gridWorldSize.y; 
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return nodeGrid[x, y];
	}


}
