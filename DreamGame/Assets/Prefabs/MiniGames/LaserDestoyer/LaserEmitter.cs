using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform emissionPoint;
    public Transform centerTarget;

    public float laserInitializationLength;  //VGIU

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1;
        lineRenderer.enabled = true;
        StartCoroutine(InitialLaserCast());
        print("starting line position count: " + lineRenderer.positionCount);

    }

    void Update()
    {
        // RaycastHit2D hit = Physics2D.Raycast(emissionPoint.position, centerTarget.position- emissionPoint.position);
        // Debug.DrawLine(transform.position, hit.point);
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
        
        RaycastHit2D hit = Physics2D.Raycast(emissionPoint.position, centerTarget.position- this.transform.position);
        Vector2 newPos = Vector2.Lerp((Vector2)emissionPoint.position, (Vector2)hit.point, elapsedPercent);
        lineRenderer.SetPosition(0, newPos);

        while (Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount-1), hit.point) > 0.2f)
        {
            lineRenderer.positionCount++;
            hit = Physics2D.Raycast(emissionPoint.position, centerTarget.position- this.transform.position);
            elapsedPercent = (Time.time - startTime)/laserInitializationLength;
            nextPosIndex = lineRenderer.positionCount-1;
            newPos = Vector2.Lerp((Vector2)emissionPoint.position, (Vector2)hit.point, elapsedPercent);
            lineRenderer.SetPosition(nextPosIndex, newPos);
            yield return null;
            
        }
        print("end of the loop reached!");

    }
}
