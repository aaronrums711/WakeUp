using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroyerTarget : MiniGameElement, IProgressionAdder
{

    //////////////////////////////Config

    //////////////////////////////State
    public bool isScoreAdding;
    //////////////////////////////Cached Component References
    public  ParticleSystem ps;  //vgiu



    void Start()
    {
        isScoreAdding = false;

    }

    void Update()
    {
        if (isScoreAdding && parentMiniGame.isActive)
        {
            AddMiniGameProgress();
        }
        else
        {
            
        }
    }

    public void AddMiniGameProgress()
    {
        parentMiniGame.AddProgress((parentMiniGame.baseProgression * Time.deltaTime) * parentMiniGame.progressionParams.universalProgressionMultiplier);
    }
}
