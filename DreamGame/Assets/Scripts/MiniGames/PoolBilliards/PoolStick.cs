using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolStick : MiniGameElement, IOneKeyPlay
{

    /**
    notes: this must be on the PoolStick object, which has the two children beneath it. 
    **/

    //////////////////////////////Config
    private float angle = 20; //controls rotation speed around ball. this is arbitrary, since we are using rotationSpeed to control it as well. 
    public float rotationSpeed;
    public float drawBackSpeed;
    public float hitSpeed = 10;
    private float pullBackDistance;
    [Range(0.1f, 2)]public float maxAllowedPullbackDistance;  //VGIU
    private Vector3 startingPos;
    private Quaternion startingRot;
    private Color resetColor = Color.white;

    //////////////////////////////State
    public bool isDrawingBack = false;

    [HideInInspector]
    public bool isSlowed;  //this is set from PoolStartStopper, and it's used to delay the reappearance of the pool stick

    //////////////////////////////Cached Component References
    public CueBall ball; //VGIU
    public Rigidbody2D thisRB;
    [SerializeField] private Collider2D cueCollider; //VGIU
    public PoolTargetSpawner spawner;
    public PoolHitManager hitManager; 
    public float speedMultiplier;  //just making this for efficiency purpose, since we don't want to reach for level.difficultyParams.UniversalSpeedMultiplier every frame
    



    void Start()
    {
        hitManager = parentMiniGame.GetComponentInChildren<PoolHitManager>();
        spawner = GameObject.FindObjectOfType<PoolTargetSpawner>();
        startingPos = this.transform.position;
        startingRot = this.transform.rotation;
        thisRB = GetComponent<Rigidbody2D>();
        speedMultiplier = parentMiniGame.difficultyParams.scaleUpMultiplier;
    }

    void Update()
    {
        if (parentMiniGame.isActive) //this will stop the rotation AND the input if isActive = false
        {
            if (!Input.GetKey(parentMiniGame.keyForThisGame)  && isDrawingBack == false)
            {
                transform.RotateAround(ball.transform.position, Vector3.forward, (angle * Time.deltaTime * rotationSpeed)* speedMultiplier);
            }
            else if (Input.GetKeyDown(parentMiniGame.keyForThisGame))
            {
                StartCoroutine(InitiatePullBack());
            }
        }
    }

    private IEnumerator InitiatePullBack()
    {
        isDrawingBack = true;
        
        Vector2 pullBackStartPos = this.transform.position;
        pullBackDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y),pullBackStartPos);

        while(Input.GetKey(parentMiniGame.keyForThisGame) && pullBackDistance < maxAllowedPullbackDistance)  
        {
            transform.Translate(Vector2.down  * Time.deltaTime * drawBackSpeed);
            pullBackDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y),pullBackStartPos);
            yield return null;
        }
        OneKeyPlay();
    }

    public void OneKeyPlay() //this is being given the OneKeyPlay(), because if it's ever called separately, this is what actually generates the force
    {
        float finalHitMultiplier = hitSpeed+ (pullBackDistance * 30); //number here is arbitrary, but it just needs to be high enough to make pulling back the cue stick farther have a distinguishable effect 
        thisRB.AddForce(transform.up * finalHitMultiplier , ForceMode2D.Impulse);
        //this will always hit the ball (and it must).  from the OnCollEnter from that collision, ResetStickPos() is called
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out CueBall ball))
        {
            thisRB.velocity = Vector2.zero;
            thisRB.angularVelocity = 0;
            cueCollider.enabled = false;
            StartCoroutine(ResetStickPos());
        }
    }

    private IEnumerator ResetStickPos()
    {
        //fade out
        for (int i = 0; i < this.transform.childCount; i++)
        {
            StartCoroutine(FadeOut(this.transform.GetChild(i).GetComponent<SpriteRenderer>(), 0.5f, 1,0));
        }

        while (ball.thisRB.velocity.sqrMagnitude > ball.clampPoint  || isSlowed) //keep yielding one frame until the ball stops, THEN set the stick position
        {
            yield return null;
        }
        this.transform.position = ball.transform.position + new Vector3(2,0,0);
        this.transform.rotation = startingRot;
        spawner.AttemptToSpawnTargets();
        //fade in 
        for (int i = 0; i < this.transform.childCount; i++)
        {
            StartCoroutine(FadeOut(this.transform.GetChild(i).GetComponent<SpriteRenderer>(), 0.5f, 0,1));
        }
        
        isDrawingBack = false;
        cueCollider.enabled = true;
        hitManager.ResetTargetsHit();
    }

    private IEnumerator FadeOut(SpriteRenderer sr, float totalTime, float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color newColor = sr.color;
        while(elapsed <= totalTime)
        {
            newColor.a = Mathf.Lerp(startAlpha,endAlpha, elapsed/totalTime);
            sr.color = newColor;
            elapsed+=Time.deltaTime;
            yield return null;
        }

    }

}


