using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroyerInputManager : MiniGameElement, IOneKeyPlay
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
        currentActiveEmitter = allLaserEmitters[currentActiveEmitterChildIndex].CallInitialLaserCast();
    }

    void Update()
    {
        if (Input.GetKeyDown(parentMiniGame.keyForThisGame) && parentMiniGame.isActive)
        {
            OneKeyPlay();
        }
    }

    public void OneKeyPlay()
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
        currentActiveEmitter = allLaserEmitters[currentActiveEmitterChildIndex].CallInitialLaserCast();
    }

}
