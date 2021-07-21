using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHitManager :  MiniGameElement
{
    public int targetsHitThisShot = 0;

    public void ProcessHit()
    {
        targetsHitThisShot++;
        float progressionAmount = parentMiniGame.baseProgression + (targetsHitThisShot * 0.02f);
        parentMiniGame.AddProgress(progressionAmount);
    }

    public void ResetTargetsHit()
    {
        targetsHitThisShot = 0;
    }

}
