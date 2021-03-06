using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

	public float colorAlterationDivider;
	[Tooltip("mostly for testing if you want to play one game specifically, or have one game included in the level at least ")]  
	public List<MiniGame> mandatoryMiniGames;
	public Color lerpTargetColor;  //VGIU

	//////////////////////////////State
	public bool isLevelFinished = false;
	public bool isLevelWon = false;
	protected bool allGamesSlowed = false;
	
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

	
    public void GatherProgressionParameters()
    {

        Object[] allProgressionParams = Resources.LoadAll("", typeof(ProgressionParams));

        foreach (ProgressionParams p in allProgressionParams)
        {
            if (p.difficultyDescription ==  thisLevel.generalDifficulty)
            {
                this.progressionParams = p;
            }
        }

        if (progressionParams == null)
        {
            Debug.LogError("there was no  matching ProgressionParameter object found.  Something is wrong");
            return;
        }
    }

    public void GatherDifficultyParameters()
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

	public void PassParametersToMiniGame(MiniGame mg)
	{
		mg.difficultyParams = this.difficultyParams;
		mg.progressionParams = this.progressionParams;
	}



	//this method uses a colorToAdd and adds it to each pre-existing color to get the target color, instead of just lerping all colors to a single constant color.  ColorToAdd should be close to white if the goal is to lighten everything
	public IEnumerator ColorFade(List<SpriteRenderer> renderers, float duration, Color lerpTargetColor)
    {
		allGamesSlowed = true;
		float startTime = Time.time;
		float elapsed = 0f;

		//the original idea for this was to get a greyed out version of each existing color.   But that didn't account for new SRs getting added in the midst of the loop. 
		//it wouldn't be impossible to do that, but simply lerping all SRs to a singular grey color is just fine for now. 
        // List<Color> newColors = new List<Color>();
		// List<Color> initColors = new List<Color>();
		// foreach(SpriteRenderer sr in renderers)
		// {
		// 	initColors.Add(sr.color);
		// 	float hue, sat, val;
		// 	Color.RGBToHSV(sr.color, out hue, out sat, out val);
		// 	Color reducedColor = Color.HSVToRGB(hue, sat/colorAlterationDivider, val/colorAlterationDivider );
		// 	reducedColor.a = 0.6f;

		// 	newColors.Add(reducedColor);
		// }


        while(elapsed <= duration)
        {
			for(int i = 0; i< renderers.Count; i++)
			{
				// renderers[i].color = Color.Lerp(initColors[i], newColors[i], elapsed/duration);
				renderers[i].color = Color.Lerp(renderers[i].color, lerpTargetColor, elapsed/duration);  //I know this isn't the perfect lerp because the first argument moves with each iteration.  but it works
			}
			elapsed = Time.time-startTime;
			yield return null;
        }
		print("outer loop exited");
		allGamesSlowed = false;
    }

	//takes in a list of transforms, and gives you a list of components of type T that are children of those transforms, but not if that object has tag in tagsToAvoid
	public List<T> GetComponentsFromTransforms<T>(List<Transform> parents, List<string> tagsToAvoid) where T : Component
	{
		List<T> SRs = new List<T>();
		foreach(Transform trans in parents)
		{
			T[] SRsInThisTransform = trans.GetComponentsInChildren<T>();
			for (int i = 0; i < SRsInThisTransform.Length; i++)
			{
				if (tagsToAvoid.Contains(SRsInThisTransform[i].tag) )
				{
					continue;
				}
				else
				{
					SRs.Add(SRsInThisTransform[i]);
				}
				
			}
		}
		return SRs;
	}

	public void DistributeParamsAndLevel(MiniGame mg)
	{
		mg.level = this.thisLevel;
		mg.difficultyParams = this.difficultyParams;
		mg.progressionParams = this.progressionParams;
	}


	///leaving it here for now, but it's not being used
	public void removeObjectFromList<T>(GameObject go, List<T> list)
	{
		if (go.TryGetComponent<T>(out T test))
		{
			list.Remove(go.GetComponent<T>());
		}
		
	}


}
