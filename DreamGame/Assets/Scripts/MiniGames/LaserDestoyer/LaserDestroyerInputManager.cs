using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroyerInputManager : MiniGameElement
{

    //////////////////////////////Config

    //////////////////////////////State
    public LaserEmitter currentActiveEmitter;
    public int currentActiveEmitterChildIndex;

    //////////////////////////////Cached Component References
    public List<LaserEmitter> allLaserEmitters;

    void Start()
    {
        var emitters = parentMiniGame.GetComponentsInChildren<LaserEmitter>();
        foreach (LaserEmitter emitter in emitters)
        {
            allLaserEmitters.Add(emitter);
        }
        currentActiveEmitterChildIndex = Random.Range(0, allLaserEmitters.Count-1);
        currentActiveEmitter = allLaserEmitters[currentActiveEmitterChildIndex].CallInitialLaserCast2();
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
        currentActiveEmitter.CallRetractLaser();
        
        if (currentActiveEmitterChildIndex == allLaserEmitters.Count-1 )
        {
            currentActiveEmitterChildIndex = 0;
        }
        else
        {
            currentActiveEmitterChildIndex++;
        }
        currentActiveEmitter = allLaserEmitters[currentActiveEmitterChildIndex].CallInitialLaserCast2();
    }

}
