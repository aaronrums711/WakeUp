using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PassageOpening : MonoBehaviour
{
	/*****************
	CreateDate: 	3/12/2022
	Functionality:	this class creates openings in walls as it slides up and down the forward direction of a passage
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	[HideInInspector]public bool left;
	[HideInInspector]public bool right;
	public int openingWidth;
	public Passage attachedPassage;
	[Range(0f, 1f)] public float posAlongPath;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
	void Awake()
    {
        
    }
	
    void Start()
    {
        SetPosition();
    }

    void Update()
    {
		SetPosition();
		CreateOpening();
    }

	public void CreateOpening()
	{
		if (Physics.Raycast(this.transform.position,  this.transform.right,out RaycastHit hitInfo, 10f))
		{
			print("Raycast hit: " + hitInfo.transform.gameObject.name);
			if (hitInfo.transform.TryGetComponent<PassageWall> (out PassageWall hitWall))
			{
				print("wall destroyed");
				DestroyImmediate(hitWall.gameObject);
			}
		}
		else
		{
			Debug.LogWarning("a PassageOpening object didn't hit any wall on it's raycast");
		}
	}

	private void SetPosition()
	{
		if (attachedPassage != null)
		{
			List<Vector3> ends = attachedPassage.Ends;
			this.transform.position = Vector3.Lerp(ends[0], ends[1], posAlongPath);
		}
	}
}
