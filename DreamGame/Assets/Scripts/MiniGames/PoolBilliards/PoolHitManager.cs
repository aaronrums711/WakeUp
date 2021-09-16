using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHitManager :  MiniGameElement, IProgressionAdder
{
    public int targetsHitThisShot = 0;

     [Tooltip("should be between 0 and 0.3 maybe.  Increase this number to increase how much progression is earned with each consecutive hit in a single shot")]  
     public float multipleHitsMultiplier = 0.12f;  //VGIU

    public void ProcessHit()
    {
        AddMiniGameProgress();
        targetsHitThisShot++;
    }

    public void ResetTargetsHit()
    {
        targetsHitThisShot = 0;
    }

    public void AddMiniGameProgress()
    {
        float progressionAmount = (parentMiniGame.baseProgressionChunk + (targetsHitThisShot * multipleHitsMultiplier)) * parentMiniGame.progressionParams.universalProgressionMultiplier;
                                                                                      
        parentMiniGame.AddProgress(progressionAmount);
    }


}
