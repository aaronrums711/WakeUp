using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PassageEnd : MonoBehaviour
{
	/*****************
	CreateDate: 	9-14-2022
	Functionality:	this represents one end of a passage.  
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	public bool snap = false;
	
	//////////////////////////////Cached Component References
		[SerializeField] public _Grid grid;

	
	
    void Start()
    {
		grid = FindObjectOfType<_Grid>();
		AssignSelfToParent();
    }

    void Update()
    {
		SnapPosition();
    }

	public void AssignSelfToParent()
	{
		if (this.transform.parent.TryGetComponent<PassageV2>(out PassageV2 parentPassage))
		{
			parentPassage.ends.Add(this);
		}
		else
		{
			Debug.LogError("this PassageEnd doesn't have a Passage parent!");
		}
	}

	public void SnapPosition()
	{
		if (snap == true)
		{
			if (grid.nodeGrid == null) {grid.CreateGrid();}
			Vector3 newPos = grid.NodeFromWorldPoint(this.transform.position).worldPosition;
			this.transform.position = newPos;
		}
	}

	void OnDestroy()
	{
		if (this.transform.parent.TryGetComponent<PassageV2>(out PassageV2 parentPassage))
		{
			parentPassage.ends.Remove(this);
		}

	}
}
