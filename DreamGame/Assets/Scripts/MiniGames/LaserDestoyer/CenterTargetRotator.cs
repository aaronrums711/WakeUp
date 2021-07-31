using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTargetRotator : MiniGameElement
{
    //////////////////////////////Config
    public Vector3 rot;
    public Vector3 rotPlusRandom;
    //////////////////////////////State
    //////////////////////////////Cached Component References


    void Start()
    {
        StartCoroutine(ContinuallyRotate());
    }

    public IEnumerator RotateChunk()
    {   
        float rotationAmount = 45f * (Random.Range(1,5));
        float targetRotation = this.transform.rotation.eulerAngles.z + rotationAmount > 360 ? this.transform.rotation.eulerAngles.z + rotationAmount -360 : this.transform.rotation.eulerAngles.z + rotationAmount;
        int OneOrZero = Random.Range(0,2);
        int NegOrPosOne = OneOrZero == 1 ? 1 : -1;
        rotPlusRandom = rot * Random.Range(0.5f, 3f);
        while (Mathf.Abs(this.transform.rotation.eulerAngles.z - targetRotation) > 2)
        {
            this.transform.Rotate(rotPlusRandom *Time.deltaTime ) ; //* NegOrPosOne)
            yield return null;
        }
        this.transform.rotation = Quaternion.Euler(0,0,targetRotation);
    }

    
    public IEnumerator ContinuallyRotate()
    {
        yield return StartCoroutine(RotateChunk());
        yield return new WaitForSeconds(Random.Range(2,4));
        StartCoroutine(ContinuallyRotate());
    }

}
