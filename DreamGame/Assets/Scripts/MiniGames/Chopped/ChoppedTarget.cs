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
    }

    void Update()
    {
        
    }

    public IEnumerator handleHit()
    {
        Instantiate(slashEffPrefab, this.transform.position,Quaternion.Euler(0,0, Random.Range(0,359)));
        Vector2 velocityAtHit = rb.velocity;
        rb.velocity = Vector2.zero;
        yield return StartCoroutine(SpriteFlash(thisSR));
        rb.velocity = velocityAtHit;
        if (currentHealth <= 0)
        {
            spawner.allTargets.RemoveAt(0);
            Destroy(this.gameObject);
        }
    }

    public IEnumerator SpriteFlash(SpriteRenderer sr)
    {
        currentHealth--;
        Sprite startingSprite = sr.sprite;
        for (int i = 0; i < flashEffectCount; i++)
        {
            sr.sprite = null;
            yield return new WaitForSeconds(flashLength);
            sr.sprite = startingSprite;
        }

    }

    [ContextMenu("handle hit")]
    public void CallHandleHit()
    {
        StartCoroutine(handleHit());
    }
}
