using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalettes : MonoBehaviour
{
	/*****************
	CreateDate: 8/2/2021
	Functionality: this should just hold a list of palettes to be used in each level. 
	******************/
	
    public Palette[] allPalletes;
}

[System.Serializable]
public struct Palette
{
    public string paletteName;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
}
