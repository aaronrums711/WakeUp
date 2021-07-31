using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroyerTarget : MiniGameElement
{

    //////////////////////////////Config

    //////////////////////////////State
    public bool isScoreAdding;
    //////////////////////////////Cached Component References


    void Start()
    {
        isScoreAdding = false;
    }

    void Update()
    {
        if (isScoreAdding)
        {
            parentMiniGame.AddProgress(parentMiniGame.baseProgression * Time.deltaTime);
        }
    }
}
