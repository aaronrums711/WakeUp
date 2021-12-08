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
	public bool lerpAll;

	//////////////////////////////Cached Component References
	public List<SpriteRenderer> AllSRs;
	
	

    void Start()
    {
        startingColor = Color.white;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
		{
			if(lerpAll == true)
			{
				print("color lerp called");
				// StartCoroutine(LerpAll(duration));
				List<SpriteRenderer> listToPass = new List<SpriteRenderer>();
				foreach (SpriteRenderer sr in AllSRs)
				{
					listToPass.Add(sr);
				}
					
				StartCoroutine(LerpAllListPassedIn(duration, AllSRs));
			}
			if (lerpAll == false)
			{
				print("lerp one called");
				for (int i = 0; i < AllSRs.Count; i++)
				{
					StartCoroutine(LerpOne(AllSRs[i])); 
				}
			}
		}
    }


	public IEnumerator LerpAll(float duration)
	{
		print("coroutine started");
		List<SpriteRenderer> SRsForMethod = AllSRs;
		float startTime = Time.time;
		float totalTime = duration;
		float elapsed = 0f;
		while (elapsed < totalTime)
		{
			// sr.color = Color.Lerp(startingColor, targetColor, elapsed/duration);
			for (int i = 0; i < SRsForMethod.Count; i++)
			{
				SRsForMethod[i].color = Color.Lerp(startingColor, targetColor, elapsed/duration);
			}
			elapsed = Time.time- startTime;
			
			yield return null;

		}
		print("loop complete");
		print("all SRs at end of loop: " + SRsForMethod.Count); 
		yield return new WaitForSeconds(2f);
		for (int i = 0; i < SRsForMethod.Count; i++)
		{
			SRsForMethod[i].color = startingColor;
		}
	}



	public IEnumerator LerpAllListPassedIn(float duration, List<SpriteRenderer> SRlist)
	{
		print("coroutine started");
		float startTime = Time.time;
		float totalTime = duration;
		float elapsed = 0f;
		while (elapsed < totalTime)
		{
			// sr.color = Color.Lerp(startingColor, targetColor, elapsed/duration);
			for (int i = 0; i < SRlist.Count; i++)
			{
				SRlist[i].color = Color.Lerp(startingColor, targetColor, elapsed/duration);
			}
			elapsed = Time.time- startTime;
			yield return null;

		}
		print("loop complete");
		print("all SRs at end of loop: " + SRlist.Count); 
		yield return new WaitForSeconds(2f);
		for (int i = 0; i < SRlist.Count; i++)
		{
			SRlist[i].color = startingColor;
		}
	}

	public IEnumerator LerpOne(SpriteRenderer sr)
	{
		float startTime = Time.time;
		float totalTime = duration;
		float elapsed = 0f;
		while (elapsed < totalTime)
		{
			sr.color = Color.Lerp(startingColor, targetColor, elapsed/duration);
			elapsed = Time.time- startTime;
			yield return null;
		}
		print("loop complete");
		yield return new WaitForSeconds(2f);
		for (int i = 0; i < AllSRs.Count; i++)
		{
			AllSRs[i].color = startingColor;
		}
	}


}
