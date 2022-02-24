using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
	public static event Action<MovementOrientation>  ChangeOrientation;
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

			ChangeOrientation?.Invoke(currentMO.nextMovementOrientation);
			currentMO = currentMO.nextMovementOrientation;
			/**
			rules:  
			if nextMOOrder > currentMOOrder   OR nextMOOrder = 1     --the OR is for if the currentMOOrder = 4, only then will the next one be 1
				rotate -90
			**/

		}		

	}


}
