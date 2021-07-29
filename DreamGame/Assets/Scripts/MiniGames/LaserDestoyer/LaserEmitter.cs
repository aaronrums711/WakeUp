using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MiniGameElement
{

    //////////////////////////////Config
    public float laserInitializationLength;  //VGIU
    private IEnumerator maintainCoroutine;
    public float laserReductionMultiplier = 0.09f;

    //////////////////////////////State
    public bool isLaserInitialized = false;


    //////////////////////////////Cached Component References
    private LineRenderer lineRenderer;
    public Transform emissionPoint;   //VGIU
    public Transform centerTarget;    //VGIU


    void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = true;
        // StartCoroutine(InitialLaserCast());   this will now get called from LaserDestroyerInputManager script
    }


    [ContextMenu("initialize laser")]
    public void CallInitialLaserCastFromEditor()
    {
        lineRenderer.positionCount= 0;
        StartCoroutine(InitialLaserCast());
    }

    public IEnumerator InitialLaserCast()
    {
        float startTime = Time.time;
        float totalTime = laserInitializationLength;
        float elapsedPercent = (Time.time - startTime)/laserInitializationLength;
        int nextPosIndex;
        lineRenderer.positionCount= 1;

        RaycastHit2D hit = Physics2D.Raycast(emissionPoint.position, centerTarget.position- emissionPoint.position);
        Vector2 newPos = Vector2.Lerp((Vector2)emissionPoint.position, (Vector2)hit.point, elapsedPercent);
        lineRenderer.SetPosition(0, newPos);

        while (Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount-1), hit.point) > 0.2f)
        {
            lineRenderer.positionCount++;
            hit = Physics2D.Raycast(emissionPoint.position, centerTarget.position- emissionPoint.position);
            elapsedPercent = (Time.time - startTime)/laserInitializationLength;
            nextPosIndex = lineRenderer.positionCount-1;
            newPos = Vector2.Lerp((Vector2)emissionPoint.position, (Vector2)hit.point, elapsedPercent);
            lineRenderer.SetPosition(nextPosIndex, newPos);
            yield return null;
        }
        hit = Physics2D.Raycast(emissionPoint.position, centerTarget.position- emissionPoint.position);
        lineRenderer.positionCount=2;
        lineRenderer.SetPosition(0, emissionPoint.position);
        lineRenderer.SetPosition(1, hit.point);

        isLaserInitialized = true;
        maintainCoroutine = MaintainLaser();
        StartCoroutine(maintainCoroutine);
    }



    public IEnumerator MaintainLaser()
    {
        print("maintain laser coroutine started");
        RaycastHit2D hit = Physics2D.Raycast(emissionPoint.position, centerTarget.position- emissionPoint.position);

        while(isLaserInitialized)
        {
            hit = Physics2D.Raycast(emissionPoint.position, centerTarget.position- emissionPoint.position);
            lineRenderer.SetPosition(0, emissionPoint.position);
            lineRenderer.SetPosition(1, hit.point);
            yield return null;
        }
    }


    public IEnumerator RetractLaser()
    {
        isLaserInitialized = false;
        StopCoroutine(maintainCoroutine);
        Vector3 subtractionVector = ((centerTarget.position- emissionPoint.position).normalized)*laserReductionMultiplier;
        

        while (Vector2.Distance(lineRenderer.GetPosition(1), emissionPoint.position) > 0.1)
        {
            Vector2 newPos = lineRenderer.GetPosition(1) - subtractionVector;
            lineRenderer.SetPosition(1, newPos);
            yield return null;
        }
        lineRenderer.positionCount = 0;        
    }

    [ContextMenu("retract laser")]
    public void CallRetractLaser()
    {
        StartCoroutine(RetractLaser());
    }


}
