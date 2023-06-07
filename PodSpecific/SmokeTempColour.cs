//Copyright 2018, James Milton, All rights reserved.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmokeTempColour : MonoBehaviour {
	//GameObjects of the 6 smoke objects
	public GameObject SmokeLeft1;
	public GameObject SmokeLeft2;
	public GameObject SmokeLeft3;
	public GameObject SmokeRight1;
	public GameObject SmokeRight2;
	public GameObject SmokeRight3;
	//3D texts which are visible in the system that give actual reading
	public TextMesh airConLeftText;
	public TextMesh airConRightText;
	//Toggle whether air conditoning visualisation is switched on or off
	public Toggle airConToggle;

	//when given a air conditioning temperature reading, will output an appropriate colour
	static public Color tempToRGB(float temp){
		float red = 198f/225f;
		float green = (260f - (temp*4))/225f;
		float blue = 29f/225f;
		Color rgbColor = new Color(red,green, blue,1F);
		return rgbColor;
	}

	//when air conditioning visualisation is switched on, data is obtained from BMS database and passed through 
	// ApplyColorChange().  The received settings are then applied to the smoke GameObject's properties
	public void ApplyColorChange() { 
		airConToggle.gameObject.SetActive(true);
		if (airConToggle.isOn) {
			//obtain air conditioning data from left side of room and apply colour to left smoke particles
			ParticleSystem.MainModule settings1 = SmokeLeft1.GetComponent<ParticleSystem>().main;
			settings1.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonData.airConLeft) );
			settings1.startSpeed = 5.5f;
			settings1 = SmokeLeft2.GetComponent<ParticleSystem>().main;
			settings1.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonData.airConLeft) );
			settings1.startSpeed = 5.5f;
			settings1 = SmokeLeft3.GetComponent<ParticleSystem>().main;
			settings1.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonData.airConLeft) );
			settings1.startSpeed = 5.5f;
			//obtain air conditioning data from right side of room and apply colour to right smoke particles
			ParticleSystem.MainModule settings2 = SmokeRight1.GetComponent<ParticleSystem>().main;
			settings2.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonData.airConRight) );
			settings2.startSpeed = 5.5f;
			settings2 = SmokeRight2.GetComponent<ParticleSystem>().main;
			settings2.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonData.airConRight) );
			settings2.startSpeed = 5.5f;
			settings2 = SmokeRight3.GetComponent<ParticleSystem>().main;
			settings2.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonData.airConRight) );
			settings2.startSpeed = 5.5f;
			//obtaining air conditioning data and writting to 3D test in scene
			airConLeftText.text = GetJsonData.airConLeft.ToString("0.0") +"°C";
			airConRightText.text = GetJsonData.airConRight.ToString("0.0") +"°C";

		}else{
			//if air conditioning toggle is off, remove all smoke effects
			ParticleSystem.MainModule settings1 = SmokeLeft1.GetComponent<ParticleSystem>().main;
			ParticleSystem.MainModule settings2 = SmokeRight1.GetComponent<ParticleSystem>().main;
			settings1.startSpeed = 0f;
			settings2.startSpeed = 0f;
			settings1 = SmokeLeft2.GetComponent<ParticleSystem>().main;
			settings2 = SmokeRight2.GetComponent<ParticleSystem>().main;
			settings1.startSpeed = 0f;
			settings2.startSpeed = 0f;
			settings1 = SmokeLeft3.GetComponent<ParticleSystem>().main;
			settings2 = SmokeRight3.GetComponent<ParticleSystem>().main;
			settings1.startSpeed = 0f;
			settings2.startSpeed = 0f;
			//and 3d texts
			airConLeftText.text = "";
			airConRightText.text = "";
		}
	}
}
