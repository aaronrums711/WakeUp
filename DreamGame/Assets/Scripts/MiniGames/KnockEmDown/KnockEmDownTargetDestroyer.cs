using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockEmDownTargetDestroyer : MiniGameElement
{
    //////////////////////////////Config
     [Tooltip("only used for the brief time that the target grows before it shrinks and disappears")]  
    public float growRate;
    public float growEffectLength = 0.5f;
    public float endEffectShrinkRate = 10;

    //////////////////////////////State

    //////////////////////////////Cached Component References
    private KnockEmDownWaveManager waveManager;

    void Start()
    {
        waveManager = parentMiniGame.GetComponentInChildren<KnockEmDownWaveManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(parentMiniGame.keyForThisGame))
        {
            if (waveManager.objectsInCurrentWave.Count <= 0)
            {
                ///negative progression
            }
            else if(waveManager.objectsInCurrentWave.Count >= 0)
            {
                StartCoroutine(GrowAndShrink(waveManager.objectsInCurrentWave[0].gameObject.transform, growRate, waveManager.objectsInCurrentWave[0].GetComponent<KnockEmDownTarget>()));
            }
        }
    }

   public IEnumerator GrowAndShrink(Transform trans, float rate, KnockEmDownTarget target) 
	{
        float endTime= Time.time + growEffectLength;

		Vector3 changeVector = new Vector3(rate, rate, rate);
		while(Time.time < endTime)
		{
			trans.localScale += changeVector;
			yield return null;
		}
        target.rate = endEffectShrinkRate;  //basically this makes the ball shrink very fast, then dissappear
	}


}
