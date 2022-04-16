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
	[SerializeField] float openingFindRaycastLength = 2;



	//////////////////////////////State
	[SerializeField] private PassageV2 currentPassage;
	private Vector3 nearMoveTarget;
	private Vector3 farMoveTarget;
	private bool isSetOnPassage; 
	[SerializeField] private bool forwardOpeningAvailable;
	[SerializeField] private bool rearOpeningAvailable;


	[Range(0f, 1f)]
	[SerializeField]private float movementLerp;
		


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
		SearchForOpenings();
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
		if (!isSetOnPassage)
		{
			movementLerp = Vector3.Distance(this.transform.position, farMoveTarget)/  Vector3.Distance(nearMoveTarget, farMoveTarget);
			this.transform.position = Vector3.Lerp(nearMoveTarget, farMoveTarget, movementLerp);
			isSetOnPassage = true;
			print("initial player position set");
		}
		//player distance to end/total distance between two ends

		if (Input.GetKey(KeyCode.D))
		{
			movementLerp += movementSpeed * Time.deltaTime;
			this.transform.position = Vector3.Lerp(nearMoveTarget, farMoveTarget, movementLerp);
		}
		else if (Input.GetKey(KeyCode.A))
		{
			movementLerp -= movementSpeed * Time.deltaTime;
			this.transform.position = Vector3.Lerp(nearMoveTarget, farMoveTarget, movementLerp);
		}

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
				farMoveTarget =  currentPassage.endPositions[1] + new Vector3(0,1,0);
			}
			else
			{
				farMoveTarget =  currentPassage.endPositions[0] + new Vector3(0,1,0);
			}
			nearMoveTarget += new Vector3(0,1,0);
			print("near move target:  " + nearMoveTarget);
			print("far move target:  " + farMoveTarget);

		}
		else
		{
			Debug.LogError("the currentPassage object isn't assigned, cannot set the player movement targets");
		}

	}



	/**
	need to raycast in forwards and backwards directions in Update() (maybe at some small interval, don't really need to do it EVERY frame).  If a wall is immediately hit, then there is no passage available. 
	if there isn't a wall hit, then there is a turn available in that direction. 
	Make sure to actually rotate the player object when it turns, so that the forward and backward directions will always find the openings
	**/
	void OnDrawGizmos()
	{
		// Debug.DrawRay(this.transform.position, this.transform.forward * openingFindRaycastLength, Color.red, 1f);
		// Debug.DrawRay(this.transform.position, this.transform.forward * -openingFindRaycastLength, Color.red, 1f);

	}


	private void SearchForOpenings()
	{
		
		//forward
		if (Physics.Raycast(this.transform.position,  this.transform.forward,out RaycastHit forwardHitInfo,openingFindRaycastLength ))
		{

			forwardOpeningAvailable = false;
			Debug.DrawRay(this.transform.position, this.transform.forward * openingFindRaycastLength, Color.red, 0.2f);
		}
		else
		{
			forwardOpeningAvailable = true;
			Debug.DrawRay(this.transform.position, this.transform.forward * openingFindRaycastLength, Color.green, 0.2f);
		}

		//rear
		if (Physics.Raycast(this.transform.position,  this.transform.forward * -1,out RaycastHit rearHitInfo, openingFindRaycastLength))
		{
			rearOpeningAvailable = false;
			Debug.DrawRay(this.transform.position, this.transform.forward * -openingFindRaycastLength, Color.red, 0.2f);

		}
		else
		{
			rearOpeningAvailable = true;
			Debug.DrawRay(this.transform.position, this.transform.forward * -openingFindRaycastLength, Color.green, 0.2f);
		}
	}



}
