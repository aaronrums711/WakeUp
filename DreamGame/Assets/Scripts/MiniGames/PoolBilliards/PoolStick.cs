using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolStick : MiniGame
{
    public GameObject target;
    private float angle = 20; //controls speed. this is arbitrary, since we are using rotationSpeed to control it as well. 
    public float rotationSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.RotateAround(target.transform.position, Vector3.forward, angle * Time.deltaTime * rotationSpeed);
    }
}
