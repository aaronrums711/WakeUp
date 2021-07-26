using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTargetRotator : MonoBehaviour
{
    //////////////////////////////Config
    //////////////////////////////State
    //////////////////////////////Cached Component References
    public Transform rotationTesting;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(rotationTesting.position, Vector3.up);
        // this.transform.up = rotationTesting.position;
    }
}
