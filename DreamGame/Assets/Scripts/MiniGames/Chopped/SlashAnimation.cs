using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAnimation : MonoBehaviour
{
	/*****************
	CreateDate: 8/5/21  
	Functionality: this just plays a slash anmiation.  
	******************/
	
	//////////////////////////////Config
    public List<Sprite> animationSprites;
    
	[Range(0f,0.5f)] public float animationSpeed;
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
    private SpriteRenderer sr;
	
	

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(playAnimation());
    }


    public IEnumerator playAnimation()
    {
        for (int i =0; i < animationSprites.Count; i++)
        {
            sr.sprite = animationSprites[i];
            yield return new WaitForSeconds(animationSpeed);
        }
        sr.sprite = null;
        Destroy(this.gameObject);
    }

    [ContextMenu("play anim")]
    public void playAnimFromEditor()
    {
        StartCoroutine(playAnimation());
    }

}

 
