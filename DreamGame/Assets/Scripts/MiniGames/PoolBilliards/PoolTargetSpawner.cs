using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//this class is mostly just responsible for respawning targets.  There needs to be enough targets at a time so that the player 
//always has something to aim at.  

public class PoolTargetSpawner : MiniGameElement
{


    //////////////////////////////Config
    public float targetLocationPadding = 1f;
    // public Vector3 finalTargetBallLocation;

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
        InvokeRepeating("GetTargetCount", 0.5f, 0.5f);
        InvokeRepeating("AttemptToSpawnTargets", 0.1f, 10f);
    }

    void Update()
    {
        
    }

    void GetTargetCount()
    {
        targetsInPlay = parentMiniGame.transform.GetComponentsInChildren<BilliardsTarget>().Length;
    }

    [ContextMenu("attempt to spawn target")]
    public void AttemptToSpawnTargets()
    {
        GetTargetCount();
        int targetsToSpawn = 0;
        if (targetsInPlay >= maxTargetsAllowed) {return;}

        float attempts = maxTargetsAllowed-targetsInPlay;
        float spawnChance= 1f * (attempts/maxTargetsAllowed);  //if there's Max-1 targets in play, there will only be one attempt to spawn, and it will have a 1/MaxTargetsAllowed chance success
        float spawnChanceReduction = 0.1f* parentMiniGame.difficultyParams.scaleUpMultiplier;  //this will make it slightly more likely to spawn more targets on easier difficulties
        for (float i=attempts; i>0; i--)
        {
            float chance = UnityEngine.Random.Range(0f, 1f);
            if (chance < spawnChance)
            {
                targetsToSpawn++;
            }
            spawnChance -= spawnChanceReduction;
        }
        StartCoroutine(SpawnTargets(targetsToSpawn));
    }
 
    private IEnumerator SpawnTargets(int iterations)
    {
        for(int i =0; i< iterations; i++)
        {

            List<Transform> barriers = new List<Transform>();
            foreach (GameObject go in parentMiniGame.playAreaBarriers)
            {
                barriers.Add(go.transform);;
            }

            List<Transform> objectsToAvoid = new List<Transform>();
            foreach (BilliardsTarget target in targetBallParent.GetComponentsInChildren<BilliardsTarget>())
            {
                objectsToAvoid.Add(target.transform);
            }
            objectsToAvoid.Add(parentMiniGame.GetComponentInChildren<CueBall>().transform);



            // Vector3 targetDestination = SearchForLocation();
            Vector3 targetDestination = GetRandomPoint(barriers, targetLocationPadding, objectsToAvoid);
            if (targetDestination.x == 1000f)
            {
                print("the spawn method failed to return a valid location");
                // continue;
            }
            else 
            {
                Instantiate(targetBallPrefab, targetDestination, Quaternion.identity, targetBallParent);
                GetTargetCount();
            }
            
            yield return new WaitForSeconds(0.5f);
        }
    }


    // private Vector3 SearchForLocation()
    // {
    //     float maxY = 0;
    //     float maxX=  0;
    //     float minY = 0;
    //     float minX = 0;
    //     for (int i = 0; i < transform.childCount; i++)
    //     {
    //         float testY =  this.transform.GetChild(i).transform.position.y;
    //         float testX = this.transform.GetChild(i).transform.position.x;
    //         if (testY > maxY) {maxY = testY-targetLocationPadding;}
    //         else if(testY< minY){minY = testY+targetLocationPadding;}

    //         if (testX > maxX) {maxX = testX-targetLocationPadding;}
    //         else if(testX < minX) {minX = testX+targetLocationPadding;}
    //     }


    //     Vector3 finalTargetBallLocation = new Vector3(1000,1000,1000);
    //     List<float> distances = new List<float>();
    //     int iterations = 0;
    //     while(finalTargetBallLocation.x == 1000)
    //     {
    //         distances = new List<float>();
    //         Vector3 targetBallLocation = new Vector3(Random.Range(minY, maxY), Random.Range(minX, maxX));
            
    //         foreach (BilliardsTarget target in parentMiniGame.transform.GetComponentsInChildren<BilliardsTarget>())
    //         {   ///get other target ball distances
    //             distances.Add(Vector3.Distance(targetBallLocation, target.transform.position));
    //         }
    //         //get cueball distance
    //         distances.Add(Vector3.Distance(targetBallLocation, parentMiniGame.transform.GetComponentInChildren<CueBall>().transform.position));

    //         if (distances.Min() >targetLocationPadding)
    //         {
    //             finalTargetBallLocation = targetBallLocation;
    //         }
    //         else if( iterations >=10)
    //         {
    //             break;
    //         }
    //         iterations++;
    //     }
    //     return finalTargetBallLocation;
    // }



}
