using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;


public class InkStoryController_V2 : MonoBehaviour
{
	//Config
	private bool insertNewLine = false;
	private List<string> playerTextList;
	private List<string> partnerTextList;

	private List<string> masterTextList;

	[SerializeField]private float timeBetweenSpeakers= 1.5f;
	[SerializeField]private float waitTime = 2f;
	[Tooltip("smaller number = faster text")]  [Range(0.05f, 0.0001f)]
	public float textDisplaySpeed = 0.05f;
	private OrderedDictionary masterTextDict;

	//State
	private string speaker = "";
	private string playerTag = "pl";
	private string partnerTag = "part";
	private string waitTag = "wait";
	private string newLineTag = "nl";
	private string refreshTextTag = "refresh";
	private string fullRefreshTag = "fullrefresh";
	private bool playerShouldWait = false;
	private bool partnerShouldWait = false;
	private string firstSpeaker;  //this will be the FIRST person to speak in a conversation before a choice is given.  For example, in the very first set of choices, the partner is the firstSpeaker. 
								  //before using this, the player's text would always show first, even if the player was meant to be responding to a question from the partner. 
								  //currently, this logic DOES NOT support multiple back and forths between the two speakers before a choice is given.  
								  //given value in CheckTags() and nulled out in ChooseStoryChoice() so that it can be given a new value
	[SerializeField] private bool isTesting = false;
	private bool playerShouldRefresh;
	private bool partnerShouldRefresh;	
	private bool shouldFullRefresh;
	
	//Cached Component References
	public TextAsset inkJSON;
	private Story story;
	[SerializeField] private Text textPrefab;
	[SerializeField] private Button buttonPrefab;
	[SerializeField] private TextDisplay playerTextDisplay;
	[SerializeField] private TextDisplay partnerTextDisplay;
	[SerializeField] private Canvas buttonCanvas;

	void Awake()
	{
		speaker = playerTag;
		if (isTesting)
		{
			timeBetweenSpeakers =0.5f;
			waitTime = 0.5f;
			textDisplaySpeed = 0.00001f;
		}
	}
	
    void Start()
    {
	    story = new Story(inkJSON.text);
	    wipeTextAndButtons();
	    // ParseStoryText();
		CreateMasterTextList();
    }

    void Update()
    {
        
    }
	
	
	private string CheckTags()
	{
		//set speaker
		if (story.currentTags.Contains(playerTag)) { speaker = playerTag; }
		else if (story.currentTags.Contains(partnerTag)) { speaker = partnerTag; }

		//set shouldWait
		if (story.currentTags.Contains(waitTag) && speaker == playerTag) { playerShouldWait = true; }
		else if (story.currentTags.Contains(waitTag) && speaker == partnerTag) { partnerShouldWait = true; }

		//set insertNewLine
		if (story.currentTags.Contains(newLineTag)) {insertNewLine = true;}
		
		//set player/partnerShouldRefresh
		if (story.currentTags.Contains(refreshTextTag) && speaker == partnerTag) {partnerShouldRefresh = true;}
		else if  (story.currentTags.Contains(refreshTextTag) && speaker == playerTag) {playerShouldRefresh = true;}
		
		//set firstSpeaker
		if (String.IsNullOrEmpty(firstSpeaker)) {firstSpeaker = speaker;}

		//set shouldFullRefresh
		if (story.currentTags.Contains(fullRefreshTag)) {shouldFullRefresh = true;}
		return speaker;
	}
	

	
	void ChooseStoryChoice(Choice choice)
	{
		story.ChooseChoiceIndex(choice.index);
		wipeTextAndButtons();
		// ParseStoryText();
		CreateMasterTextList();
		firstSpeaker = "";
	}
	
	void DisplayChoices()
	{
		foreach (Choice choice in story.currentChoices)
		{
			Button choiceButton = Instantiate(buttonPrefab) as Button;
			choiceButton.transform.SetParent(buttonCanvas.transform, false);
			choiceButton.GetComponentInChildren<Text>().text = choice.text;
			choiceButton.onClick.AddListener(delegate
				{
					ChooseStoryChoice(choice);
				}
			);
		}
	}
	
