using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBall : MonoBehaviour
{

    //////////////////////////////Config
     [Range(0f, 0.5f)] public float clampPoint;

    //////////////////////////////State

    //////////////////////////////Cached Component References
    public Rigidbody2D thisRB;



    void Start()
    {
        thisRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        ClampVelocity();
    }

    public void ClampVelocity() //all this does is set the velocity to zero instead of waiting for it to go to 0 natuarlly.  the poolstick the velocity to reappear. 
    {
        if (thisRB.velocity.sqrMagnitude <= clampPoint)
        {
            thisRB.velocity = Vector2.zero;
        }
    }
}
