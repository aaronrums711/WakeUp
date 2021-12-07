using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    /**
        this should create a wave every 8-10 seconds maybe.  The
        Then, each wave gets a semi-random number of targets spawned.  
    **/
public class KnockEmDownWaveManager : MiniGameElement
{
    //////////////////////////////Config
    public int minWaveAmount;
    public int maxWaveAmount;

    private float timeBetweenEachSpawn;

    public float minTimeBetweenEachSpawn;  //VGIU
    public float maxTimeBetweeenEachSpawn; //VGIU

    public int minTimeBetweenEachWave;
    public int maxTimeBetweenEachWave;

    //////////////////////////////State
    public bool isCurrentWaveOnPlayArea = false;


    //////////////////////////////Cached Component References
    KnockDownTargetSpawner spawner;
    public List<GameObject> objectsInCurrentWave;


    void Start()
    {
        spawner = parentMiniGame.GetComponentInChildren<KnockDownTargetSpawner>();
        if (parentMiniGame.difficultyParams == null)
        {Debug.LogError("it's null!!!");}
        StartCoroutine(SpawnWave(minWaveAmount, maxWaveAmount, minTimeBetweenEachWave, maxTimeBetweenEachWave));    
    }


    public IEnumerator SpawnWave(int minCount, int maxCount, int minTimeBetweenWave, int maxTimeBetweenWave)
    {
        timeBetweenEachSpawn = Random.Range(minTimeBetweenEachSpawn, maxTimeBetweeenEachSpawn) * parentMiniGame.difficultyParams.scaleDownMultiplier;

        while (isCurrentWaveOnPlayArea)
        {
            yield return new WaitForSeconds(0.1f);
            isCurrentWaveOnPlayArea = spawner.targetParent.childCount > 0;
        }

        yield return new WaitForSeconds(Random.Range(minTimeBetweenEachWave,maxTimeBetweenEachWave)); //even after isCurrentWaveOnPlayArea = false, wait a little bit

        isCurrentWaveOnPlayArea = true;
        int targetsInWave = Random.Range(minCount, maxCount+1);
        for(int i=0; i < targetsInWave; i++)
        {
            objectsInCurrentWave.Add(spawner.InstantiateTarget());
            yield return new WaitForSeconds(timeBetweenEachSpawn);
        }
        StartCoroutine(MonitorCurrentWave());
    }

    public IEnumerator MonitorCurrentWave()
    {
        while (spawner.targetParent.childCount > 0 )
        {
            yield return new WaitForSeconds(0.1f);
        }
        isCurrentWaveOnPlayArea = false;
        StartCoroutine(SpawnWave(minWaveAmount, maxWaveAmount,  minTimeBetweenEachWave, maxTimeBetweenEachWave));
    }
    
}
