using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockEmDownTargetDestroyer : MiniGameElement, IProgressionAdder
{
    //////////////////////////////Config
     [Tooltip("only used for the brief time that the target grows before it shrinks and disappears")]  
    public float growRate;
    public float growEffectLength = 0.5f;
    public float endEffectShrinkRate = 10;

    //////////////////////////////State

    //////////////////////////////Cached Component References
    private KnockEmDownWaveManager waveManager;

    void Start()
    {
        waveManager = parentMiniGame.GetComponentInChildren<KnockEmDownWaveManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(parentMiniGame.keyForThisGame) && parentMiniGame.isActive)
        {
            if (waveManager.objectsInCurrentWave.Count <= 0)
            {
                parentMiniGame.AddProgress(-0.02f);
            }
            else if(waveManager.objectsInCurrentWave.Count >= 0)
            {
                AddMiniGameProgress();
            }
        }
    }

    public void AddMiniGameProgress()
    {
        float scale = waveManager.objectsInCurrentWave[0].GetComponent<KnockEmDownTarget>().CallDestroyEffect();
        parentMiniGame.AddProgress(parentMiniGame.baseProgression * scale);
    }
}