	void ParseStoryText()
	{
		string player = "";
		string other = "";
		playerTextList = new List<string>();
		partnerTextList = new List<string>();
		while (story.canContinue)
		{
			string textChunk = story.Continue();
			CheckTags();
			if (speaker == playerTag)
			{
				player += textChunk;
				if (playerShouldWait) //logic: we need to add the textChunk to the latest list item, THEN create another blank item, so the pause will//happen AFTER the text with #wait tag is displayed
				{
					if (playerTextList.Count == 0)
					{
						playerTextList.Add(textChunk);
						playerTextList.Add("");
					}
					else if (playerTextList.Count > 0)
					{
						int max = playerTextList.Count;
						playerTextList[max-1] += textChunk;
						playerTextList.Add("");
					}
					playerShouldWait = false;
				}
				else  //logic: if list.count = 0, set the 0th item.  If list.count>0, then add the textChunk to the LATEST item in the list this is good if there are MULTIPLE waits within one person's uninterupted speech. 
				{
					if (playerTextList.Count == 0)
					{
						playerTextList.Add(textChunk);
					}
					else if (playerTextList.Count > 0)
					{
						int max = playerTextList.Count;
						playerTextList[max-1] += textChunk;
					}
				}
				if (insertNewLine)
				{
					player += "\n";
					int max = playerTextList.Count;
					playerTextList[max-1] += "\n";
					insertNewLine = false;
				}
			}
			else if (speaker == partnerTag)
			{
				other += textChunk;
				if (partnerShouldWait)
				{
					if (partnerTextList.Count == 0)
					{
						partnerTextList.Add(textChunk);
						partnerTextList.Add("");
					}
					else if (partnerTextList.Count > 0)
					{
						int max = partnerTextList.Count;
						partnerTextList[max-1] += textChunk;
						partnerTextList.Add("");
					}
					partnerShouldWait = false;
				}
				else   
				{
					if (partnerTextList.Count == 0)
					{
						partnerTextList.Add(textChunk);
					}
					else if (partnerTextList.Count > 0)
					{
						int max = partnerTextList.Count;
						partnerTextList[max-1] += textChunk;
					}
				}
				if (insertNewLine)
				{
					other += "\n";
					int max = partnerTextList.Count;
					partnerTextList[max-1] += "\n";
					insertNewLine = false;
				}
			}
			else
			{
				print("text displayed through debug, something is wrong, there may be no partner OR speaker tag" + story.Continue());
			}
		}
		StartCoroutine(DisplayLoop());
	}

	
	private void wipeTextAndButtons()
	{
		playerTextDisplay.DeleteAllText();
		partnerTextDisplay.DeleteAllText();
		
		for (int i = 0; i < buttonCanvas.transform.childCount; i++)
		{
			Destroy(buttonCanvas.transform.GetChild(i).gameObject);
		}
	}
	
	private IEnumerator DisplayLoop()  //params may be options for this new version...
	{
		if (firstSpeaker == playerTag)
		{
			for (int i = 0; i <= playerTextList.Count - 1; i++)
			{
				yield return StartCoroutine(playerTextDisplay.TextLetterByLetter(playerTextList[i], textDisplaySpeed ));
				yield return new WaitForSeconds(waitTime);
				if (playerShouldRefresh)
				{
					playerTextDisplay.DeleteAllText();
					playerShouldRefresh = false;
				}
			}

			yield return new WaitForSeconds(timeBetweenSpeakers);
		
			for (int i = 0; i <= partnerTextList.Count - 1; i++)
			{
				yield return StartCoroutine(partnerTextDisplay.TextLetterByLetter(partnerTextList[i], textDisplaySpeed));
				yield return new WaitForSeconds(waitTime);
				if (partnerShouldRefresh)
				{
					partnerTextDisplay.DeleteAllText();
					partnerShouldRefresh = false;
				}
			}
		}
		else if (firstSpeaker == partnerTag)
		{
			for (int i = 0; i <= partnerTextList.Count - 1; i++)
			{
				yield return StartCoroutine(partnerTextDisplay.TextLetterByLetter(partnerTextList[i], textDisplaySpeed));
				yield return new WaitForSeconds(waitTime);
				if (partnerShouldRefresh)
				{
					partnerTextDisplay.DeleteAllText();
					partnerShouldRefresh = false;
				}
			}

			yield return new WaitForSeconds(timeBetweenSpeakers);
		
			for (int i = 0; i <= playerTextList.Count - 1; i++)
			{
				yield return StartCoroutine(playerTextDisplay.TextLetterByLetter(playerTextList[i], textDisplaySpeed));
				yield return new WaitForSeconds(waitTime);
				if (playerShouldRefresh)
				{
					playerTextDisplay.DeleteAllText();
					playerShouldRefresh = false;
				}
			}
		}
		else { print("something is wrong in the DisplayLoop function, or with the firstSpeaker assignment logic");}

		DisplayChoices();
	}
	


