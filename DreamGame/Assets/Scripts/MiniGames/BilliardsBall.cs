using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilliardsBall : MonoBehaviour
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

    public void ClampVelocity()
    {
        if (thisRB.velocity.sqrMagnitude <= clampPoint)
        {
            thisRB.velocity = Vector2.zero;
        }
    }
}
