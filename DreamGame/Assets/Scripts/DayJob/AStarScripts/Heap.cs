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

					the main rule of a heap is that each index has up to 2 children, and each parent must be less than each of it's children.  In reverse, each child must be greater than it's parent

					To put it another way - the main advantage of using this heap instead of just looping through all the nodes in openSet to find the lowest FCost, is
					that it is basically self contained and self-sorting.  

					Any entity that needs to grab the item with the highest priority (which will be defined by the CompareTo method from the IComparable interface,
					in our case the item with the LOWEST Fcost is the HIGHEST priority) can just call Heap.RemoveFirst().   Then the heap will re-organize itself to replace that item,
					and will adjust the positions of any other items as needed.  
	
	
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

	//
	void Swap (T itemA, T itemB)
	{
		items[itemA.HeapIndex] = itemB; //at the heapIndex of itemA, place itemB
		items[itemB.HeapIndex] = itemA;	//at the heapIndex of itemB, place itemA

		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex; 	//give itemA the heapIndex of itemB, since that's where it is now
		itemB.HeapIndex = itemAIndex;		//give itemB the heapIndex of itemA
	}

	public T RemoveFirst()
	{
		T firstItem = items[0]; 	//save a ref of the first item
		currentItemCount--;
		items[0] = items[currentItemCount];	//set last item as the first in the heap
		items[0].HeapIndex = 0;				//set the heapIndex of that item to match it's new location in the list
		SortDown(items[0]);
		return firstItem;

	}

	void SortDown(T item) {
		while (true) {
			int childIndexLeft = item.HeapIndex * 2 + 1;
			int childIndexRight = item.HeapIndex * 2 + 2;
			int swapIndex = 0;

			if (childIndexLeft < currentItemCount) {
				swapIndex = childIndexLeft;

				if (childIndexRight < currentItemCount) {
					if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
						swapIndex = childIndexRight;
					}
				}

				if (item.CompareTo(items[swapIndex]) < 0) {
					Swap (item,items[swapIndex]);
				}
				else {
					return;
				}

			}
			else {
				return;
			}

		}
	}
	public int Count
	{
		get{return currentItemCount;}
	}

	public bool Contains(T item)
	{
		return Equals(items[item.HeapIndex], item);
	}

	public void UpdateItem(T item)
	{
		SortUp(item);
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

		If an item is inserted and put at the last spot, lets say index 14, the only potential indexes it can have if it needs to get sorted up are 6, 2 or 0.  
		**/
}
