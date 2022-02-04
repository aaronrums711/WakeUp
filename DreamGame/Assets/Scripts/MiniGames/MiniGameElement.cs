using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MiniGameElement : MonoBehaviour
{
    public MiniGame parentMiniGame;
    public static  Action<GameObject> OnDestroyGameElement;  //these can't be events because they need to be called from child scripts.  Events can only be calle from the class that created them. 
    public static  Action<GameObject> OnSpawnGameElement;

    void Awake()
    {
        GetParentMiniGame();
    }

    public void GetParentMiniGame()
    {
        // print(this.GetType() + " on " + this.gameObject.name + " is searching for parent mini game");  //uncomment this for testing

        parentMiniGame = GetComponent<MiniGame>(); //for scripts on the same object as MiniGame
        if (parentMiniGame == null)
        {
            parentMiniGame = GetComponentInParent<MiniGame>(); //for scripts on a child object

            if (parentMiniGame == null)
            {
                parentMiniGame = GetComponentInParent<Transform>().GetComponentInParent<MiniGame>();  //for scripts on a child's child
            }
        }

        if(parentMiniGame == null)
        {
            Debug.LogError("this game element doesn't have a parent MiniGame in a direct parent or a parent's parent.  Something is wrong");
        }
    }
    

    ///this method doesn't take into account trying to avoid any other objects
    public Vector3 GetRandomPoint(Transform barrierObjectParent, float padding)
    {
        float maxY = -1000;
        float maxX=  -1000;
        float minY = 1000;
        float minX = 1000;
        for (int i = 0; i < barrierObjectParent.childCount; i++)
        {
            float testY =  barrierObjectParent.GetChild(i).transform.position.y;
            float testX = barrierObjectParent.GetChild(i).transform.position.x;
            if (testY > maxY) {maxY = testY-padding;}
            else if(testY< minY){minY = testY+padding;}

            if (testX > maxX) {maxX = testX-padding;}
            else if(testX < minX) {minX = testX+padding;}
        }
        Vector3 targetLocation = new Vector3(UnityEngine.Random.Range(minY, maxY), UnityEngine.Random.Range(minX, maxX));
        return targetLocation;
    }

    public Vector3 GetRandomPoint(List<Transform> barriers, float padding, List<Transform> objectsToAvoid)
    {
        float maxY = -1000;
        float maxX=  -1000;
        float minY = 1000;
        float minX = 1000;
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
            // Vector3 testTargetLocation = new Vector3(Random.Range(minY, maxY), Random.Range(minX, maxX));  wow, this was wrong!
            Vector3 testTargetLocation = new Vector3( UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
            
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
        return finalTargetLocation;
    }



}
