using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockEmDownTarget : MonoBehaviour
{
    //////////////////////////////Config
    [Range(0.0001f, 0.005f)] public float rate;

    //////////////////////////////State

    //////////////////////////////Cached Component References


    void Start()
    {
        StartCoroutine(Shrink(this.transform, rate));
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
        Destroy(this.gameObject);
    }
}
