using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;


public class Schedule : MonoBehaviour
{
	/*****************
	CreateDate: 	2/11/22
	Functionality:	an instance of this object should store a list of    Datetime and Transform key value pairs
	Notes:
	Dependencies:
	******************/
	
	public int itemsInSchedule;
	
	public OrderedDictionary schedule = new OrderedDictionary();


	//these lists need to be made so that new Schedule objects can easily be made in the editor. Dictionaries (and OrderedDictionaries) don't show in the inspector. 
	public List<Transform> destinations;
	public List<Vector3>  times;


	
}
