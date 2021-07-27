using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform endPoint;
    public Transform emissionPoint;



    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
        Debug.DrawLine(transform.position, hit.point);
        endPoint.position = hit.point;
        lineRenderer.SetPosition(0, emissionPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
