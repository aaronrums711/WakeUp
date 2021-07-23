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
        //we need list of transforms barriers and list transforms objectsToAvoid
        // List<KnockEmDownTarget> currentTargets = new List<KnockEmDownTarget>();
        // parentMiniGame.GetComponentsInChildren<KnockEmDownTarget>(currentTargets);
        List<Transform> targetTransforms = new List<Transform>();
        foreach (KnockEmDownTarget target in parentMiniGame.GetComponentsInChildren<KnockEmDownTarget>())
        {
            targetTransforms.Add(target.transform);
        }

        Vector3 dest = GetRandomPoint(playAreaBarriers, targetLocationPadding, targetTransforms);
        print(dest);
        // Instantiate(targetPrefab, , Quaternion.identity, targetParent);
    }



}
///UPON RETURN:  get this methodh working so that when it's called from the editor, a single target randomly appears.  The target should also start shrinking