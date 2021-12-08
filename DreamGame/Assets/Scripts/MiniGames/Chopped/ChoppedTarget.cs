using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppedTarget : MiniGameElement, IProgressionAdder
{
	/*****************
	CreateDate:  8/5/21
	Functionality:  the behavior of the chopped mini game targets
	******************/
	
	//////////////////////////////Config
    public int currentHealth;
    public int totalHealth;
    public int maxHealth;
    public int minHealth;
    public float sizeToHealthMultiplier;
    public float flashEffectCount;
    public float flashLength;
    public GameObject slashEffPrefab;
	public Vector3 startingScale;

     [Tooltip("this should be be < 0.02.  it controls how much additional progress is given for destroying larger targets versus smaller ones")] 
    public float progressionToSizeMultiplier;

	//////////////////////////////State
	private float startingYPos;
    [HideInInspector]public Vector2 velocityAtStop; //used by the stop/start script to store the velocity of each target before it's stopped, then to set it back again if necessary
    // [HideInInspector]public float  magnitudeAtSpeedChange; //used by stop/start script, but for the slow down method
    // [HideInInspector]public Vector2 velocityAtSpeedChange;  //same as above, but this is given a default value below in the Start() method for sake of the slow down method

	//////////////////////////////Cached Component References
    private SpriteRenderer thisSR;
    [HideInInspector]
    public Rigidbody2D rb;
    private ChoppedTargetSpawner spawner;
    public Sprite startingSprite;
    

	
	
	// void Awake()
    // {
    //     GetParentMiniGame();  //I have not idea why, but this is not getting called automatically like it's supposed to, so I'm calling it here. 
    // }
	
    void Start()
    {
        if (MiniGameElement.OnSpawnGameElement != null)
        {
            MiniGameElement.OnSpawnGameElement(this.gameObject);
        }
        startingScale = this.transform.localScale;
        SetHealthAndSize();
        currentHealth = totalHealth;
        thisSR = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spawner = GameObject.Find("LaunchPoints").GetComponent<ChoppedTargetSpawner>();
        startingSprite = thisSR.sprite;
        startingYPos = this.transform.position.y-0.25f;  //subtracting 0.25 just so there's no accidental self destruction right when it's spawned
        // velocityAtSpeedChange = rb.velocity;
    }

    void Update()
    {
        if (this.transform.position.y < startingYPos)
        {
            spawner.allTargets.Remove(this);
            Destroy(this.gameObject);
        }
    }

    public IEnumerator handleHit()
    {
        currentHealth--;
        Instantiate(slashEffPrefab, this.transform.position,Quaternion.Euler(0,0, Random.Range(0,359)));
        Vector2 velocityAtHit = rb.velocity;
        rb.velocity = Vector2.zero;
        StartCoroutine(SpriteFlash(thisSR));
        yield return StartCoroutine(FreezeVelocityForTime(0.15f, rb));
        rb.velocity = velocityAtHit;
        if (currentHealth <= 0)
        {
            if (MiniGameElement.OnDestroyGameElement != null)
            {
                MiniGameElement.OnDestroyGameElement(this.gameObject);
            }
            spawner.allTargets.RemoveAt(0);
            AddMiniGameProgress();
            Destroy(this.gameObject);
        }
    }

    public IEnumerator SpriteFlash(SpriteRenderer sr)
    {
        for (int i = 0; i < flashEffectCount; i++)
        {
            sr.sprite = null;
            yield return new WaitForSeconds(flashLength);
            sr.sprite = startingSprite;
            yield return new WaitForSeconds(flashLength);
        }
    }

    public IEnumerator FreezeVelocityForTime(float freezeTime, Rigidbody2D rbToFreeze)
    {
        float elapsed = 0f;
        while (elapsed < freezeTime)
        {
            rbToFreeze.velocity = Vector2.zero;
            elapsed += Time.deltaTime;
            yield return null;
        }

    }


    [ContextMenu("handle hit")]
    public void CallHandleHit()
    {
        StartCoroutine(handleHit());
    }

    private void SetHealthAndSize()
    {
        totalHealth = Random.Range(minHealth, maxHealth);
        //this logic assumes that there are only 3 options for health.  right now it's 1,2,3. 
        if (maxHealth - minHealth > 3) {print("there is a larger span of health than originally planned, you may want to look at this logic again");}
        if (totalHealth == minHealth)
        {
            this.transform.localScale = startingScale * (1+(-sizeToHealthMultiplier));
        }
        else if (totalHealth == minHealth + 1)
        {
            return;
        }
        else if( totalHealth == maxHealth -1)
        {
            this.transform.localScale = startingScale * (1+(sizeToHealthMultiplier));
        }
    }

    public void AddMiniGameProgress()
    {
        parentMiniGame.AddProgress((parentMiniGame.baseProgressionChunk + (totalHealth * progressionToSizeMultiplier))  * parentMiniGame.progressionParams.universalProgressionMultiplier);  
            //award more progress based on the target totalHealth
            //toggling the progressionToSizeMultiplier is one way to make this game harder
    }

}
