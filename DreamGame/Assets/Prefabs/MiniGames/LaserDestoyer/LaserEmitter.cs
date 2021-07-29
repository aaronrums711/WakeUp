using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{

    //////////////////////////////Config
    public float laserInitializationLength;  //VGIU
    private IEnumerator maintainCoroutine;

    //////////////////////////////State
    public bool isLaserInitialized;


    //////////////////////////////Cached Component References
    private LineRenderer lineRenderer;
    public Transform emissionPoint;   //VGIU
    public Transform centerTarget;    //VGIU


    void OnEnable()
    {
        print("this has been called from OnEnable");
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = true;
        StartCoroutine(InitialLaserCast());

    }

    void Update()
    {
        // RaycastHit2D hit = Physics2D.Raycast(emissionPoint.position, centerTarget.position- emissionPoint.position);
        // Debug.DrawLine(emissionPoint.position, hit.point);
        // lineRenderer.SetPosition(0, emissionPoint.position);
        // lineRenderer.SetPosition(1, hit.point);
        // // lineRenderer.SetPosition(2, new Vector3(5,5,5));
    }


    [ContextMenu("initialize laser")]
    public void CallInitialLaserCastFromEditor()
    {
        lineRenderer.positionCount= 0;
        StartCoroutine(InitialLaserCast());
    }

    private IEnumerator InitialLaserCast()
    {
        float startTime = Time.time;
        float totalTime = laserInitializationLength;
        float elapsedPercent = (Time.time - startTime)/laserInitializationLength;
        int nextPosIndex;
        int iterations = 1;
        lineRenderer.positionCount= 1;

        RaycastHit2D hit = Physics2D.Raycast(emissionPoint.position, centerTarget.position- emissionPoint.position);
        Vector2 newPos = Vector2.Lerp((Vector2)emissionPoint.position, (Vector2)hit.point, elapsedPercent);
        lineRenderer.SetPosition(0, newPos);

        while (Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount-1), hit.point) > 0.2f)
        {
            print("iteration count: " + iterations);
            lineRenderer.positionCount++;
            hit = Physics2D.Raycast(emissionPoint.position, centerTarget.position- emissionPoint.position);
            elapsedPercent = (Time.time - startTime)/laserInitializationLength;
            nextPosIndex = lineRenderer.positionCount-1;
            newPos = Vector2.Lerp((Vector2)emissionPoint.position, (Vector2)hit.point, elapsedPercent);
            lineRenderer.SetPosition(nextPosIndex, newPos);
            iterations++;
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

    // public IEnumerator RetractLaser()
    // {
    //     StopCoroutine(maintainCoroutine);

    // }

}
