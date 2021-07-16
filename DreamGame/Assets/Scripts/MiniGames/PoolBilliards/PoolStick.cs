using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolStick : MonoBehaviour
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

    //////////////////////////////State
    public bool isDrawingBack = false;

    //////////////////////////////Cached Component References
    public BilliardsBall ball;
    public Rigidbody2D thisRB;
    [SerializeField] private Collider2D cueCollider; //VGIU
    public MiniGame parentMiniGame;



    void Start()
    {
        parentMiniGame = GetComponentInParent<MiniGame>();
        if (parentMiniGame != null) {print("you've got the right mini game: " + parentMiniGame.name);}
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

        Debug.Log("final pull back distance: " + pullBackDistance);
      
        CueHit();
    }


    public void CueHit()
    {

        thisRB.AddForce(transform.up * (hitSpeed+ (pullBackDistance + 1)), ForceMode2D.Impulse);
        print(thisRB.velocity);
        //stick will draw back slightly after hit.
        //stick will fade away
        //stick will reappear when ball stops moving. 
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Ball")
        {
            thisRB.velocity = Vector2.zero;
            thisRB.angularVelocity = 0;
            cueCollider.enabled = false;
            StartCoroutine(ResetStickPos());
        }
    }

    private IEnumerator ResetStickPos()
    {
        while (ball.thisRB.velocity.sqrMagnitude > ball.clampPoint) //keep yielding one frame until the ball stops, THEN set the stick position
        {
            yield return null;
        }
        // yield return new WaitForSeconds(3);
        this.transform.position = ball.transform.position + new Vector3(2,0,0);
        this.transform.rotation = startingRot;
        isDrawingBack = false;
        cueCollider.enabled = true;
    }

}
