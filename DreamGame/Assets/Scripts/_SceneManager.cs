using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class _SceneManager : MonoBehaviour
{
	/*****************
	CreateDate: 	11/2/21
	Functionality:	handles scene loading
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References

	
    public IEnumerator LoadStartScreen()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
        
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;  //
        int totalSceneCount = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex != totalSceneCount - 1) //if this scene IS NOT the last scene...just load the next one 
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(1);  //ELSE, load the start screen
        }
    }
}
