using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppedTargetSpawner : MiniGameElement
{
	/*****************
	CreateDate:  8/4/21
	Functionality: spawns targets from different launchers below the play area.  It also applies force to the targets. 
    Notes:  Make sure all the launchers start with 0,0,0 for rotation!
            Also make sure this object is on the "LaunchPoints" game object - it is found by string reference from other scripts
	******************/
	
	//////////////////////////////Config
    private float launchSpeed;
    private Vector3 targetRotation;
    public float launchSpeedMin;
    public float launchSpeedMax;
	
	//////////////////////////////State
    public List<ChoppedTarget> allTargets;
	
	//////////////////////////////Cached Component References
    private Transform[] launchPoints;
    public GameObject choppedTargetPrefab;
    public Transform targetParent;
    
	
	
    void Start()
    {
        launchPoints = new Transform[GameObject.Find("LaunchPoints").GetComponent<Transform>().childCount];
        targetParent = GameObject.Find("AllTargets").GetComponent<Transform>();
        for (int i =0; i< GameObject.Find("LaunchPoints").GetComponent<Transform>().childCount; i++)
        {
            launchPoints[i] = GameObject.Find("LaunchPoints").GetComponent<Transform>().GetChild(i);
        }
    }

    void Update()
    {
        
    }

    [ContextMenu("spawn target")]
    public void SpawnTarget()
    {
        int launchPointIndex = Random.Range(0, launchPoints.Length);
        if (launchPointIndex == 0)
        {
             targetRotation = new Vector3 (0,0, Random.Range(-10,-30));
        }
        else if (launchPointIndex == 1)
        {
             targetRotation = new Vector3 (0,0, Random.Range(-25,25));
        }
        else if (launchPointIndex == 2)
        {
             targetRotation = new Vector3 (0,0, Random.Range(10,30));
        }

        launchSpeed = Random.Range(launchSpeedMin,launchSpeedMax);

        GameObject newObj = Instantiate(choppedTargetPrefab, launchPoints[launchPointIndex].position, Quaternion.Euler(targetRotation),targetParent);
        allTargets.Add(newObj.GetComponent<ChoppedTarget>());
        newObj.GetComponent<Rigidbody2D>().AddRelativeForce(transform.up * launchSpeed, ForceMode2D.Impulse);
    }


}
