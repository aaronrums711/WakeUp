using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
	/*****************
	CreateDate: 	2/4/22
	Functionality:	this creates a generic heap data structure  ---- https://www.youtube.com/watch?v=3Dw5d7PlcTM&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW&index=4  for a great explanation on heaps
	Notes:			it is not strictly necessary for pathfinding, but it makes it faster because, using a heap, the pathfinding algorithm won't have to loop through EVERY SINGLE node in openSet to find the one with the lowest Fcost
	Dependencies:
	******************/
	
	T[] items;
	int currentItemCount;

	//constr
	public Heap(int maxHeapSize)
	{
		items = new T[maxHeapSize];
	}

	public void Add (T item)
	{
		item.HeapIndex = currentItemCount;
		items[currentItemCount] = item;
		SortUp(item);
		currentItemCount++;
	}

	void SortUp(T item)
	{
		int parentIndex = (item.HeapIndex-1)/2;  //this formula is given in the above YT video. It gets the index the parent in the heap

		while(true)
		{
			T parentItem = items[parentIndex];
			if (item.CompareTo(parentItem) > 0)
			{
				
			}
		}

	}


	void Swap (T itemA, T itemB)
	{
		items[itemA.HeapIndex] = itemB;
		items[itemB.HeapIndex] = itemA;

		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}



}



public interface IHeapItem<T> : IComparable<T>
{
	int HeapIndex{get; set; }
}
