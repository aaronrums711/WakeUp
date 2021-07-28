using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{

    //////////////////////////////Config
    public float laserInitializationLength;  //VGIU


    //////////////////////////////State
    public bool isLaserInitialized;

    //////////////////////////////Cached Component References
    private LineRenderer lineRenderer;
    public Transform emissionPoint;   //VGIU
    public Transform centerTarget;    //VGIU


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = true;
        StartCoroutine(InitialLaserCast());
        print("starting line position count: " + lineRenderer.positionCount);

    }

    void Update()
    {
        // RaycastHit2D hit = Physics2D.Raycast(emissionPoint.position, centerTarget.position- emissionPoint.position);
        // Debug.DrawLine(emissionPoint.position, hit.point);
        // lineRenderer.SetPosition(0, emissionPoint.position);
        // lineRenderer.SetPosition(1, hit.point);
        // // lineRenderer.SetPosition(2, new Vector3(5,5,5));
    }


    private IEnumerator InitialLaserCast()
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
        isLaserInitialized = true;
        print("end of the loop reached!");

    }

    [ContextMenu("initialize laser")]
    public void CallInitialLaserCastFromEditor()
    {
        StartCoroutine(InitialLaserCast());
    }
}
