using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolStick : MiniGameElement
{


    //////////////////////////////Config
    private float angle = 20; //controls rotation speed around ball. this is arbitrary, since we are using rotationSpeed to control it as well. 
    public float rotationSpeed;
    public float drawBackSpeed;
    public float hitSpeed = 10;
    private float pullBackDistance;
    private Vector3 startingPos;
    private Quaternion startingRot;
    public int orderInLevel;   //change this back to use the mini game value, just doing this now to avoid error
    private Color resetColor = Color.white;

    //////////////////////////////State
    public bool isDrawingBack = false;

    //////////////////////////////Cached Component References
    public CueBall ball; //VGIU
    public Rigidbody2D thisRB;
    [SerializeField] private Collider2D cueCollider; //VGIU
    public PoolTargetSpawner spawner;
    



    void Start()
    {
        spawner = GameObject.FindObjectOfType<PoolTargetSpawner>();
        parentMiniGame = GetComponentInParent<MiniGame>();
        startingPos = this.transform.position;
        startingRot = this.transform.rotation;
        orderInLevel = 1;
        thisRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!Input.GetKey(parentMiniGame.keyForThisGame)  && isDrawingBack == false)
        {
            transform.RotateAround(ball.transform.position, Vector3.forward, angle * Time.deltaTime * rotationSpeed);
        }
        else if (Input.GetKeyDown(parentMiniGame.keyForThisGame))
        {
            StartCoroutine(InitiatePullBack());
        }
    }

    private IEnumerator InitiatePullBack()
    {
        isDrawingBack = true;
        float maxPullBackDistance = 2f;
        Vector2 pullBackStartPos = this.transform.position;
        pullBackDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y),pullBackStartPos);

        while(Input.GetKey(parentMiniGame.keyForThisGame) && pullBackDistance < maxPullBackDistance)  
        {
            transform.Translate(Vector2.down  * Time.deltaTime * drawBackSpeed);
            pullBackDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y),pullBackStartPos);
            yield return null;
        }
        CueHit();
    }

    public void CueHit()
    {
        thisRB.AddForce(transform.up * (hitSpeed+ (pullBackDistance + 1)), ForceMode2D.Impulse);
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

        while (ball.thisRB.velocity.sqrMagnitude > ball.clampPoint) //keep yielding one frame until the ball stops, THEN set the stick position
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


