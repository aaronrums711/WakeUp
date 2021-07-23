using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KnockDownTargetSpawner : MiniGameElement
{


    //////////////////////////////Config
    public float targetLocationPadding = 0.5f;

    //////////////////////////////State

    //////////////////////////////Cached Component References
    public KnockEmDownTarget targetPrefab; //VGIU
    private Transform targetParent;

    void Start()
    {
        targetParent = GameObject.Find("KnockDownTargets").GetComponent<Transform>();
        if (targetParent == null){print("something is wrong, a parent object for the KnockEmDown targets was not found");}
    }

    [ContextMenu("InstantiateTarget()")]
    public void InstantiateTarget()
    {
        Instantiate(targetPrefab, SearchForLocation(), Quaternion.identity, targetParent);
    }



    private Vector3 SearchForLocation()
    {   //this function depends on being on the parent PlayArea object, with the barriers as children. 
        float maxY = 0;
        float maxX=  0;
        float minY = 0;
        float minX = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            float testY =  this.transform.GetChild(i).transform.position.y;
            float testX = this.transform.GetChild(i).transform.position.x;
            if (testY > maxY) {maxY = testY-targetLocationPadding;}
            else if(testY< minY){minY = testY+targetLocationPadding;}

            if (testX > maxX) {maxX = testX-targetLocationPadding;}
            else if(testX < minX) {minX = testX+targetLocationPadding;}
        }


        Vector3 finalTargetLocation = new Vector3(1000,1000,1000);
        List<float> distances = new List<float>();
        int iterations = 0;
        while(finalTargetLocation.x == 1000)
        {
            distances = new List<float>();
            Vector3 testTargetBallLocation = new Vector3(Random.Range(minY, maxY), Random.Range(minX, maxX));
            
            foreach (KnockEmDownTarget target in targetParent.GetComponentsInChildren<KnockEmDownTarget>())
            {   
                ///get other target ball distances
                distances.Add(Vector3.Distance(testTargetBallLocation, target.transform.position));
            }
    
            if (distances.Min() >targetLocationPadding)
            {
                finalTargetLocation = testTargetBallLocation;
            }
            else if( iterations >=10)
            {
                break;
            }
            iterations++;
        }
        return finalTargetLocation;
    }
}


///UPON RETURN:  get this methodh working so that when it's called from the editor, a single target randomly appears.  The target should also start shrinking