using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelConstruction : MonoBehaviour
{
	/*****************
	CreateDate: 	3/11/2022
	Functionality:	this script assists with DayJob level creation.  It can spawn walls along all the passages, delete them, etc.
	Notes:
	Dependencies:
	******************/
	
	//////////////////////////////Config
	public List<Passage> allPassages {get{ return  GetAllPassages();}}
	
	//////////////////////////////State
	
	//////////////////////////////Cached Component References
	
	
	void Awake()
    {
        
    }
	
    void Start()
    {
        
    }

    void Update()
    {
        
    }

	private List<Passage> GetAllPassages()
	{
		List<Passage> allPassages = GameObject.FindObjectsOfType<Passage>().ToList();
		return allPassages;
	}

	public void AllignAllPassages()
	{
		foreach (Passage p in GetAllPassages())
		{
			p.SnapRotation();
			p.SnapPosition();
			p.SnapScale();
			p.SnapScale();
		}
	}

}
