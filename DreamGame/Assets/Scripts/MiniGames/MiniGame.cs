using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //need the ToList() function


///this class is the parent class of all mini games, and contains shared behavior
public class MiniGame : MonoBehaviour
{

    ////////////////////////Config
    public string miniGameName;
    public Color baseColor;
    public Color targetColor;
    [Tooltip("will almost always be 0-1.  added room on each side to account for edge cases")]  
    [Range(-0.2f, 1.2f)] public float completionPercent = 0.5f;
    public float baseProgression = 0.05f;
    [Tooltip("will only be used in some cases, where progression is in chunks instead of gradual")]  
    public float progressionMultiplier = 1;
    public float displayHeight;
    public float displayWidth;
    [Range(1,4)] public int orderInLevel;
    public enum skills
    {
        precision, rhythm, speed, attention, strength
    }
    [Range(0f, 0.5f)]  [Tooltip("rate in seconds that the mini game will lose progress.  This should be pretty close to 0")]  
    public float rateOfDecay= 0.01f;

     [Tooltip("with space at position 0, now any game can just use keysToPlay[orderInLevel]  to get the key that game is using")]
    public string [] keysToPlay = {"space", "r", "i", "v", "m" };
    public string keyForThisGame;


    //////////////////////////State
    public bool isActive;
    public bool isFrozen;
    public bool isComplete;
    public static int numActiveGames;
    public bool isTesting;


    //////////////////////////////Cached Component References
    public List<GameObject> playAreaBarriers = new List<GameObject>();

    void Start()
    {
        completionPercent = 0.5f;
        keyForThisGame = keysToPlay[orderInLevel];  //orderInLevel will eventually be set by a manager class. For now, VGIU
        getPlayAreaBarriers();
    }

    void Update()
    {
        if(isTesting == false && isComplete == false)
        {
            DecayCompletion();
        } 
        
        TrackColorWithCompletionPercent();
    }


    private void getPlayAreaBarriers()
    {
        GameObject barrierParent = new GameObject("test");
        for (int i=0; i< this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).gameObject.CompareTag("PlayAreaBarriers"))
            {
                barrierParent = this.transform.GetChild(i).gameObject;
                break;
            }
        }
        if (barrierParent.name == "test")
        {
            Debug.LogError("this mini game does not have a child with the PlayAreaBarriers tag.  The parent mini game must be at the top of a hierarchy, and it must have a direct child that has this tag");
            return;
        }

        for(int i=0; i< barrierParent.transform.childCount; i++)
        {
            playAreaBarriers.Add(barrierParent.transform.GetChild(i).gameObject);
        }
        Destroy(GameObject.Find("test"));
    }

    [ContextMenu("stop game")]
    public void StopGame()
    {
        print("StopGame() method has been called from " + this.name);
        List <Rigidbody2D> allRBs = GetComponentsInChildren<Rigidbody2D>().ToList();
        List <MonoBehaviour> allScripts = GetComponentsInChildren<MonoBehaviour>().ToList();
        allScripts.Remove(this); //this script, the MiniGame script, should not be in this list. 
        foreach (Rigidbody2D rb in allRBs)
        {
            rb.gravityScale = 0f;  //rigidbodies can't be disabled, so, doing this. 
            rb.bodyType = RigidbodyType2D.Static;
        }
        foreach (MonoBehaviour script in allScripts)
        {
            script.StopAllCoroutines();
            script.enabled = false;
            print(script.GetType());
        }
    }

    public void TrackColorWithCompletionPercent()
    {
        //changes the color of playArea surrounding a game to indicate completionPercent.  Need to decide if it's going to be from white to baseColor or baseColor to white. 
        for (int i =0; i< playAreaBarriers.Count; i++)
        {
            SpriteRenderer sr  = playAreaBarriers[i].GetComponent<SpriteRenderer>();
            sr.color = Color.Lerp(baseColor, targetColor, completionPercent);
        }

    }

    public float DecayCompletion()
    {
        //pushes back the completion percent over time. 
        if (completionPercent >0)
        {
            completionPercent-= rateOfDecay*Time.deltaTime;
        }
        else if(completionPercent<0)
        {
            completionPercent = 0;
        }
        return completionPercent;   
    }

    public float AddProgress(float additionalProgress)
    {
        //adds additionalProgress to completionPercent
        completionPercent += additionalProgress;
        CheckCompletion();
        return completionPercent;
    }

    public bool CheckCompletion()
    {
        //check completionPercent and modify isComplete
        if (completionPercent >= 1)
        {
            isComplete = true;
        }
        else 
        {
            isComplete = false;
        }
        if(isComplete)
        {
            StopGame();
        }
        return isComplete;
    }

    public void ApplyColor(SpriteRenderer sr, float percent)
    {
        sr.color = Color.Lerp(baseColor, targetColor, percent);
    }

    public void ApplyColor(List<SpriteRenderer> AllSRs, float percent)
    {
        foreach(SpriteRenderer sr in AllSRs)
        {
            sr.color = Color.Lerp(baseColor, targetColor, percent);
        }
    }


}


