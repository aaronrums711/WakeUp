using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTargetRotator : MiniGameElement
{
    //////////////////////////////Config
    public Vector3 baseRotSpeed;
    public Vector3 rotSpeedPlusRandom;
    public float targetRotation;
    //////////////////////////////State
    //////////////////////////////Cached Component References


    void Start()
    {
        StartCoroutine(ContinuallyRotate());
    }

    public IEnumerator RotateChunk()
    {   
        float rotationAmount = 45f * (Random.Range(1,7));
        targetRotation = this.transform.rotation.eulerAngles.z + rotationAmount > 360 ? this.transform.rotation.eulerAngles.z + rotationAmount -360 : this.transform.rotation.eulerAngles.z + rotationAmount;
        int OneOrZero = Random.Range(0,2);
        rotSpeedPlusRandom = baseRotSpeed * Random.Range(0.5f, 3f);   //as of now, the rotator only rotates in one direction, to match the counter clockwise direction of the passing turrets.  If you want have it    
                                                        //rotate randomly in both directions, change this to Random.Range(-3f, 3f);  
        while (Mathf.Abs(this.transform.rotation.eulerAngles.z - targetRotation) > 2)
        {
            this.transform.Rotate(rotSpeedPlusRandom *Time.deltaTime ) ; 
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
