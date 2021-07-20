using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///this class is the parent class of all mini games, and contains shared behavior
public class MiniGame : MonoBehaviour
{

    ////////////////////////Config
    public Color baseColor;
    public Color targetColor;
    [Tooltip("will almost always be 0-1.  added room on each side to account for edge cases")]  
    [Range(-0.5f, 1.5f)] public float completionPercent;
    public float displayHeight;
    public float displayWidth;
    [Range(1,4)] public int orderInLevel;
    public enum skills
    {
        precision, rhythm, speed, attention, strength
    }
    [Range(0f, 0.5f)]  [Tooltip("rate in seconds that the mini game will lose progress.  This should be pretty close to 0")]  
    public float rateOfDecay;

     [Tooltip("with space at position 0, now any game can just use keysToPlay[orderInLevel]  to get the key that game is using")]
    public string [] keysToPlay = {"space", "r", "i", "v", "m" };
    public string keyForThisGame;

    //////////////////////////State
    public bool isActive;
    public bool isFrozen;
    public bool isComplete;
    public static int numActiveGames;


    //////////////////////////////Cached Component References
    private List<GameObject> playAreaBarriers;

    void Start()
    {
        keyForThisGame = keysToPlay[orderInLevel];  //orderInLevel will eventually be set by a manager class. For now, VGIU
        print("children of mini game: " + this.transform.childCount);
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
    }

    public void StopGame()
    {
        //stops the game. This will be ued when isComplete = true or when player loses
        return;
    }

    public void TrackColorWithCompletionPercent()
    {
        //changes the color of playArea surrounding a game to indicate completionPercent.  Need to decide if it's going to be from white to baseColor or baseColor to white. 
        return;
    }

    public float DecayCompletion()
    {
        //pushes back the completion percent over time. 
        completionPercent-= rateOfDecay;
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
        return isComplete;
    }

}
