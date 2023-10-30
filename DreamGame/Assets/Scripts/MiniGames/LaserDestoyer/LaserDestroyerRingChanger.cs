using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserDestroyerRingChanger : MiniGameElement
{
	/*****************
	CreateDate: 7/31/21
	UpdateDate: 10/19/23 
    Functionality: shifts around the piecies of the protective ring.  Previously was using an animator, but has been refactored. 
	******************/
	
	//////////////////////////////Config
    private Dictionary<GameObject, float> startingYValues = new Dictionary<GameObject, float>();
    [SerializeField] private float changeTime = 3f;
    [SerializeField] private float waitTimeAfterChange = 3f;
    [SerializeField] private float waitTimeBetweenChange = 3f;

	//////////////////////////////State
	
	//////////////////////////////Cached Component References
    public GameObject[] allRingPieces; //VGIU

    void Start()
    {
        if (allRingPieces == null)
        {
            Debug.LogError("You need to assin the ring pieces on this object: " + this.name);
        }
        foreach (GameObject go in allRingPieces)
        {
            startingYValues.Add(go, go.transform.localScale.y);
        }
        StartCoroutine(ChangeRings());
    }



    public IEnumerator ChangeRings()
    {
        int piecesToChange = UnityEngine.Random.Range(1, allRingPieces.Length);
        List<int> indexesToChange = new List<int>();

        while (indexesToChange.Count < piecesToChange)
        {
            int index = UnityEngine.Random.Range(1, allRingPieces.Length);
            if (!indexesToChange.Contains(index))
            {
                indexesToChange.Add(index);
            }
        }
        
        for (int i = 0; i < indexesToChange.Count; i++)
        {
            StartCoroutine(ChangePiece(allRingPieces[indexesToChange[i]], true));
        }
        yield return StartCoroutine(Wait(waitTimeAfterChange));
        
        for (int i = 0; i < indexesToChange.Count; i++)
        {
            StartCoroutine(ChangePiece(allRingPieces[indexesToChange[i]], false));
        }
        yield return StartCoroutine(Wait(waitTimeBetweenChange));
        yield return ChangeRings();
    }

    public IEnumerator ChangePiece(GameObject obj, Boolean shrink)
    {
        float initScaleY = obj.transform.localScale.y;
        float elapsed = 0f;
        float totalTime = changeTime;
        while (elapsed < totalTime)
        {
            float targetYVal = (shrink) ? 0 : startingYValues[obj];
            float newY =  Mathf.Lerp(initScaleY, targetYVal,  elapsed/totalTime);
            Vector3 newScale = new Vector3(obj.transform.localScale.x, newY, obj.transform.localScale.z);
            obj.transform.localScale = newScale;

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Wait(float seconds)
    {
         yield return new WaitForSeconds(seconds);
    }


}
