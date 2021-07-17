using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is mostly just responsible for respawning targets.  There needs to be enough targets at a time so that the player 
//always has something to aim at.  

//either...
//it can spit out balls periodically.  the more balls that are in play should  mean that it's less likley for one to appear. We don't want it to get crowded. 

//or, they can be spit out only when some balls are hit. 
public class PoolBilliardsManager : MiniGameElement
{
    [SerializeField] private int targetsInPlay;
    [SerializeField] private int maxTargetsAllowed = 4;

    
    void Start()
    {
        GetTargetCount();
    }

    void Update()
    {
        
    }

    void GetTargetCount()
    {
        targetsInPlay = parentMiniGame.transform.GetComponentsInChildren<BilliardsTarget>().Length;
    }

    //UPON RETURN: create a method to spawn target balls periodically.  
    //delete the old layers and create new ones.  I think I like the idea of target balls being able to knock out other target balls, 
    //so that layer should be able to collide with itself. 

    //for spawning logic:
    //for every ball less than the max amount, there is a 50% chance to spawn one, then a 40% chance to spawn the next...etc. 
    //this makes it likely that at least one ball will spawn,  but unlikely for all of them up to the max to spawn

    //attempt to spawn for each missing ball up to the max.  the chance of getting one to spawn decreases with each.  
    //however, if there are Max-1 balls in play, then there should be a slim chance of spawning another one, since there are already others to hit
    private void AttemptToSpawnTargets()
    {
        if (targetsInPlay == maxTargetsAllowed) {return;}

        int attempts = maxTargetsAllowed-targetsInPlay;

        float spawnChance= 1f * (attempts/maxTargetsAllowed);  //if there's Max-1 targets in play, there will only be a 20% chance of spawning another
        float spawnChanceReduction = 0.1f;
        for (int i=attempts; i>=0; i--)
        {
            float chance = UnityEngine.Random.Range(0f, 1f);
            if (chance > spawnChance)
            {
                //spawn target
                return;
            }
            spawnChance -= spawnChanceReduction;
        }
    }

    private void SpawnTarget()
    {
        return;
        //need to search for a spot somehow, then spawn the target ball at that spot.  
    }

    ///UPON RETURN: find a way to randomly spawn targets within the play area .  I think the AttemptToSpawnTargets method is good 
}
