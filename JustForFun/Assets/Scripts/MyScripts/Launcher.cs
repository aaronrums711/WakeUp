using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
	/*****************
	CreateDate: 	11/19/22
	Functionality:	this only launches projectiles from random points on a circle
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public GameObject thingToLaunch;
	public float launchSpeed;
	public float circleRadius= 12;
	public float spawnRate;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
    void Start()
    {
		StartCoroutine(LaunchRepeat(thingToLaunch));
    }

    void Update()
    {
        
    }


	public IEnumerator LaunchRepeat(GameObject objectToLaunch)
	{
		yield return new WaitForSeconds(3f);
		while(1==1)
		{
			Vector2 spawnPoint = Random.insideUnitCircle.normalized * circleRadius;
			GameObject newObj = Instantiate(objectToLaunch, spawnPoint, Quaternion.identity);
			Vector3 launchDirection = Vector3.zero - newObj.transform.position + new Vector3(Random.Range(0,5), Random.Range(0,5), Random.Range(0,5));
			newObj.GetComponent<Rigidbody>().AddForce(launchDirection * launchSpeed, ForceMode.Impulse);
			yield return new WaitForSeconds(spawnRate);
		}
	}

	///UPON RETURN:  MOVE THE DESTRUCTION CODE ONTO ItemRotator.cs


}
