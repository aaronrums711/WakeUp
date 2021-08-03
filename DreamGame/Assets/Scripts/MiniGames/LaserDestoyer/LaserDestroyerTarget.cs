using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroyerTarget : MiniGameElement
{

    //////////////////////////////Config

    //////////////////////////////State
    public bool isScoreAdding;
    //////////////////////////////Cached Component References
    public  ParticleSystem ps;



    void Start()
    {
        isScoreAdding = false;
        ps = parentMiniGame.GetComponentInChildren<ParticleSystem>();

    }

    void Update()
    {
        if (isScoreAdding)
        {
            parentMiniGame.AddProgress(parentMiniGame.baseProgression * Time.deltaTime);
            // ps.Play();
        }
        else
        {
            // ps.Stop();
        }
    }
}
