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

    [Range(0.1f, 2f)]  public float timeBetweenEachSpawn = 0.3f;

    //////////////////////////////State
    public bool isCurrentWaveOnPlayArea = false;


    //////////////////////////////Cached Component References
    KnockDownTargetSpawner spawner;
    public List<GameObject> objectsInCurrentWave;


    void Start()
    {
        spawner = parentMiniGame.GetComponentInChildren<KnockDownTargetSpawner>();
        StartCoroutine(SpawnWave(minWaveAmount, maxWaveAmount));
    }


    public IEnumerator SpawnWave(int min, int max)
    {
        yield return new WaitForSeconds(Random.Range(3,6)); //this wait is so that waves don't come immediately after another.  This timer starts when the last target from previous wave is gone
        if (isCurrentWaveOnPlayArea)
        {
            yield return new WaitForSeconds(0.1f);
        }

        isCurrentWaveOnPlayArea = true;
        int targetsInWave = Random.Range(min, max+1);
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
        StartCoroutine(SpawnWave(minWaveAmount, maxWaveAmount));
    }
    


        


        
    [ContextMenu("SpawnWaveFromEditor() testing only")] 
    public void SpawnWaveFromEditor()
    {
        StartCoroutine(SpawnWave(minWaveAmount, maxWaveAmount));
    }

}
