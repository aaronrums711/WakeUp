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
        print(dest);
        Instantiate(targetPrefab, dest, Quaternion.identity, targetParent);
    }
}
