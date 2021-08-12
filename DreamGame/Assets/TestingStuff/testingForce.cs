using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingForce : MonoBehaviour
{
	/*****************
	CreateDate: 
	Functionality:
	******************/
	
	//////////////////////////////Config
    public float launchSpeed;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	private Rigidbody2D rb;
	
	void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
	
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            rb.AddForce(transform.up * launchSpeed, ForceMode2D.Impulse);
        }
    }
}
