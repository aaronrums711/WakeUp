using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	/*****************
	CreateDate: 	9/21/21	
	Functionality:	observes all of the mini games in the level, as well as the level timer, and determines if/when the level is beaten.   There will be children of this class that handle level-type specific tasks, that are NOT universal between all level types
	Notes:			I think all level manager scripts will be on this one game object at all time, but only one will be active to match the level that is being played. 
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public Level thisLevel;
	
	[Tooltip("having these on the parent LevelManager class assumes that all games in all level types will use the same progressionParams and difficultyParams.  This seems likely, and any cases where it's not true can just be dealt with in the child script ")]
	public ProgressionParams progressionParams;
    public DifficultyParams difficultyParams;
	
	public int gamesNeededToWin;
	public int currentGamesWon;
	[HideInInspector] protected int executionInterval= 5;

	[HideInInspector] public Color additiveColor = new Color(197,197,197,0);

	//////////////////////////////State
	public bool isLevelFinished = false;
	public bool isLevelWon = false;

	
	//////////////////////////////Cached Component References
	 [Tooltip("this is simply ALL games that will appear in THIS particular level at any time. ")]  
	public List<MiniGame>  allGamesForThisLevel;
	public List<MiniGame>  gamesActiveInThisLevel;
	public Transform activeMiniGameParent;

	

	public void CheckAllMiniGames()
	{
		currentGamesWon = 0;
		foreach (MiniGame mg in gamesActiveInThisLevel)
		{
			if (mg.isComplete)
			{
				currentGamesWon++;
			}
		}


		if (currentGamesWon >= gamesNeededToWin)
		{
			isLevelWon = true;
			isLevelFinished = true;
			print("you've beaten the level, congratulations!");
		}


	}

	[ContextMenu("stop all games")]
	public void StopAllMiniGames()
	{

		for  (int i =0; i< activeMiniGameParent.childCount; i++)
		{
			activeMiniGameParent.GetChild(i).GetComponent<IStoppable>().StopMiniGame();
		}
	}

	[ContextMenu("restart all games")]
	public void RestartAllMiniGames()
	{
		for  (int i =0; i< activeMiniGameParent.childCount; i++)
		{
			activeMiniGameParent.GetChild(i).GetComponent<IStoppable>().RestartMiniGame();
		}
	}

	//in the future, another parameter could be repeatThreshold.  For levels that have a higher number of total games, we may want to allow more than one instance of the same game in a level.  
	//this method loads a list of mini games that will actually be played IN THIS LEVEL.  
	public List<MiniGame> LoadMiniGames(int numGamesToLoad)
	{
		List<MiniGame> allAvailableGames = new List<MiniGame>();

		Transform miniGamesParent = GameObject.Find("AllMiniGames").GetComponent<Transform>();
		for (int i = 0; i < miniGamesParent.transform.childCount; i++)
		{
			allAvailableGames.Add(miniGamesParent.GetChild(i).GetComponent<MiniGame>());
		}

		for (int i = 0; i < thisLevel.totalGamesInLevel; i++)
		{
			int randInt = Random.Range(0, allAvailableGames.Count);
			allGamesForThisLevel.Add(allAvailableGames[randInt]);
			allAvailableGames.Remove(allAvailableGames[randInt]);
		}

		return allGamesForThisLevel;
	}

	///loads games for this level, sets gamesNeededToWin, sets the parent for all spawned mini games, possibly more stuff in future
	public void SetUpLevel()
	{
		LoadMiniGames(thisLevel.totalGamesInLevel);
        gamesNeededToWin = thisLevel.minGamesNeededToWin;
		activeMiniGameParent = GameObject.Find("ActiveMiniGames").GetComponent<Transform>();

	}

	
    public void AssignProgressionParameters()
    {

        Object[] allProgressionParams = Resources.LoadAll("", typeof(ProgressionParams));

        foreach (ProgressionParams p in allProgressionParams)
        {
            if (p.difficultyDescription ==  thisLevel.generalDifficulty)
            {
                this.progressionParams = p;
            }
        }

        if (progressionParams = null)
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
            if (p.difficultyDescription == thisLevel.generalDifficulty)
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

	public void AssignProgressionMetric(MiniGame mg)
	{
		mg.rateOfDecay *= progressionParams.universalDragMultiplier;
        mg.baseProgression *=  progressionParams.universalProgressionMultiplier;
        mg.baseProgressionChunk *= progressionParams.universalProgressionChunkMultiplier;
	}

	//this method uses a colorToAdd and adds it to each pre-existing color to get the target color, instead of just lerping all colors to a single constant color.  ColorToAdd should be close to white if the goal is to lighten everything
	public IEnumerator FadeOut(List<SpriteRenderer> renderers, float duration, Color colorToAdd)
    {
		float startTime = Time.time;
		float totalTime = duration;
		float elapsed = 0f;
        List<Color> newColors = new List<Color>();
		List<Color> initColors = new List<Color>();
		foreach(SpriteRenderer sr in renderers)
		{
			newColors.Add(sr.color + colorToAdd);
		}

		foreach(SpriteRenderer sr in renderers)
		{
			initColors.Add(sr.color);
		}

		int innerIterations = 0;
		int outerIterations = 0;
		print("renderer count: " + renderers.Count);
		print("initColors count: " + initColors.Count);
		print("newColors count: " + newColors.Count);

        while(elapsed <= totalTime)
        {
			
			for(int i = 0; i< renderers.Count; i++)
			{
				renderers[i].color = Color.Lerp(initColors[i], newColors[i], elapsed/duration);
				elapsed = Time.time-startTime;
				innerIterations++;
				yield return null;
				print("elapsed: " + elapsed);
				print("duration : " +  duration);
				print("elapsed over duration : " + elapsed/duration);
				print("innerIterations: " + innerIterations);
			}
			outerIterations++;
			print("outerIterations: " +outerIterations);
			
			yield return null;
			
        }
    }

	//takes in a list of transforms, and gives you a list of SpriteRenderers that are children of those transforms
	public List<SpriteRenderer> GetAllSpriteRenderers(List<Transform> parents)
	{
		List<SpriteRenderer> SRs = new List<SpriteRenderer>();
		foreach(Transform trans in parents)
		{
			SpriteRenderer[] SRsInThisTransform = trans.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < SRsInThisTransform.Length; i++)
			{
				SRs.Add(SRsInThisTransform[i]);
			}
		}
		return SRs;
	}


}
