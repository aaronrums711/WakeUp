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
        string timeOnly = Clock.gameDateTime.ToString("t");
		clockText.text = timeOnly;
    }
}
