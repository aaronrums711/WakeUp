using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInnerBar 
{
	/*****************
	CreateDate: 11/7/21
	Functionality: 	this only exists so it will be easy to grab all inner bars by searching for "IInnerBar" instead of having to search for the up/down and right left scripts together
	Notes:	
	Dependencies:
	******************/
	
	public void ExtendRectangle();
}
