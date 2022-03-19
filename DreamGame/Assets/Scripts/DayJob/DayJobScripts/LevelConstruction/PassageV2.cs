using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PassageV2 : MonoBehaviour
{
	/*****************
	CreateDate: 	3/18/22
	Functionality:	repesents one passage.  Draws a gizmo ray between the two PassageEnd parents
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public List<PassageEnd> ends;
	private List<Vector3> endPositions {get{ return GetEndPositions();}}
	[SerializeField] private GameObject endPrefab;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	void OnDrawGizmos()
	{
		if (ends.Count == 2)
		{
			Gizmos.DrawLine(ends[0].transform.position, ends[0].transform.position);
		}				
		
	}
	
	void Awake()
    {
        
    }
	
    void Start()
    {
        
    }

    void Update()
    {
        
    }

	private List<Vector3> GetEndPositions()
	{
		List<Vector3> endPositions = new List<Vector3>();
		for (int i = 0; i < ends.Count; i++)
		{
			endPositions.Add(ends[i].transform.position);
		}
		return endPositions;
	}

	private void SpawnEnd()
	{
		if (ends.Count < 2)
		{
			for (int i = ends.Count; i < 2; i++)
			{
				GameObject.Instantiate(endPrefab, this.transform.position, Quaternion.identity, this.transform);
			}
		}
	}
}
