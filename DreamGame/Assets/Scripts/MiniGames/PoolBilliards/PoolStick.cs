using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolStick : MiniGame
{
    public GameObject target;
    private float angle = 20; //controls speed. this is arbitrary, since we are using rotationSpeed to control it as well. 
    public float rotationSpeed;
    public float drawBackSpeed;

    void Start()
    {
        orderInLevel = 1;
    }

    void Update()
    {
        if (!Input.GetKey(keysToPlay[orderInLevel]))
        {
            transform.RotateAround(target.transform.position, Vector3.forward, angle * Time.deltaTime * rotationSpeed);
        }
        else if (Input.GetKey(keysToPlay[orderInLevel]))
        {
            transform.Translate(Vector2.down * Time.deltaTime * drawBackSpeed);
        }
    }
}
