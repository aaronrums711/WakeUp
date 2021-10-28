using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KnockDownTargetSpawner : MiniGameElement
{

	/*****************
	CreateDate: 	8/05/21 approx
	Functionality:	spawns a single target.  This method is called from the WaveManager class
	Notes:	
	Dependencies:
	******************/

    //////////////////////////////Config
    public float targetLocationPadding = 0.5f;

    //////////////////////////////State

    //////////////////////////////Cached Component References
    public KnockEmDownTarget targetPrefab; //VGIU
    public Transform targetParent;

    void Start()
    {
        targetParent = GameObject.Find("KnockDownTargets").GetComponent<Transform>();
        if (targetParent == null){print("something is wrong, a parent object for the KnockEmDown targets was not found");}
    }

    [ContextMenu("InstantiateTarget()")]
    public GameObject InstantiateTarget()
    {
        List<Transform> targetTransforms = new List<Transform>();
        foreach (KnockEmDownTarget target in targetParent.GetComponentsInChildren<KnockEmDownTarget>())
        {
            targetTransforms.Add(target.transform);
        }

        List<Transform> barriers = new List<Transform>();
        foreach (GameObject go in parentMiniGame.playAreaBarriers)
        {
            barriers.Add(go.transform);;
        }

        Vector3 dest = GetRandomPoint(barriers, targetLocationPadding, targetTransforms);
        return Instantiate(targetPrefab.gameObject, dest, Quaternion.identity, targetParent);
    }
}
