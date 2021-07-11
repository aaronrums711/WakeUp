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
	private string firstSpeaker;  //first person to speak between each selection
	[SerializeField] private bool isTesting = false;
	private bool playerShouldRefresh;
	private bool partnerShouldRefresh;	
	private bool shouldFullRefresh;
	
	//Cached Component References
	public TextAsset inkJSON;
	private Story story;
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
		ParseStoryToDict();
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
		ParseStoryToDict();
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
	
	private void wipeTextAndButtons()
	{
		playerTextDisplay.DeleteAllText();
		partnerTextDisplay.DeleteAllText();
		
		for (int i = 0; i < buttonCanvas.transform.childCount; i++)
		{
			Destroy(buttonCanvas.transform.GetChild(i).gameObject);
		}
	}
	
	public void ParseStoryToDict()
	{
		int counter = 0;  //this is used to make the dictionary keys unique.  it essentially counts the unique speech chunks per speaker until there is a choice
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

		//uncomment this to see the final masterTextDict output
		// print("dictionary output below:");
		// foreach (DictionaryEntry de in masterTextDict)
		// 	{
		// 		print("key: " +de.Key.ToString()    + "  value: " + de.Value.ToString());
		// 	}

		StartCoroutine(DisplayTextFromMasterDict());
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
