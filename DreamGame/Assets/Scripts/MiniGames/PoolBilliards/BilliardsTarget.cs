using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BilliardsTarget : MonoBehaviour
{
    
    //////////////////////////////Config
    public float totalTime;
    
    [Range(0.1f, 0.9f)] public float growShrinkSplit;

    [Range(0.001f, 0.9f)] public float rate;


    //////////////////////////////State
    private bool isHit = false;

    //////////////////////////////Cached Component References
    private Transform thisTransform;

    void Start()
    {
        thisTransform = GetComponent<Transform>();
    }


    void OnCollisionEnter2D(Collision2D other)    
    {
        if (other.gameObject.TryGetComponent(out CueBall ball)    )
        {
            if (!isHit) StartCoroutine(Shrink(thisTransform, rate));
            isHit = true;
        }
    }

    public IEnumerator Shrink(Transform trans, float rate) //growShrinkSplit should be between 0 and 1, and indicates the percent of time that object will grow before it shrinks to nothing
    {

        Vector3 changeVector = new Vector3(rate, rate, rate);
        while(trans.localScale.x >=0.01)
        {
            trans.localScale -= changeVector;
            yield return null;
        }
        //in case it's not perfect, at the end of the loops just set scale to 0
        trans.localScale = Vector3.zero;

    }
}
