using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	/*****************
	CreateDate: 	2/17/22
	Functionality:	main player class
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	[SerializeField] private int movementSpeed = 2;
	[SerializeField] private MovementOrientation currentMO;

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
        Move(currentMO);
		SwitchOrientation();
    }

	public void Move(MovementOrientation MO)
	{
		if (Input.GetKey(KeyCode.D))
		{
			this.transform.Translate(currentMO.rightWorldDirection * movementSpeed * Time.deltaTime, Space.World);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			this.transform.Translate(currentMO.leftWorldDirection * movementSpeed * Time.deltaTime, Space.World);
		}
	}


	public void SwitchOrientation()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			currentMO = currentMO.nextMovementOrientation;
		}		

	}


}
