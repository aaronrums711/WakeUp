using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClockDisplay : MonoBehaviour
{
	/*****************
	CreateDate: 	2/12/22
	Functionality:	basic display class.  This will probably get finessed substantially in the future.
	Notes:
	Dependencies:
	******************/
	
	public TextMeshProUGUI clockText; 
	
	
	void Awake()
    {
        
    }
	
    void Start()
    {
        
    }

    void Update()
    {
        string newString = Clock.gameDateTime.Hour.ToString("00") + ":" + Clock.gameDateTime.Minute.ToString("00");
		clockText.text = newString;
    }
}
