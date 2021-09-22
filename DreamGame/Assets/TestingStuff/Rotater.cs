using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
	/*****************
	CreateDate: 
	Functionality:
	******************/
	
	//////////////////////////////Config
    public bool isForward;
    public bool isRight;
    public bool isUp;
    public bool isLocalSpace;

    public Vector3 rotDirection;
    public float rotSpeed; 

    public Transform lookAtTarget;
    
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
	void Awake()
    {
        
    }
	
    void Start()
    {
        
    }

    void Update()
    {
        if (isForward)
        {
            rotDirection = Vector3.forward;
        }
        if (isRight)
        {
            rotDirection = Vector3.right;
        }        
        if (isUp)
        {
            rotDirection = Vector3.up;
        }

        // if (isLocalSpace)
        // {
        //     this.transform.Rotate(rotDirection, Space.Self);
        // }
        // else
        // {
        //     this.transform.Rotate(rotDirection, Space.World);
        // }
        // this.transform.LookAt(lookAtTarget);
        this.transform.forward = lookAtTarget.position - this.transform.position;
        // print(lookAtTarget.position - this.transform.position);
        // print(this.transform.right);
    }
}
