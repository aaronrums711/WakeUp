using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is mostly just responsible for respawning targets.  There needs to be enough targets at a time so that the player 
//always has something to aim at.  

public class PoolBilliardsManager : MiniGameElement
{


    //////////////////////////////Config
    public float targetLocationPadding = 1f;

    //////////////////////////////State

    //////////////////////////////Cached Component References
    [SerializeField] private int targetsInPlay;
    [SerializeField] private int maxTargetsAllowed = 4;
    public GameObject targetBallPrefab;
    public Transform targetBallParent;
    
    void Start()
    {
        GetTargetCount();
        // AttemptToSpawnTargets();
    }

    void Update()
    {
        
    }

    void GetTargetCount()
    {
        targetsInPlay = parentMiniGame.transform.GetComponentsInChildren<BilliardsTarget>().Length;
    }

    //UPON RETURN: create a method to spawn target balls periodically.  
    //delete the old layers and create new ones.  I think I like the idea of target balls being able to knock out other target balls, 
    //so that layer should be able to collide with itself. 

    [ContextMenu("attempt to spawn target")]
    private void AttemptToSpawnTargets()
    {
        if (targetsInPlay == maxTargetsAllowed) {return;}

        float attempts = maxTargetsAllowed-targetsInPlay;
        float spawnChance= 1f * (attempts/maxTargetsAllowed);  //if there's Max-1 targets in play, there will only be a 20% chance of spawning another
        float spawnChanceReduction = 0.1f;
        for (float i=attempts; i>0; i--)
        {
            float chance = UnityEngine.Random.Range(0f, 1f);
            print("spawn threshold: " + spawnChance);
            print("spawn attempt: " + chance);
            if (chance < spawnChance)
            {
                SpawnTarget();
            }
            spawnChance -= spawnChanceReduction;
        }
    }

    private void SpawnTarget()
    {
        Instantiate(targetBallPrefab, SearchForLocation(), Quaternion.identity, targetBallParent);
        GetTargetCount();
    }


    private Vector3 SearchForLocation()
    {
        float maxY = 0;
        float maxX=  0;
        float minY = 0;
        float minX = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            float testY =  this.transform.GetChild(i).transform.position.y;
            float testX = this.transform.GetChild(i).transform.position.x;
            if (testY > maxY) {maxY = testY-targetLocationPadding;}
            else if(testY< minY){minY = testY+targetLocationPadding;}

            if (testX > maxX) {maxX = testX-targetLocationPadding;}
            else if(testX < minX) {minX = testX+targetLocationPadding;}
        }

        Vector3 targetBallLocation = new Vector3(Random.Range(minY, maxY), Random.Range(minX, maxX));
        // print("maxY: " + maxY);
        // print("maxX: " + maxX);
        // print("minY: " + minY);
        // print("minX: " + minX);
        
        //UPON RETURN: need to do another check here to make sure the final position isn't too close to the cueBall. 
        //if it is, run SearchForLocation() again.  
        //not sure if it's necessary for the targert balls not to be touching eachother...probably is. 
        //could just get the distance between the targetBallLocation and all the other target balls AND the cue ball. 
        //if any of those distances is < some threshold, then it's too close and we run it again. 

        return targetBallLocation;

    }
}
