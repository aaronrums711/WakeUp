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

    //with space at position 0, now any game can just use keysToPlay[orderInLevel]  to get the key that game is using
    string [] keysToPlay = {"space", "r", "i", "v", "m" };


    //////////////////////////State
    public bool isActive;
    public bool isFrozen;
    public bool isComplete;
    public int numActiveGames;



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
