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
	[Range(0.001f, 1f)]
	[SerializeField] private float movementSpeed;
	[SerializeField] private MovementOrientation currentMO;
	public static event Action<MovementOrientation>  ChangeOrientation;

	[SerializeField] private KeyCode rotateCounterClockwiseKey;
	[SerializeField] private KeyCode rotateClockwiseKey;



	//////////////////////////////State
	[SerializeField] private PassageV2 currentPassage;
	private Vector3 nearMoveTarget;
	private Vector3 farMoveTarget;

	[Range(0f, 1f)]
	private float movementLerp;
		


	//////////////////////////////Cached Component References
	
	
	void Awake()
    {
        
    }
	
    void Start()
    {
        if (currentPassage != null)
		{
			this.transform.position = Vector3.Lerp(currentPassage.endPositions[0], currentPassage.endPositions[1], 0.1f);
		}
		else
		{
			Debug.LogWarning ("there is no Passage to move on currently assigned to the player");
		}
		SetMoveTargets();
    }

    void Update()
    {
		SwitchOrientation();
		MoveAlongPassage(currentPassage);
    }

	//this will probably get deprecated.  movement is now based on the current PassageV2 object that the player is on
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

	private void MoveAlongPassage(PassageV2 passage)
	{
		//player distance to end/total distance between two ends
		float lerp = Vector3.Distance(this.transform.position, farMoveTarget)/  Vector3.Distance(nearMoveTarget, farMoveTarget);

		if (Input.GetKey(KeyCode.D))
		{
			// movementLerp +=
		}
		else if (Input.GetKey(KeyCode.A))
		{
			this.transform.Translate(currentMO.leftWorldDirection * movementSpeed * Time.deltaTime, Space.World);
		}

		this.transform.position = Vector3.Lerp(this.transform.position, farMoveTarget, lerp);
	}


	public void SwitchOrientation()
	{
		if (Input.GetKeyDown(rotateCounterClockwiseKey))
		{
			ChangeOrientation?.Invoke(currentMO.nextMovementOrientation);
			currentMO = currentMO.nextMovementOrientation;
		}	
		else if (Input.GetKeyDown(rotateClockwiseKey))
		{
			ChangeOrientation?.Invoke(currentMO.previousMovementOrientation);
			currentMO = currentMO.previousMovementOrientation;
		}	
	}

	private void SetMoveTargets()
	{
		if (currentPassage != null)
		{
			nearMoveTarget = Utils.GetNearestVector(currentPassage.endPositions, this.transform.position);
			if (nearMoveTarget == currentPassage.endPositions[0])
			{
				farMoveTarget =  currentPassage.endPositions[1];
			}
			else
			{
				farMoveTarget =  currentPassage.endPositions[0];
			}
		}
		else
		{
			Debug.LogError("the currentPassage object isn't assigned, cannot set the player movement targets");
		}

	}


}
