using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding_Heap : MonoBehaviour
{
	/*****************
	CreateDate: 	2/4/22
	Functionality:
	Notes:
	Dependencies:
	******************/
	
	private _Grid grid;
	public Transform seeker;
	public Transform target; 



	void Awake()
	{
		grid = GetComponent<_Grid>();
	}

	void Update()
	{
		// if (Input.GetKeyDown("space"))
		// {
			FindPath(seeker.position, target.position);
		// }
	}

	public List<Vector3>  FindPath(Vector3 startPos, Vector3 endPos)
	{
		List<Vector3> pathWaypoints = new List<Vector3>();
		Node startNode = grid.NodeFromWorldPoint(startPos);	
		Node targetNode = grid.NodeFromWorldPoint(endPos);

		Heap<Node> openSet = new Heap<Node>(grid.gridSizeX*grid.gridSizeY);			//represents the  nodes that the algorithm has NOT EVER picked as the lowest Fcost. 
		
		HashSet<Node> closedSet  = new HashSet<Node>(); //holds all the nodes that have already been "picked" as the path at some point in the algorithm. 
														//this means that, at some point, these nodes had the lowest Fcost of all the known nodes
		openSet.Add(startNode);							//first we add the startNode to a list. 

		/*
		to sum up this while loop....
		the first iteration the first for loop will NOT be executed because openSet will only have 1 element.   
			**In each subsequent iteration, openSet will have all the neighbors of currentNode

		then, we remove that node from openSet and add it to closeSet

		then we check if the currentNode == targetNode.  If not, keep going

		then in the foreach we loop through all the neighbors and, if they are walkable or are not in closeSet, we 
			give each of them a Gcost, Hcost and set their parent to the currentNode (the center of the 3x3 grid of nodes)
			then we add each neighbor to openSet

		then the while loop repeats, but now openSet contains all the neighbors of the startNode
			the forloop will then check all those and see which has the lowest F cost (also taking Hcost into acct if necessary)
			and set THAT node to currentNode. 

			It will then dump the new currentNode into closedSet, because it's been picked as a path. 

			all the un-selected nodes REMAIN IN OPENSET because at a later iteration, they may actually be the node with the lowest Fcost. 

				This is what happened in the first map example in Sebastian Lagues first YT tutorial. 
				The path initiall went up and to the left, but then later on it realized that going up and to the right was better, even though
					initially that route appeared to have a higher Fcost


		*/


		while (openSet.Count > 0)						
		{
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);					

			if (currentNode == targetNode)  //if the current evaluated node == targetNode, stop everything, we've got it
			{
				break;
			}
			
			foreach (Node neighbor in grid.GetNeighbors(currentNode))  //now we check all the neighbors of currentNode.  
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
						continue;
					}
				}
			}
		}
		
		pathWaypoints = RetracePath(startNode, targetNode);
		return pathWaypoints;
	}



	//this method just creates a list of Nodes that represent the found path. 
	//so it just keeps getting the parent nodes of the EndNode, until the parentNode = startNode, then you've got the whole path
	//it is only called once in the FindPath() method
	List<Vector3> RetracePath(Node startNode, Node endNode)
	{
		List<Vector3> pathWaypoints = new List<Vector3>();
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		while (currentNode != startNode)
		{
			path.Add(currentNode);
			pathWaypoints.Add(currentNode.worldPosition);
			currentNode = currentNode.parentNode;

		}
		path.Reverse();  //the calculated path starts with the end, so this just reverses it so the path[0] is the startingNode;
		pathWaypoints.Reverse();
		grid.path = path;
		return pathWaypoints;
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
