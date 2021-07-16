using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilliardsTarget : MonoBehaviour
{
    
    void OnCollisionEnter2D(Collision2D other)    
    {
        if (other.gameObject.TryGetComponent(out CueBall ball)    )
        {
            print("cue ball hit target");
        }
    }
}
