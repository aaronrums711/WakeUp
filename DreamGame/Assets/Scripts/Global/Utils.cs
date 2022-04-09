using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils 
{
	/*****************
	CreateDate: 	3/21/2022
	Functionality:	contains a variety of misc convenience methods. 
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
	public static Transform SearchByNameFromParent(string searchName, Transform parent)
	{
		Transform target = parent;
		for (int i = 0; i < parent.childCount; i++)
		{
			if (parent.GetChild(i).name == searchName)
			{
				target = parent.GetChild(i).transform;
			}
		}
		if (target == parent)
		{
			Debug.LogWarning("a game object with the name " + searchName + " was not found as a child of " + parent.name  +".  The parent was returned instead.");
	
		}
		return target;
	}


	public static  Vector3 GetNearestVector(List<Vector3> vectors, Vector3 comparison)
	{
		float comparisonDistance = float.MaxValue;
		Vector3 returnVector = new Vector3();
		foreach (Vector3 v in vectors)
		{
			float distance = Vector3.Distance(v, comparison);
			if (distance < comparisonDistance )
			{
				comparisonDistance = distance;
				returnVector = v;
			}
		}
		return returnVector;
	}

	
	public static void DestroyAllChildren( Transform parent, bool destroyImmediate = false)
	{
		for (int i = parent.transform.childCount; i > 0; --i)
		{
			if (destroyImmediate)
			{
				GameObject.DestroyImmediate(parent.GetChild(0).gameObject);
			}
			else
			{
				GameObject.Destroy(parent.GetChild(i).gameObject);
			}
		}
	}

}
