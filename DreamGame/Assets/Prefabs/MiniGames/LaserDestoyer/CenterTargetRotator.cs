using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTargetRotator : MonoBehaviour
{
    //////////////////////////////Config
    public Vector3 rot;

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
        // this.transform.LookAt(rotationTesting.position, Vector3.forward);
        // this.transform.LookAt(rotationTesting.position, Vector3.right);
        this.transform.right = rotationTesting.position;

        // this.transform.Rotate(rot *Time.deltaTime);
    }

    public void RotateChunk()
    {

    }

}
