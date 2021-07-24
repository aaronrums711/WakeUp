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

    [Range(0.1f, 2f)]  public float timeBetweenEachSpawn;
    //////////////////////////////State
    public bool isCurrentWaveOnPlayArea = false;

    //////////////////////////////Cached Component References
    KnockDownTargetSpawner spawner;



    void Start()
    {
        spawner = parentMiniGame.GetComponentInChildren<KnockDownTargetSpawner>();
    }


    void Update()
    {
        
    }

    public IEnumerator SpawnWave(int min, int max)
    {
        if (isCurrentWaveOnPlayArea)
        {
            yield return new WaitForSeconds(0.1f);
        }

        isCurrentWaveOnPlayArea = true;
        int targetsInWave = Random.Range(min, max+1);
        for(int i=0; i < targetsInWave; i++)
        {
            spawner.InstantiateTarget();
            yield return new WaitForSeconds(timeBetweenEachSpawn);
        }
        StartCoroutine(MonitorCurrentWave());
    }
    
    [ContextMenu("SpawnWaveFromEditor()")]
    public void SpawnWaveFromEditor()
    {
        StartCoroutine(SpawnWave(minWaveAmount, maxWaveAmount));
    }

    public IEnumerator MonitorCurrentWave()
    {
        while (spawner.targetParent.childCount > 0 )
        {
            yield return new WaitForSeconds(0.1f);
        }
        isCurrentWaveOnPlayArea = false;
    }

}
