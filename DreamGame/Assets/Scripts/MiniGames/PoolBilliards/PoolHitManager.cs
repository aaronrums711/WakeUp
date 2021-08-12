using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHitManager :  MiniGameElement, IProgressionAdder
{
    public int targetsHitThisShot = 0;

    public void ProcessHit()
    {
        targetsHitThisShot++;
        AddMiniGameProgress();
    }

    public void ResetTargetsHit()
    {
        targetsHitThisShot = 0;
    }

    public void AddMiniGameProgress()
    {
        float progressionAmount = parentMiniGame.baseProgression + (targetsHitThisShot * 0.02f);
        parentMiniGame.AddProgress(progressionAmount);
    }


}
