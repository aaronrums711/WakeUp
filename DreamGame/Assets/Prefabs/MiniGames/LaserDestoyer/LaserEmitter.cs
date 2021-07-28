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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, centerTarget.position- this.transform.position);
        Debug.DrawLine(transform.position, hit.point);
        lineRenderer.SetPosition(0, emissionPoint.position);
        lineRenderer.SetPosition(1, hit.point);
    }

    /**
    at this point the line is rendering, but not in the right place.  gotta fix the raycast to emit from the relative up direction of shootpoint
    **/
}
