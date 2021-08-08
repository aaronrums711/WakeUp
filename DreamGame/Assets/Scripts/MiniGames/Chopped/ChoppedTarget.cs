using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppedTarget : MiniGameElement
{
	/*****************
	CreateDate:  8/5/21
	Functionality:  the behavior of the chopped mini game targets
	******************/
	
	//////////////////////////////Config
    public int currentHealth;
    public int maxHealth;
    public float flashEffectCount;
    public float flashLength;
    public GameObject slashEffPrefab;
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
    private SpriteRenderer thisSR;
    private Rigidbody2D rb;
    private ChoppedTargetSpawner spawner;
    public Sprite startingSprite;

	
	
	void Awake()
    {
        GetParentMiniGame();  //I have not idea why, but this is not getting called automatically like it's supposed to, so I'm calling it here. 
    }
	
    void Start()
    {
        currentHealth = maxHealth;
        thisSR = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spawner = GameObject.Find("LaunchPoints").GetComponent<ChoppedTargetSpawner>();
        startingSprite = thisSR.sprite;
    }

    void Update()
    {
        
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
            spawner.allTargets.RemoveAt(0);
            parentMiniGame.AddProgress(parentMiniGame.baseProgression * parentMiniGame.progressionMultiplier);
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
}
