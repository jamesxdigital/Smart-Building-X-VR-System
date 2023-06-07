//Copyright 2018, James Milton, All rights reserved.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallTempColour : MonoBehaviour {
	//Toggle whether room temperature visualisation is switched on or off
	public Toggle tempToggle;
	public GameObject ToggleTitle;
	//Materials of the rooms's wall
	public Material Shader;
	//3D texts which are visible in the system that give actual reading
	public TextMesh temperatureLeftText;
	public TextMesh temperatureRightText;
	//when given a room temperature reading, will output an appropriate colour
	static public Color tempToRGB(float temp){
		float red = 198f/225f;
		float green = (1000f - (temp*40))/225f;
		float blue = 29f/225f;
		Color rgbColor = new Color(red,green, blue,1F);

		return rgbColor;
	}
	//when room temperature visualisation is switched on, data is obtained from BMS database and passed through 
	// ApplyGradient().  The received settings are then applied to the GameObject's material properties	
	public void ApplyGradient() { 
		tempToggle.gameObject.SetActive(true);
		ToggleTitle.SetActive (true);
		if (tempToggle.isOn) {
			//obtaining room temperature data applying colour to shader property
			Shader.SetColor ("_ColourLeftGradient", tempToRGB (GetJsonData.tempLeft));
			Shader.SetColor ("_ColourRightGradient", tempToRGB (GetJsonData.tempRight));
			//obtaining room temperature data and writting to 3D text in scene to one decimal place
			temperatureLeftText.text = GetJsonData.tempLeft.ToString ("0.0") +"°C";
			temperatureRightText.text = GetJsonData.tempRight.ToString ("0.0") +"°C";

		} else {
			//if room temperature toggle is off, remove all grids
			Shader.SetColor("_ColourLeftGradient", new Color(0.8F,0.8F, 0.8F,1F));
			Shader.SetColor("_ColourRightGradient", new Color(0.8F,0.8F, 0.8F,1F));
			//and remove all texts
			temperatureLeftText.text = "";
			temperatureRightText.text = "";
		}
	}

	//when visualisation is finished, reset the wall's colour	
	void OnApplicationQuit(){
		Shader.SetColor("_ColourLeftGradient", new Color(0.8F,0.8F, 0.8F,1F));
		Shader.SetColor("_ColourRightGradient", new Color(0.8F,0.8F, 0.8F,1F));
	}
}
