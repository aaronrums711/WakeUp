using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBall : MiniGameElement
{

    //////////////////////////////Config
     [Range(0f, 0.5f)] public float clampPoint;

    //////////////////////////////State

    //////////////////////////////Cached Component References
    public Rigidbody2D thisRB;
    private PoolStick poolStick;




    void Start()
    {
        poolStick =  parentMiniGame.GetComponentInChildren<PoolStick>();
        thisRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        ClampVelocity();
    }

    public void ClampVelocity() //all this does is set the velocity to zero instead of waiting for it to go to 0 natuarlly.  the poolstick the velocity to reappear. 
    {
        if (thisRB.velocity.sqrMagnitude <= clampPoint && poolStick.isSlowed == false)
        {
            thisRB.velocity = Vector2.zero;
        }
    }
}
