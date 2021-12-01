using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerpTest : MonoBehaviour
{
	/*****************
	CreateDate: 
	Functionality:
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public Color targetColor;
	public Color startingColor;
	public float duration;
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	public SpriteRenderer sr;
	public List<SpriteRenderer> AllSRs;
	
	

    void Start()
    {
        startingColor = sr.color;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(LerpColor(duration));
		}
    }


	public IEnumerator LerpColor(float duration)
	{
		// print("coroutine started");
		float startTime = Time.time;
		float totalTime = duration;
		float elapsed = 0f;
		while (elapsed < totalTime)
		{
			// sr.color = Color.Lerp(startingColor, targetColor, elapsed/duration);
			for (int i = 0; i < AllSRs.Count; i++)
			{
				AllSRs[i].color = Color.Lerp(startingColor, targetColor, elapsed/duration);
			}
			elapsed = Time.time- startTime;
			yield return null;

		}
		// print("loop complete");
		yield return new WaitForSeconds(2f);
		sr.color = startingColor;
	}
}
