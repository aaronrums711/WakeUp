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
}