	//initial attempt at displaying partner and player text.  This worked, but couldn't handle pauses within one person's speech.
	// private IEnumerator DisplayDelay(string text1, string text2, float delay = 1f)  //the last param syntax means that its optional, and 2f is used if nothing else is passed in
	// {
	// 	yield return StartCoroutine(playerTextDisplay.TextLetterByLetter(text1));
	// 	yield return new WaitForSeconds(delay);
	// 	yield return StartCoroutine(partnerTextDisplay.TextLetterByLetter(text2));  
	// 	playerShouldWait = false;
	// 	yield return new WaitForSeconds(delay/2);
	// 	DisplayChoices();
	// }
	
	/**
	7/7/21
		okay, third iteration of this.  Now I want it to be able to correctly display a back and forth conversation between
		two speakers, even if there isn't a choice.  Currently, it cannot do this, and instead it just displays all the text 
		from one speaker 1, then all the text from speaker 2, then the choice, even if it's written as Speaker 1, speaker 2, speaker 1
		speaker 2, choice.  

		logic can be as follows:
			one master list is created that is then passed into the actual display method. 
			this list contains one element for each chunk of story text for each speaker. 
			so....
				speaker 1: hey
				speaker 2: hello!
				speaker 1: how are you?
				speaker 2: great!
			would contain 4 elements

			only the firstSpeaker needs to be used in logic, because after that the text in the list will just 
			be displayed alternating until there is no elements left. 

			so if firstSpeaker = speaker 2, then we know to display the first element as speaker 2, then the secondd as speaker 1,
			then the third as speaker 2 and so on. 
	**/
	public void CreateMasterTextList()
	{
		int counter = 0;  //this is used to make the dictionary keys unique.  it essentially counts the unique speech chunks per speaker until there is a choice
		masterTextList = new List<string>();
		masterTextDict = new OrderedDictionary();
		string currentSpeaker = "";
		string previousSpeaker = "";
		while (story.canContinue)
		{
			string textChunk = story.Continue();
			CheckTags();
			
			 //this if statement is only necessary for the first loop iteration, because there will be no currentSpeaker, and we don't want to leave it as a "" because it gets compared against the currentSpeaker
			previousSpeaker = (string.IsNullOrEmpty(currentSpeaker)) ? speaker : currentSpeaker;
			currentSpeaker = speaker;  

			if (masterTextList.Count == 0) {masterTextList.Add("");} //no matter what, if it's the first piece of text, add an element

			if(currentSpeaker == previousSpeaker) //add to the latest element
			{
				int max = masterTextList.Count;
				masterTextList[max-1] += textChunk;
			}
			
			else if(currentSpeaker != previousSpeaker) //create a new element and add to that
			{
				masterTextList.Add("");
				int max = masterTextList.Count;
				masterTextList[max-1] += textChunk;
			}

			///////////////dictionary part/////////////////////
			/**
			logic: I think we can keep the previous/current speaker logic...
					so if the currentSpeaker != previousSpeaker, make a new element, else add it to the latest element
					however, we also need to add a new element if there is a player/partnerShouldWait = true;
			**/
			if (masterTextDict.Count == 0) //adding an empty element for the first iteration
			{
				masterTextDict.Add(currentSpeaker+ counter.ToString(), "");
				counter++;
			}

			if(currentSpeaker == previousSpeaker) //we only need to add a new element if there is a wait tag.  if not, just add the text to the latest element. 
			{
				int max = masterTextDict.Count;
				masterTextDict[max-1] += textChunk;
				if(insertNewLine)
				{
					masterTextDict[max-1] += "\n";
					insertNewLine = false;
				}

				//check for wait tags
				if (playerShouldWait && currentSpeaker == playerTag)
				{
					masterTextDict.Add(currentSpeaker+ counter.ToString(), "");
					playerShouldWait = false;
					counter++;
				}
				if (partnerShouldWait && currentSpeaker == partnerTag)
				{
					masterTextDict.Add(currentSpeaker+ counter.ToString(), "");
					partnerShouldWait = false;
					counter++;
				}

				//check for refresh tags.  these are handled in the display method
				if (playerShouldRefresh && currentSpeaker == playerTag)
				{
					masterTextDict[max-1] += " #" + refreshTextTag;
					playerShouldRefresh = false;
					counter++;
				}
				if (partnerShouldRefresh && currentSpeaker == partnerTag)
				{
					masterTextDict[max-1] += " #" + refreshTextTag;
					partnerShouldRefresh = false;
					counter++;
				}
			}
			
			else if(currentSpeaker != previousSpeaker) 
			{	//always add an element for a new speaker
				masterTextDict.Add(currentSpeaker+ counter.ToString(), "");
				int max = masterTextDict.Count;
				masterTextDict[max-1] += textChunk;
				if(insertNewLine)
				{
					masterTextDict[max-1] += "\n";
					insertNewLine = false;
				}
				counter++;

				//need to check for a wait tag again, because sometimes the first line of a new speaker may also contain a wait tag. 
				//so in that case we will add an element for the new speaker, add the text to it, then add ANOTHER new element because that line has a #wait tag
				if (playerShouldWait && currentSpeaker == playerTag)
				{
					masterTextDict.Add(currentSpeaker+ counter.ToString(), "");
					playerShouldWait = false;
					counter++;
				}
				if (partnerShouldWait && currentSpeaker == partnerTag)
				{
					masterTextDict.Add(currentSpeaker+ counter.ToString(), "");
					partnerShouldWait = false;
					counter++;
				}

				//check for refresh tags.  these are handled in the display method
				if (playerShouldRefresh && currentSpeaker == playerTag)
				{
					masterTextDict[max-1] += " #" + refreshTextTag;
					playerShouldRefresh = false;
					counter++;
				}
				if (partnerShouldRefresh && currentSpeaker == partnerTag)
				{
					masterTextDict[max-1] += " #" + refreshTextTag;
					partnerShouldRefresh = false;
					counter++;
				}
			}
		}

		print("dictionary output below:");
		foreach (DictionaryEntry de in masterTextDict)
			{
				print("key: " +de.Key.ToString()    + "  value: " + de.Value.ToString());
			}

		StartCoroutine(DisplayTextFromMasterDict());

		//calls the old logic using the list
		// if (firstSpeaker == playerTag)
		// {
		// 	StartCoroutine(DisplayTextFromMasterList(playerTextDisplay, partnerTextDisplay));
		// }
		// else if (firstSpeaker == partnerTag)
		// {
		// 	StartCoroutine(DisplayTextFromMasterList(partnerTextDisplay, playerTextDisplay));
		// }
		
	}

