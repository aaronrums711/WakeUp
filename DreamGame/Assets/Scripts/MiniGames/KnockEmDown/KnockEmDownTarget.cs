using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockEmDownTarget : MiniGameElement
{
    //////////////////////////////Config
    [Tooltip("should be a very small number between 0.0001 and 0.0007 or so ")]  
    public float initialShrinkRate = 0.00035f;
    public float growEffectLength = 0.5f;
    public float endEffectRate = 0.01f;

    //////////////////////////////State
    [HideInInspector]
    public IEnumerator initialCoroutine;

    //////////////////////////////Cached Component References
    private KnockEmDownWaveManager waveManager;

    void Start()
    {
        waveManager = parentMiniGame.GetComponentInChildren<KnockEmDownWaveManager>();
        initialCoroutine = Shrink(this.transform, initialShrinkRate);
        StartCoroutine(initialCoroutine);
    }

    void Update()
    {
        
    }



    public IEnumerator Shrink(Transform trans, float rate) 
    {
        Vector3 changeVector = new Vector3(rate, rate, rate);
        while(trans.localScale.x >=0.01)
        {
            trans.localScale -= changeVector;
            yield return null;
        }
        //in case it's not perfect, at the end of the loops just set scale to 0
        trans.localScale = Vector3.zero;
        waveManager.objectsInCurrentWave.Remove(this.gameObject);
        Destroy(this.gameObject);
    }


    public IEnumerator DestroyEffect(Transform trans, float rate, KnockEmDownTarget target) 
	{
        waveManager.objectsInCurrentWave.Remove(this.gameObject);
        StopCoroutine(initialCoroutine);
        float endTime= Time.time + growEffectLength;

		Vector3 changeVector = new Vector3(rate, rate, rate);
		while(Time.time < endTime)
		{
			trans.localScale += changeVector;
			yield return null;
		}

        while(trans.localScale.x >=0.01)
        {
            changeVector = new Vector3(rate, rate, rate);
            trans.localScale -= changeVector;
            yield return null;
        }
        //in case it's not perfect, at the end of the loops just set scale to 0
        trans.localScale = Vector3.zero;
        Destroy(this.gameObject);
	}

    //to be called more simply from the TargetDestroyerClass. 
    public float CallDestroyEffect()
    {
        float scale = this.transform.localScale.x + 1;  //the +1 makes the score multiplcation make more sense
        StartCoroutine(DestroyEffect(this.transform,endEffectRate, this ));
        return scale;
    }

}
