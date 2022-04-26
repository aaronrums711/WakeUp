using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
	/**
	this script should be attached to each canvas that displays text.  That way, the ink parsing script can simply call Display(textToPassIn) and it will get displayed. 
	Additionally, this will allow us to easily display other text as well, since the parsing and the display will be decoupled. 
	**/

	//Config
	public  float textSpeed = 0.03f;
	[SerializeField] private AudioClip textSoundClip;
	
	//4 below variables were going to be used for the display effect until I realized it would be alot harder than I thought
	// private int initLetterSize;
	// private int letterEffectSize;
	// private int letterEffectGrowth = 5;
	// private float letterEffectTotalTime = 2f;

	//State
	
	//Cached Component References
	[SerializeField] private Text textPrefab;
	private Text textBox;
	private AudioSource audioSource;

	void Awake()
	{
		audioSource = FindObjectOfType<AudioSource>();
		audioSource.clip = textSoundClip;
		if (GetComponentInChildren<Text>() == null)
		{
			textBox = Instantiate(textPrefab);
			textBox.transform.SetParent(this.transform, false);
		}
		else
		{
			textBox = GetComponentInChildren<Text>();
		}

		textBox.text = "";
	}
	

    public IEnumerator TextLetterByLetter(string text, float speed = 0.0001f)
    {
	    audioSource.Play();
	    for (int i = 0; i <= text.Length-1; i++)
	    {
		    textBox.text += text[i];
		    yield return new WaitForSeconds(speed);
	    }

	    audioSource.Pause();
    }

    public void DeleteAllText()
    {
	    textBox.text = "";
    }
    
    //notes for the fade-in text effect. 
    //pass each letter into a lerp function
    //logic of lerp function:
    /*
     *letterSize = Lerp(StartSize, EndSize, (effectElapsedTime/totalTimeForLetterEffect));
     */


}