	public IEnumerator DisplayTextFromMasterList(TextDisplay firstTextDisplay, TextDisplay secondTextDisplay)
	{
		
		for (int i = 0; i <= masterTextList.Count - 1; i++)
			{	//+10 is arbitrary, we are just doing it so we don't have to deal with 0%2 or 1%2.  Any even number + 10 is still even, any odd number + 10 is still odd
				if((i + 10)%2 == 0) 
				{	
					string newLineString ="";
					if (i >= 2) {newLineString = "\n \n";}
					
					yield return StartCoroutine(firstTextDisplay.TextLetterByLetter(newLineString + masterTextList[i], textDisplaySpeed));
					yield return new WaitForSeconds(timeBetweenSpeakers);
				}
				else if((i + 10)%2 == 1)
				{
					string newLineString ="";
					if (i >= 2) {newLineString = "\n \n";}
					yield return StartCoroutine(secondTextDisplay.TextLetterByLetter(newLineString + masterTextList[i], textDisplaySpeed));
					yield return new WaitForSeconds(timeBetweenSpeakers);
				} 
			}

		DisplayChoices();
	}

	public IEnumerator DisplayTextFromMasterDict()
	{
		//the below logic for new line string basically adds two new lines after the first chunk that a person speaks
		//if a section of content is just Player -> Partner -> choices, as is common, then no new lines will be added
		//visually, this should make it more obvious that a conversation is happening between two people, because accros the screen the text will have a diaganol shape
		string newLineString ="";
		int playerSpeechChunks = 0;
		int PartnerSpeechChunks = 0;
		foreach (DictionaryEntry de in masterTextDict)
		{
			if (de.Key.ToString().Contains(playerTag))
			{
				if (playerSpeechChunks >= 1) {newLineString = "\n \n";}
				yield return StartCoroutine(playerTextDisplay.TextLetterByLetter(newLineString + de.Value.ToString().Replace("#refresh", ""	), textDisplaySpeed));
				yield return new WaitForSeconds(timeBetweenSpeakers);
				playerSpeechChunks++;
				newLineString ="";
				if (de.Value.ToString().Contains(refreshTextTag)) {playerTextDisplay.DeleteAllText();}
				
			}

			if (de.Key.ToString().Contains(partnerTag))
			{
				if (PartnerSpeechChunks >= 1) {newLineString = "\n \n";}
				yield return StartCoroutine(partnerTextDisplay.TextLetterByLetter( newLineString + de.Value.ToString().Replace("#refresh", ""	), textDisplaySpeed));
				yield return new WaitForSeconds(timeBetweenSpeakers);
				PartnerSpeechChunks++;
				newLineString ="";
				if (de.Value.ToString().Contains(refreshTextTag)) {partnerTextDisplay.DeleteAllText();}
			}
		}

		DisplayChoices();
		
	}


}
