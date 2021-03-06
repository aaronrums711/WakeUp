using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //need the ToList() function


///this class is the parent class of all mini games, and contains shared behavior
public class MiniGame : MonoBehaviour
{

    ////////////////////////Config

    [Header("basic info")]
    public string miniGameName;
    [Tooltip("this is not difficulty.  This represents the level of complexity that this mini game has.  Roughly, how much attention or skill does it take to complete?")]  
    public InherentComplexity inherentComplexity; 
    public Color baseColor;
    public Color targetColor;
    [Tooltip("will almost always be 0-1.  added room on each side to account for edge cases")]  
    [Range(-0.2f, 1.2f)] public float completionPercent = 0.5f;
    

    [Header("progression")]
    [Tooltip("base progression.  use this for games that have gradual PER SECOND progression")]  
    public float baseProgression = 0.05f;
    [Tooltip("this should be used in games where progression is in chunks, not per second")]  
    public float baseProgressionChunk = 0.05f;
    public float progressionMultiplier = 1;
    [Range(0f, 0.5f)]  [Tooltip("rate in seconds that the mini game will lose progress.  This should be pretty close to 0")]  
    public float rateOfDecay= 0.01f;

 
    [Header("input")]
    [Tooltip("with space at position 0, now any game can just use keysToPlay[orderInLevel]  to get the key that game is using")]
    public string [] keysToPlay = {"space", "r", "i", "v", "m" };
    public string keyForThisGame;
    [Range(1,4)] public int orderInLevel;

    [Header("level parameters")]
    public Level level;  //I'm not 100% sure if the mini game needs a reference to the level...but I'll leave it here for now.  
    public ProgressionParams progressionParams;
    public DifficultyParams difficultyParams;


    //////////////////////////State
    [Header("game state")]
    public bool isActive;
    public bool isFrozen;
    public bool isComplete;
    //public static int numActiveGames; don't think this is used anywhere, and doesn't really make sense to be here
    public bool isTesting;

   

    //////////////////////////////Cached Component References
    [Header("display")]
    public float displayHeight;
    public float displayWidth;
    public List<GameObject> playAreaBarriers = new List<GameObject>();

    public IStoppable stopStarter;  //this is not showing in the inspector, I don't know why. 
    
    

    void Awake()
    {
        if ((this.progressionParams == null || this.difficultyParams == null) && isTesting == true)
        {
            Debug.LogWarning(this.miniGameName + " has assigned it's own progression or difficulty params.  This should only happen for testing purposes.");
            AssignProgressionParameters();
            AssignDifficultyParameters();
        }



        stopStarter = GetComponentInChildren<IStoppable>();
        isActive = true; //switched this to Awake from Start()  on 10/24/21.  I don't think this will cause any issue, but juse be aware. 

    }

    void Start()
    {
        rateOfDecay *= progressionParams.universalDragMultiplier;  //whether the diff/prog params as assigned from the levelManager or above, we are setting the relevant values here in Start()
        baseProgression *=  progressionParams.universalProgressionMultiplier;
        baseProgressionChunk *= progressionParams.universalProgressionChunkMultiplier;

        keyForThisGame = keysToPlay[orderInLevel];  
        getPlayAreaBarriers();
        completionPercent = progressionParams != null ? progressionParams.startingProgression : 0.5f;

        print("the key for " + this.miniGameName + " is " + this.keyForThisGame);
    }

    void Update()
    {
        if(isTesting == false && isActive == true)
        {
            DecayCompletion();
        } 
        
        // TrackColorWithCompletionPercent();  //commented out on 12/1/21
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
            completionPercent-= (rateOfDecay*Time.deltaTime);
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
            stopStarter.StopMiniGame();
            print(this.miniGameName + "HAS BEEN BEATEN!!!");
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

    public void AssignProgressionParameters()
    {

        Object[] allProgressionParams = Resources.LoadAll("", typeof(ProgressionParams));

        foreach (ProgressionParams p in allProgressionParams)
        {
            if (p.difficultyDescription == this.level.generalDifficulty)
            {
                this.progressionParams = p;
            }
        }

        if (progressionParams != null)
        {
            rateOfDecay *= progressionParams.universalDragMultiplier;
            baseProgression *=  progressionParams.universalProgressionMultiplier;
            baseProgressionChunk *= progressionParams.universalProgressionChunkMultiplier;
        }
        else
        {
            Debug.LogError("there was no  matching ProgressionParameter object found.  Something is wrong");
            return;
        }
    }

    public void AssignDifficultyParameters()
    {
        Object[] allDifficultyParams = Resources.LoadAll("", typeof(DifficultyParams));
        
        foreach (DifficultyParams p in allDifficultyParams)
        {
            if (p.difficultyDescription == this.level.generalDifficulty)
            {
                this.difficultyParams = p;
                break;
            }
        }

        if (this.difficultyParams == null)
        {
            Debug.LogError("there was no  matching difficultyParameters object found.  Something is wrong");
        }

    }



}


