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
    public  ParticleSystem ps;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = parentMiniGame.targetColor;
        colorKeys[0].time= 0.0f;
        colorKeys[1].color = parentMiniGame.baseColor;
        colorKeys[1].time =0.35f;

        GradientAlphaKey[] alphaKeys;
        alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 0.0f;
        alphaKeys[1].time = 1.0f;

        gradient.SetKeys(colorKeys, alphaKeys);
        lineRenderer.colorGradient = gradient;
    }

    void OnEnable()
    {
        
        target = parentMiniGame.GetComponentInChildren<LaserDestroyerTarget>();
        ps = parentMiniGame.GetComponentInChildren<ParticleSystem>();
        centerTargetTransform = target.transform;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        // StartCoroutine(InitialLaserCast());   this will now get called from LaserDestroyerInputManager script
        foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            parentMiniGame.ApplyColor(sr, 0.6f);
        }
        ps.Stop();   
        
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
                ps.transform.position = hit.point;
                ps.transform.right = this.transform.position - centerTargetTransform.position;
                if(ps.isStopped)
                {
                    ps.Play();
                }
                
            }
            else 
            {
                target.isScoreAdding = false;
                // ps.Stop();
                ps.Pause();
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
