using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppedTargetSpawner : MiniGameElement
{
	/*****************
	CreateDate:  8/4/21
	Functionality: spawns targets from different launchers below the play area.  It also applies force
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
    public Transform[] launchPoints;
    public GameObject choppedTargetPrefab;
    private Transform targetParent;
	
	
    void Start()
    {
        launchPoints = GetComponentsInChildren<Transform>();
        targetParent = GameObject.Find("AllTargets").GetComponent<Transform>();
    }

    void Update()
    {
        
    }

    [ContextMenu("spawn target")]
    public void SpawnTarget()
    {
        int launchPointIndex = Random.Range(0, launchPoints.Length);
        Instantiate(choppedTargetPrefab, launchPoints[launchPointIndex].position, Quaternion.identity,targetParent );
    }

}
