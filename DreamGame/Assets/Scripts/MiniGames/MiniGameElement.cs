using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MiniGameElement : MonoBehaviour
{
    public MiniGame parentMiniGame;

    void Awake()
    {
        GetParentMiniGame();
    }

    public void GetParentMiniGame()
    {
        print(this.gameObject.name + " is searching for parent mini game");
        parentMiniGame = GetComponentInParent<MiniGame>();
        if (parentMiniGame == null)
        {
            parentMiniGame = GetComponentInParent<Transform>().GetComponentInParent<MiniGame>();

            if(parentMiniGame == null)
            {
                Debug.LogError("this game element doesn't have a parent MiniGame in a direct parent or a parent's parent.  Something is wrong");
            }
            
        }
    }
    

    ///this method doesn't take into account trying to avoid any other objects
    public Vector3 GetRandomPoint(Transform barrierObjectParent, float padding)
    {
        float maxY = 0;
        float maxX=  0;
        float minY = 0;
        float minX = 0;
        for (int i = 0; i < barrierObjectParent.childCount; i++)
        {
            float testY =  barrierObjectParent.GetChild(i).transform.position.y;
            float testX = barrierObjectParent.GetChild(i).transform.position.x;
            if (testY > maxY) {maxY = testY-padding;}
            else if(testY< minY){minY = testY+padding;}

            if (testX > maxX) {maxX = testX-padding;}
            else if(testX < minX) {minX = testX+padding;}
        }
        Vector3 targetLocation = new Vector3(Random.Range(minY, maxY), Random.Range(minX, maxX));
        return targetLocation;
    }

    public Vector3 GetRandomPoint(List<Transform> barriers, float padding, List<Transform> objectsToAvoid)
    {
        float maxY = 0;
        float maxX=  0;
        float minY = 0;
        float minX = 0;
        for (int i = 0; i < barriers.Count; i++)
        {
            float testY =  barriers[i].position.y;
            float testX = barriers[i].position.x;
            if (testY > maxY) {maxY = testY-padding;}
            else if(testY< minY){minY = testY+padding;}

            if (testX > maxX) {maxX = testX-padding;}
            else if(testX < minX) {minX = testX+padding;}
        }

        Vector3 finalTargetLocation = new Vector3(1000,1000,1000);
        List<float> distances = new List<float>();
        int iterations = 0;
        while(finalTargetLocation.x == 1000)
        {
            distances = new List<float>();
            Vector3 testTargetLocation = new Vector3(Random.Range(minY, maxY), Random.Range(minX, maxX));
            
            for (int i=0; i< objectsToAvoid.Count; i++)
            {
                distances.Add(Vector3.Distance(testTargetLocation, objectsToAvoid[i].position));
            }
             
            if(distances.Count == 0)
            {
                finalTargetLocation = testTargetLocation;
            }
            else if (distances.Min() >padding)
            {
                finalTargetLocation = testTargetLocation;
            }
            else if( iterations >=10)
            {
                break;
            }
            iterations++;
        }
        // print(finalTargetLocation);
        return finalTargetLocation;
    }



}
