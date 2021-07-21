using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameElement : MonoBehaviour
{
    public MiniGame parentMiniGame;

    void Awake()
    {
        GetParentMiniGame();
    }

    public void GetParentMiniGame()
    {
        parentMiniGame = GetComponentInParent<MiniGame>();
        if (parentMiniGame == null)
        {
            Debug.LogError("this game element doesn't have a parent MiniGame that it belongs to.  Something is wrong");
        }
    }

}
