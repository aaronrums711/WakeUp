using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCrunch_LevelManager : LevelManager, ILevelMover
{
	/*****************
	CreateDate: 	10/24/21
	Functionality:	handles mini game spawning and other tasks that are specific to the Time Crunch level type
	Notes:			there will be one child level manager for each level type.  Each child will implement the same interface, but imlement the method differently of course, depending on the needs of that level type. 
	Dependencies:	LevelManager
	******************/
	
	//////////////////////////////Config
	public LevelType managingLevelType;
	public List<Vector3> miniGameSpawnPoints4; //VGIU. each of these represents the locations of mini games for levels with 4, 3 and 2 total games
	public List<Vector3> miniGameSpawnPoints3; //VGIU
	public List<Vector3> miniGameSpawnPoints2; //VGIU

	public List<Vector3> cameraPositions;  //ATM there are four entries in this list, even though the third and 4th are the same (turns out the camera keeps the same position regardles of if there are 3 or 4 games on screen)
	public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
	[SerializeField] float lerpDuration;

	//////////////////////////////State
	[Tooltip("this could potentially be part of the parent class...but what defines 'active' will be pretty different for each level type")]
	public int activeGamesCount;

	//////////////////////////////Cached Component References
	
	
    void  Awake() ///Start()
    {
		SetUpLevel();
		if (activeMiniGameParent == null)
		{Debug.LogError("ActiveMiniGame parent not found, that's an issue");}
		base.GatherDifficultyParameters();
		base.GatherProgressionParameters();
		MoveLevelForward();
		// MiniGameElement.OnSpawnGameElement += addSRToList;
    }


    void Update()
    {
		if (Time.frameCount % executionInterval == 0) //only execute it every executionInterval frames.  This probably isn't that necessary...but we'll see
		{
			CheckAllMiniGames();
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			List<Transform> transforms = new List<Transform>();
			transforms.Add(activeMiniGameParent);

			List<string> tagsToAvoid = new List<string> {"PlayAreaBarriers"};
			spriteRenderers = GetComponentsFromTransforms<SpriteRenderer>(transforms, tagsToAvoid);
			MiniGameElement.OnSpawnGameElement += addSRToList;
			MiniGameElement.OnDestroyGameElement += removeSRFromList;

			StartCoroutine(ColorFade(spriteRenderers, lerpDuration, base.lerpTargetColor));
			StartCoroutine(WaitToUnsubscribe());
		}
    }


	public void MoveLevelForward()
	{
		StartCoroutine(SpawnGames(thisLevel.lagBetweenGamesSeconds, thisLevel.totalGamesInLevel, thisLevel.timeCrunchGamesToStart));
	}

	///realizing now that this method only takes into account elapsed time when spawning new games.  In the future we may want to consider the progress of all the games, or most recent added game, or anything like that. 
	///just a thought for the future.  For the TimeCrunch level type using time only might be fine. 
	private IEnumerator SpawnGames(float secondsBetween, int numGames, int numGamesAtStart)
	{
		List<Vector3> spawnPointsToUse = new List<Vector3>();
		if (thisLevel.totalGamesInLevel == 2) {spawnPointsToUse = miniGameSpawnPoints2;}
		else if (thisLevel.totalGamesInLevel == 3) {spawnPointsToUse = miniGameSpawnPoints3;}
		else if (thisLevel.totalGamesInLevel == 4) {spawnPointsToUse = miniGameSpawnPoints4;}
		else{Debug.LogError("there is less than 2 or more than 4 total games set for this TimeCrunch level.  This should not be!");}

		Camera.main.transform.position = cameraPositions[numGamesAtStart-1];
		for (int i=0; i< numGamesAtStart; i++)
		{
			MiniGame miniGame;
			if (base.mandatoryMiniGames.Count > 0  && base.mandatoryMiniGames.Count-1 >= i)
			{
				miniGame = Instantiate(mandatoryMiniGames[i], spawnPointsToUse[i], Quaternion.identity, activeMiniGameParent);
			}
			else
			{
				miniGame = Instantiate(allGamesForThisLevel[i], spawnPointsToUse[i], Quaternion.identity, activeMiniGameParent);
			}
			miniGame.gameObject.SetActive(true);
			base.PassParametersToMiniGame(miniGame);
			miniGame.orderInLevel = i+1;
			activeGamesCount = activeMiniGameParent.childCount;
			gamesActiveInThisLevel.Add(miniGame);
		}

		float timeElapsed = 0f;
		for (int i = numGamesAtStart ; i < numGames; i++)
		{
			while(timeElapsed < secondsBetween)
			{
				yield return new WaitForSeconds(1f);
				timeElapsed += 1f;
			}
			timeElapsed = 0;
			MiniGame newMiniGame = Instantiate(allGamesForThisLevel[i], spawnPointsToUse[i], Quaternion.identity, activeMiniGameParent);
			activeGamesCount = activeMiniGameParent.childCount;
			newMiniGame.gameObject.SetActive(true);
			base.PassParametersToMiniGame(newMiniGame);
			newMiniGame.orderInLevel = i+1;
			gamesActiveInThisLevel.Add(newMiniGame);
			StartCoroutine(MoveCamera());
		}

		// print("all games have been spawned");
		
	}

	private IEnumerator MoveCamera()
	{
		Transform camera = Camera.main.transform;
		float movementTime = 1f;
		float elapsed  = 0f;
		float percent  = 0f;


		Vector3 startingPos = camera.position;
		Vector3 targetPos = cameraPositions[activeGamesCount-1];
		while (percent < 0.99)
		{
			camera.transform.position = Vector3.Lerp(startingPos, targetPos, percent);
			elapsed += Time.deltaTime;
			percent = elapsed/movementTime;
			yield return null;
		}
		camera.transform.position = targetPos;
	}

	public void SlowAllGames()
	{
		return;
	}

	//this is subscribed to MiniGameElement.OnDestroyGameElement(), so that any time that event is fired, this method is called that the destroyed gameObject's sprite renderer is removed from the list.  This handles the case of objects being destroyed in the middle of the fade coroutine. 
	public void removeSRFromList(GameObject go)
	{
		if (go.TryGetComponent<SpriteRenderer>(out var test))
		{
			spriteRenderers.Remove(go.GetComponent<SpriteRenderer>());
		}
	}

	//this is subscribed to MiniGameElement.OnSpawnGameElement(), so that any time that event is fired, this method is called and the sprite renderer is added to the list.  This handles the case of objects being spawned in the middle of the fade coroutine
	public void addSRToList(GameObject go)
	{
		if (go.TryGetComponent<SpriteRenderer>(out var test))
		{
			spriteRenderers.Add(go.GetComponent<SpriteRenderer>());
		}
	}

	private IEnumerator WaitToUnsubscribe()
	{
		while (base.allGamesSlowed)
		{
			yield return null;
		}
		MiniGameElement.OnSpawnGameElement -= addSRToList;
		MiniGameElement.OnDestroyGameElement -= removeSRFromList;


	}
	
	public void GameAdditionEffect()
	{

	}

	/**
	1. pause all games
	2. move camera
	3. instantate new game (potentially with some fade in effect)
	4. short countdown by flashing all colors white with count down sound
	5. set all colors back to what they were
	6. restart all games

	actually now that I think about it....if games are being spawned relatively quickly, the whole timer thing probably isn't necessary, and may even get annoying...
	so instead of that, we could do just a slow down effect on all active games with a color lerp to grey, then spawn new game, then restart all games and return to normal colors
	**/

	
	
}

