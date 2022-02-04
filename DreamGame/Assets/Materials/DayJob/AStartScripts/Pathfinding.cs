using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
	/*****************
	CreateDate: 
	Functionality:
	Notes:
	Dependencies:
	******************/
	
	public Grid grid;
	public Transform seeker;
	public Transform target; 



	void Awake()
	{
		grid = GetComponent<Grid>();
	}

	void Update()
	{
		FindPath(seeker.position, target.position);
	}

	void FindPath(Vector3 startPos, Vector3 endPos)
	{
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(endPos);

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet  = new HashSet<Node>();

		openSet.Add(startNode);
		while (openSet.Count > 0)
		{
			Node currentNode = openSet[0];

			for (int i = 1; i< openSet.Count; i++)
			{
				if (openSet[i].Fcost < currentNode.Fcost ||  openSet[i].Hcost == currentNode.Hcost && openSet[i].Hcost < currentNode.Hcost)
				{
					currentNode = openSet[i];
				}
			}
			openSet.Remove(currentNode);
			closedSet.Add(currentNode);

			if (currentNode == targetNode)
			{
				return;
			}
			
			foreach (Node neighbor in grid.GetNeighbors(currentNode))
			{
				if (!neighbor.walkable || closedSet.Contains(neighbor))
				{
					continue;
				}

				int newMovementCostToNeighbor = currentNode.Gcost + GetDistance(currentNode, neighbor);
				if(newMovementCostToNeighbor < neighbor.Gcost  || !openSet.Contains(neighbor))
				{
					neighbor.Gcost = newMovementCostToNeighbor;
					neighbor.Hcost = GetDistance(neighbor, targetNode);
					neighbor.parentNode = currentNode;

					if (!openSet.Contains(neighbor))
					{
						openSet.Add(neighbor);
					}
				}

			}

		}




	}




	//this method just creates a list of Nodes that represent the found path. 
	//so it just keeps getting the parent nodes of the EndNode, until the parentNode = startNode, then you've got the whole path
	void RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parentNode;

		}
		path.Reverse();  //the calculated path starts with the end, so this just reverses it so the path[0] is the startingNode;


	}


		///see 15:00 https://www.youtube.com/watch?v=mZfyt03LDH4&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW&index=3  to get the logic of getting the distance here
		//remember, we can't just use the built in Distance method because that would give us the distance in a straight line, but we need the distance when travelling on our grid
	public int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX =  Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY =  Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
		{
			return 14*dstY + 10 * (dstX - dstY);
		}
		else
		{
			return 14*dstX + 10 * (dstY - dstX);
		}
			
	}
}
