using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayTest : MonoBehaviour
{
	/*****************
	CreateDate: 
	Functionality:
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config

	public int arraySize;
	public int[] array;

	public int arrayStart;
	public int arrayEnd;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
	void Awake()
    {
        array = new int[arraySize];

		for (int i = arrayStart; i<=arrayEnd; i++)
		{
			array[i] = Random.Range(1,10);
		}


		for (int i = 0; i<array.Length; i++)
		{
			print("array index: " + i + " is value " + array[i]);
		}
		print("Array length property" + array.Length);
    }
	
}
