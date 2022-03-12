using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// this simply generates a grid using some given parameters.  It doesn't necessarily have anything to do with pathfinding in particular, except for when it 
	/// checks if a node is walkable or not.  
    /// </summary>
[ExecuteAlways]
public class _Grid : MonoBehaviour
{
	/*****************
	CreateDate: 	2/3/22
	Functionality:	represents a grid of nodes, the underlying data for our pathfinding
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	public Node[,] nodeGrid;
	public Transform player;
	public LayerMask unwalkableMask;

	[Tooltip("for some reason, setting this field to >= 1 stops the path from being drawn in black.  I don't know why")]
	public float nodeRadius;	 //VGIU
	public Vector2 gridWorldSize;// VGIU
	float nodeDiameter;
	public int gridSizeX, gridSizeY;   //these are the grid size IN NODES.  
	public bool markPlayerNode;  //for testing only, it shows exactly what node the player will be assigned to. 
	public bool drawGridGizmos;
	public List<Node> path;




	void Awake()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);  //we are taking the WORLD size here and converting it into Nodes for X and Y
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}


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
				if (path != null)
				{
					if (path.Contains(n))
					{
						Gizmos.color = Color.black;
					}
				}
				if (drawGridGizmos)
				{
					Gizmos.DrawCube(n.worldPosition,  new Vector3(0.3f, 0.3f, 0.3f));        //Vector3.one * (nodeDiameter - 0.1f)); // minus 0.1 is just for spacing the cubes out 
				}

			}
		}
	}

	
	//this simply fills in the nodeGrid object, which is a 2D list of nodes that is the underlying data.  
	[ContextMenu("CreateGrid()")]
	public void CreateGrid()
	{
		nodeGrid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;  //this gives us the bottom left corner of our world...I don't know how. 
		// print("world bottom left " + worldBottomLeft);
		for (int x = 0; x<gridSizeX; x++)
		{
			for (int y = 0; y<gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
				nodeGrid[x, y] = new Node(walkable, worldPoint, x,y);  //finally populate nodeGrid with the nodes 
			}
		}
	}

	//give it a world point, it gives you the corresponding node
	public Node NodeFromWorldPoint(Vector3 worldPos)
	{
		float percentX = (worldPos.x + gridWorldSize.x/2) / gridWorldSize.x; //this is a value between 0 and 1 for how far along the passed in vector is on the X axis
		float percentY = (worldPos.z + gridWorldSize.y/2) / gridWorldSize.y; 
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		// int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int x = Mathf.FloorToInt(Mathf.Clamp((gridSizeX) * percentX, 0, gridSizeX - 1));
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return nodeGrid[x, y];
	}


	/**
	the two initial for loops are designed to search a 3x3 grid with the passed in Node as the center of that 3x3.  

	so the first iteration will check -1 X and -1 Y, which will be the diagonally down left corner from the given node. 
	next iteration will check -1X and 0Y, which will be the direct left neighbor. 
	next iteration will check -1X and 1 Y, which will be the diagnally up left corner. 

	then it will restart with 0X, and go thorugh -1Y, 0Y, and -1Y.  
	**/
	public List<Node> GetNeighbors(Node node)
	{
		List <Node> neighbors = new List<Node>();
		for (int x = -1; x<= 1; x++)
		{
			for (int y = -1; y<= 1; y++)
			{
				if (x == 0 && y == 0)
				{
					continue;
				}
				int CheckX = node.gridX + x;
				int CheckY = node.gridY + y;
				
				if (CheckX >= 0 && CheckX < gridSizeX && CheckY >= 0 && CheckY < gridSizeY)
				{
					neighbors.Add(nodeGrid[CheckX, CheckY]);
				}


			}
		}
		return neighbors;

	}


}
