using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCenter : MonoBehaviour
{
	/*****************
	CreateDate: 
	Functionality:
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	public Transform levelCenter;
	public Transform test;
	
	
	void Awake()
    {
        
    }
	
    void Start()
    {
        
    }

    void Update()
    {
        
    }
	[ContextMenu("VectorTest()")]
	public void VectorTest()
	{
		Vector3 direction = (levelCenter.transform.position - test.transform.position).normalized;
		print("levelCenter - test : " + direction);
	}

}
