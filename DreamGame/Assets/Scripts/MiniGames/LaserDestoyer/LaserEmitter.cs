using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MiniGameElement
{

    //////////////////////////////Config
    private IEnumerator currentCoroutine;
    public float laserReductionMultiplier = 0.09f;
    public float laserInitializationMultiplier = 0.13f;

    //////////////////////////////State
    public bool isLaserInitialized = false;


    //////////////////////////////Cached Component References
    private LineRenderer lineRenderer;
    public Transform emissionPoint;   //VGIU
    private Transform centerTargetTransform;    
    private LaserDestroyerTarget target;

    void OnEnable()
    {
        
        target = parentMiniGame.GetComponentInChildren<LaserDestroyerTarget>();
        centerTargetTransform = target.transform;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        // StartCoroutine(InitialLaserCast());   this will now get called from LaserDestroyerInputManager script
        foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            parentMiniGame.ApplyColor(sr, 0.6f);
        }
    }


    public IEnumerator InitialLaserCast()
    {
        lineRenderer.positionCount= 2;
        RaycastHit2D hit = Physics2D.Raycast(emissionPoint.position, centerTargetTransform.position- emissionPoint.position);
        Vector3 additionVector =  ((centerTargetTransform.position- emissionPoint.position).normalized)*laserInitializationMultiplier;
        lineRenderer.SetPosition(0, emissionPoint.position);
        lineRenderer.SetPosition(1, emissionPoint.position+ additionVector);

        while(Vector2.Distance(lineRenderer.GetPosition(1), hit.point)>0.2)
        {
            hit = Physics2D.Raycast(emissionPoint.position, centerTargetTransform.position- emissionPoint.position);
            Vector2 newPos = lineRenderer.GetPosition(1) + additionVector;
            lineRenderer.SetPosition(1, newPos);
            yield return null;
        }

        hit = Physics2D.Raycast(emissionPoint.position, centerTargetTransform.position- emissionPoint.position);
        lineRenderer.SetPosition(1, hit.point);

        isLaserInitialized = true;
        currentCoroutine = MaintainLaser();
        StartCoroutine(currentCoroutine);
    }

    public IEnumerator MaintainLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(emissionPoint.position, centerTargetTransform.position- emissionPoint.position);

        while(isLaserInitialized)
        {
            hit = Physics2D.Raycast(emissionPoint.position, centerTargetTransform.position- emissionPoint.position);
            lineRenderer.SetPosition(0, emissionPoint.position);
            lineRenderer.SetPosition(1, hit.point);
            if (hit.collider.name == target.name)
            {
                target.isScoreAdding = true;
            }
            else 
            {
                target.isScoreAdding = false;
            }
            yield return null;
        }
    }

    public IEnumerator RetractLaser()
    {
        isLaserInitialized = false;
        StopCoroutine(currentCoroutine);
        Vector3 subtractionVector = ((centerTargetTransform.position- emissionPoint.position).normalized)*laserReductionMultiplier;
        

        while (Vector2.Distance(lineRenderer.GetPosition(1), emissionPoint.position) > 0.1)
        {
            Vector2 newPos = lineRenderer.GetPosition(1) - subtractionVector;
            lineRenderer.SetPosition(1, newPos);
            yield return null;
        }
        lineRenderer.positionCount = 0;        
    }

    public void CallRetractLaser()
    {
        StartCoroutine(RetractLaser());
    }

    public LaserEmitter CallInitialLaserCast()
    {
        lineRenderer.positionCount= 0;
        currentCoroutine = InitialLaserCast();
        StartCoroutine(currentCoroutine);
        return this;
    }


}
