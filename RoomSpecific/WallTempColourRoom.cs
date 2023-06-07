//Copyright 2018, James Milton, All rights reserved.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallTempColourRoom : MonoBehaviour {
	//3D texts which are visible in the system that give actual reading
	public TextMesh tempRightCentreText;
	public TextMesh tempRightRightText;
	public TextMesh tempRightLeftText;
	public TextMesh tempLeftCentreText;
	public TextMesh tempLeftRightText;
	public TextMesh tempLeftLeftText;
	public TextMesh tempCentreText;
	//Toggle whether room temperature visualisation is switched on or off
	public Toggle tempToggle;
	public GameObject ToggleTitle;
	//Materials of the rooms's walls
	public Material BackWall;
	public Material FrontWall;
	public Material LeftWall;
	public Material RightWall;

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
			//obtaining room temperature data applying colour to shader property for left left wall
			LeftWall.SetColor ("_ColourLeftGradient", tempToRGB (GetJsonDataRoom.tempLeftLeft));
			LeftWall.SetColor ("_ColourMiddleGradient", tempToRGB (GetJsonDataRoom.tempLeftCentre));
			LeftWall.SetColor ("_ColourRightGradient", tempToRGB (GetJsonDataRoom.tempLeftRight));
			//obtaining room temperature data applying colour to shader property for right right wall
			RightWall.SetColor ("_ColourLeftGradient", tempToRGB (GetJsonDataRoom.tempRightLeft));
			RightWall.SetColor ("_ColourMiddleGradient", tempToRGB (GetJsonDataRoom.tempRightCentre));
			RightWall.SetColor ("_ColourRightGradient", tempToRGB (GetJsonDataRoom.tempRightRight));
			//obtaining room temperature data applying colour to shader property for right right wall
			BackWall.SetColor ("_ColourLeftGradient", tempToRGB (GetJsonDataRoom.tempRightRight));
			BackWall.SetColor ("_ColourMiddleGradient", tempToRGB (GetJsonDataRoom.tempRightRight));
			BackWall.SetColor ("_ColourRightGradient", tempToRGB (GetJsonDataRoom.tempLeftLeft));
			//obtaining room temperature data applying colour to shader property for left right wall
			FrontWall.SetColor ("_ColourLeftGradient", tempToRGB (GetJsonDataRoom.tempLeftRight));
			FrontWall.SetColor ("_ColourRightGradient", tempToRGB (GetJsonDataRoom.tempRightLeft));
			//obtaining room temperature data and writting to 3D text in scene to one decimal place
			tempRightCentreText.text = GetJsonDataRoom.tempRightCentre.ToString ("0.0") +"°C";
			tempRightRightText.text = GetJsonDataRoom.tempRightRight.ToString ("0.0") +"°C";
			tempRightLeftText.text = GetJsonDataRoom.tempRightLeft.ToString ("0.0") +"°C";
			tempLeftCentreText.text = GetJsonDataRoom.tempLeftCentre.ToString ("0.0") +"°C";
			tempLeftRightText.text = GetJsonDataRoom.tempLeftRight.ToString ("0.0") +"°C";
			tempLeftLeftText.text = GetJsonDataRoom.tempLeftLeft.ToString ("0.0") +"°C";
			tempCentreText.text = GetJsonDataRoom.tempCentre.ToString ("0.0") +"°C";

		} else {
			//if room temperature toggle is off, remove all grids
			LeftWall.SetColor ("_ColourLeftGradient", new Color(0.8F,0.8F, 0.8F,1F));
			LeftWall.SetColor ("_ColourMiddleGradient", new Color(0.8F,0.8F, 0.8F,1F));
			LeftWall.SetColor ("_ColourRightGradient", new Color(0.8F,0.8F, 0.8F,1F));
			RightWall.SetColor ("_ColourLeftGradient", new Color(0.8F,0.8F, 0.8F,1F));
			RightWall.SetColor ("_ColourMiddleGradient", new Color(0.8F,0.8F, 0.8F,1F));
			RightWall.SetColor ("_ColourRightGradient", new Color(0.8F,0.8F, 0.8F,1F));
			BackWall.SetColor ("_ColourLeftGradient", new Color(0.8F,0.8F, 0.8F,1F));
			BackWall.SetColor ("_ColourMiddleGradient", new Color(0.8F,0.8F, 0.8F,1F));
			BackWall.SetColor ("_ColourRightGradient", new Color(0.8F,0.8F, 0.8F,1F));
			FrontWall.SetColor ("_ColourLeftGradient", new Color(0.8F,0.8F, 0.8F,1F));
			FrontWall.SetColor ("_ColourRightGradient", new Color(0.8F,0.8F, 0.8F,1F));
			//and remove all texts
			tempRightCentreText.text = "";
			tempRightRightText.text = "";
			tempRightLeftText.text = "";
			tempLeftCentreText.text = "";
			tempLeftRightText.text = "";
			tempLeftLeftText.text = "";
			tempCentreText.text = "";
		}
	}
	void OnApplicationQuit(){
		//when visualisation is finished, reset the wall's colour	
		LeftWall.SetColor ("_ColourLeftGradient", new Color(0.8F,0.8F, 0.8F,1F));
		LeftWall.SetColor ("_ColourMiddleGradient", new Color(0.8F,0.8F, 0.8F,1F));
		LeftWall.SetColor ("_ColourRightGradient", new Color(0.8F,0.8F, 0.8F,1F));
		RightWall.SetColor ("_ColourLeftGradient", new Color(0.8F,0.8F, 0.8F,1F));
		RightWall.SetColor ("_ColourMiddleGradient", new Color(0.8F,0.8F, 0.8F,1F));
		RightWall.SetColor ("_ColourRightGradient", new Color(0.8F,0.8F, 0.8F,1F));
		BackWall.SetColor ("_ColourLeftGradient", new Color(0.8F,0.8F, 0.8F,1F));
		BackWall.SetColor ("_ColourMiddleGradient", new Color(0.8F,0.8F, 0.8F,1F));
		BackWall.SetColor ("_ColourRightGradient", new Color(0.8F,0.8F, 0.8F,1F));
		FrontWall.SetColor ("_ColourLeftGradient", new Color(0.8F,0.8F, 0.8F,1F));
		FrontWall.SetColor ("_ColourRightGradient", new Color(0.8F,0.8F, 0.8F,1F));
	}
}
