using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroyerInputManager : MiniGameElement
{

    //////////////////////////////Config

    //////////////////////////////State

    //////////////////////////////Cached Component References
    public List<LaserEmitter> allLaserEmitters;

    void Start()
    {
        var emitters = parentMiniGame.GetComponentsInChildren<LaserEmitter>();
        foreach (LaserEmitter emitter in emitters)
        {
            allLaserEmitters.Add(emitter);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(parentMiniGame.keyForThisGame))
        {
            PassToNextTurret();
        }
    }

    private void PassToNextTurret()
    {
        return;
    }
}
