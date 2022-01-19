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
	public GameObject test;
	public float speed;
	public float circleRadius= 12;
	public float spawnRate;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
    void Start()
    {
        StartCoroutine(RadiusTest());
    }

    void Update()
    {
        
    }

	private void Launch(GameObject go)
	{
		return;
	}

	public IEnumerator RadiusTest()
	{
		while(1==1)
		{
			Vector2 spawnPoint = Random.insideUnitCircle.normalized * circleRadius;
			Instantiate(test, spawnPoint, Quaternion.identity);
			yield return new WaitForSeconds(spawnRate);
		}

	}

}
