using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BilliardsTarget : MiniGameElement
{
    
    //////////////////////////////Config
    public float totalTime;
    [Range(0.001f, 0.9f)] public float rate;


    //////////////////////////////State
    public bool isHit = false;
    private float initialScale;  //used in grow method

    //////////////////////////////Cached Component References
    private Transform thisTransform;

    void Start()
    {
        initialScale = this.transform.localScale.x;
        thisTransform = GetComponent<Transform>();
        this.transform.localScale = Vector3.zero;
        StartCoroutine(Grow(this.transform, rate));
    }


    void OnCollisionEnter2D(Collision2D other)    
    {
        if (other.gameObject.TryGetComponent(out CueBall ball)    )
        {
            if (!isHit) StartCoroutine(Shrink(thisTransform, rate));
            isHit = true;
        }
        isHit = false;  //added after some debugging.  if a target ball was spawned right next the cue ball, it's isHit would be true, but for some reason it wouldn't dissappear. 
                        //so it would be stuck on the play area.  this is a cheap way around that. 
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
        Destroy(this.gameObject);
    }

       public IEnumerator Grow(Transform trans, float rate) 
    {
        Vector3 changeVector = new Vector3(rate, rate, rate);
        while(trans.localScale.x <= initialScale)
        {
            trans.localScale += changeVector;
            yield return null;
        }
        //in case it's not perfect, at the end of the loops just set scale to initial scale
        trans.localScale = new Vector3(initialScale, initialScale, initialScale);
       ;
    }



}
