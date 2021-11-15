using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockEmDownTarget : MiniGameElement
{
    //////////////////////////////Config
    [Tooltip("should be a very small number between 0.0001 and 0.0007 or so ")]  
    public float initialShrinkRate; //VGIU from the prefab
    public float growEffectLength = 0.5f;
    public float endEffectRate = 0.01f;

    //////////////////////////////State
    [HideInInspector]
    public Coroutine initialCoroutine;

    //////////////////////////////Cached Component References
    private KnockEmDownWaveManager waveManager;

    void Start()
    {
        initialShrinkRate *= parentMiniGame.difficultyParams.scaleUpMultiplier;
        waveManager = parentMiniGame.GetComponentInChildren<KnockEmDownWaveManager>();
        initialCoroutine = StartCoroutine(Shrink(this.transform, initialShrinkRate));
    }

    void Update()
    {
        
    }



    public IEnumerator Shrink(Transform trans, float rate) 
    {
        Vector3 changeVector = new Vector3(rate, rate, rate);
        while(trans.localScale.x >=0.01 && parentMiniGame.isActive)
        {   //this loop will terminate for 1 of 2 reasons.  if the scale has reduced enough, then 
            //the target will be removed and destroyed.
            //if the isActive flag has been set to false, none of that will happen. 
            trans.localScale -= changeVector;
            yield return null;
        }
        
        if(parentMiniGame.isActive)
        {
            //in case it's not perfect, at the end of the loops just set scale to 0
            trans.localScale = Vector3.zero;
            waveManager.objectsInCurrentWave.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
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
        float scale = this.transform.localScale.x * 2;  //multiplying by two will roughly be 1 at the start of a targets life, because their X scale is about .5  
        StartCoroutine(DestroyEffect(this.transform,endEffectRate, this ));
        return scale;
    }

    //only creating this becasue I'm getting a weird "Coroutine Continue Failure" message when I try to stop them from the KnockeEmDownStartStopper script in the slow down method
    public void StopInitialCoroutine()
    {
        StopCoroutine(this.initialCoroutine);
    }

    //called from StartStopper as well
    public void RestartInitialCoroutine()
    {
        initialCoroutine = StartCoroutine(Shrink(this.transform, initialShrinkRate));
    }

}
