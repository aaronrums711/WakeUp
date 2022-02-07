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

					the main rule of a heap is that each index has up to 2 chilren, and each parent must be less than each of it's children.  
	
	
	
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
				Swap(item, parentItem);
			}
			else
			{
				break;
			}
			parentIndex = (item.HeapIndex-1)/2;
		}

	}

	////UPON RETURN: I stopped around 9:00 of the above video
	void Swap (T itemA, T itemB)
	{
		items[itemA.HeapIndex] = itemB;
		items[itemB.HeapIndex] = itemA;

		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}

	public T RemoveFirst()
	{
		T firstItem = items[0];
		currentItemCount--;
		items[0] = items[currentItemCount];
		items[0].HeapIndex = 0;
		SortDown(items[0]);
		return firstItem;

	}

	void SortDown(T item)
	{
		while (true)
		{
			int childIndexLeft = item.HeapIndex*2 + 1;
			int childIndexRight = item.HeapIndex*2 + 2;
			int swapIndex = 0;
			
			if (childIndexLeft < currentItemCount)
			{
				swapIndex = childIndexLeft;

				if (childIndexRight < currentItemCount)
				{
					if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0 )
					{
						swapIndex = childIndexRight;

					}
				}
			}

		}
	}

}



public interface IHeapItem<T> : IComparable<T>
{
	int HeapIndex{get; set; }
		/**
		HeapIndex is simply the index in the array.  But it needs to be calculated specially because a heap IS in order...but the order does not correlate exactly to the index of the list. 
		For example, using the YT video above...we know that items at index 1 and 2 will be < than index 0 (meaning index 0 is always the smalled value of the entire heap...), but we don't know that
		index 2 will be less than index 3.  We only know that item at index 1 will be less than index 3, because index 1 is the parent of index 3 and 4.   Index 2 is the parent of 5 and 6, so we only know 
		that it will be less than those.  
		**/
}
