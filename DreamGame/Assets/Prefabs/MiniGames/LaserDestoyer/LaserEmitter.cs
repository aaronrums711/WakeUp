using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform emissionPoint;
    public Transform centerTarget;



    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
    }

    void Update()
    {
        // RaycastHit2D hit = Physics2D.Raycast(transform.position, centerTarget.position- this.transform.position);
        // Debug.DrawLine(transform.position, hit.point);
        // lineRenderer.SetPosition(0, emissionPoint.position);
        // lineRenderer.SetPosition(1, hit.point);

        StartCoroutine(InitialLaserCast());
    }


    private IEnumerator InitialLaserCast()
    {
        float startTime = Time.time;
        float elapsed = Time.time - startTime;
        float totalTime = 1f;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, centerTarget.position- this.transform.position);
        int maxPositions = lineRenderer.positionCount;
        Vector2 newPos = Vector2.Lerp((Vector2)emissionPoint.position, (Vector2)hit.point, elapsed);
        lineRenderer.SetPosition(0, newPos);

        while (Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount), hit.point) > 0.2f)
        {
            hit = Physics2D.Raycast(transform.position, centerTarget.position- this.transform.position);
            elapsed = Time.time - startTime;
            int nextPosIndex = lineRenderer.positionCount +1;
            newPos = Vector2.Lerp((Vector2)emissionPoint.position, (Vector2)hit.point, elapsed);
            lineRenderer.SetPosition(nextPosIndex, newPos);
            yield return null;
        }


    }
}
