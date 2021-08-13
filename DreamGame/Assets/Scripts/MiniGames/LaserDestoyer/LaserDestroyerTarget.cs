using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroyerTarget : MiniGameElement, IProgressionAdder
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
            AddMiniGameProgress();
        }
        else
        {
            
        }
    }

    public void AddMiniGameProgress()
    {
        parentMiniGame.AddProgress(parentMiniGame.baseProgression * Time.deltaTime);
    }
}
