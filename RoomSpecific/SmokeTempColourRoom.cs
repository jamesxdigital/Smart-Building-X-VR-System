//Copyright 2018, James Milton, All rights reserved.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmokeTempColourRoom : MonoBehaviour {
	//3D texts which are visible in the system that give actual reading
	public TextMesh airCon1TextMesh;
	public TextMesh airCon2TextMesh;
	public TextMesh airCon3TextMesh;
	public TextMesh airCon4TextMesh;
	public TextMesh airCon5TextMesh;
	public TextMesh airCon6TextMesh;
	public TextMesh airCon7TextMesh;
	public TextMesh airCon8TextMesh;
	//GameObjects of the 6 smoke objects
	public GameObject Smoke1;
	public GameObject Smoke2;
	public GameObject Smoke3;
	public GameObject Smoke4;
	public GameObject Smoke5;
	public GameObject Smoke6;
	public GameObject Smoke7;
	public GameObject Smoke8;
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
			//obtain air conditioning data for unit 1 apply colour to left smoke particles
			ParticleSystem.MainModule settings1 = Smoke1.GetComponent<ParticleSystem>().main;
			settings1.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon1) );
			settings1.startSpeed = 5.5f;
			//obtain air conditioning data for unit 2 apply colour to left smoke particles
			ParticleSystem.MainModule settings2 = Smoke2.GetComponent<ParticleSystem>().main;
			settings2.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon2) );
			settings2.startSpeed = 5.5f;
			//obtain air conditioning data for unit 3 apply colour to left smoke particles
			ParticleSystem.MainModule settings3 = Smoke3.GetComponent<ParticleSystem>().main;
			settings3.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon3) );
			settings3.startSpeed = 5.5f;
			//obtain air conditioning data for unit 4 apply colour to left smoke particles
			ParticleSystem.MainModule settings4 = Smoke4.GetComponent<ParticleSystem>().main;
			settings4.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon4) );
			settings4.startSpeed = 5.5f;
			//obtain air conditioning data for unit 5 apply colour to left smoke particles
			ParticleSystem.MainModule settings5 = Smoke5.GetComponent<ParticleSystem>().main;
			settings5.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon5) );
			settings5.startSpeed = 5.5f;
			//obtain air conditioning data for unit 6 apply colour to left smoke particles
			ParticleSystem.MainModule settings6 = Smoke6.GetComponent<ParticleSystem>().main;
			settings6.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon6) );
			settings6.startSpeed = 5.5f;
			//obtain air conditioning data for unit 7 apply colour to left smoke particles
			ParticleSystem.MainModule settings7 = Smoke7.GetComponent<ParticleSystem>().main;
			settings7.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon7) );
			settings7.startSpeed = 5.5f;
			//obtain air conditioning data for unit 8 apply colour to left smoke particles
			ParticleSystem.MainModule settings8 = Smoke8.GetComponent<ParticleSystem>().main;
			settings8.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon8) );
			settings8.startSpeed = 5.5f;
			//obtaining air conditioning data and writting to 3D test in scene
			airCon1TextMesh.text = GetJsonDataRoom.airCon1.ToString("0.0") +"°C";
			airCon2TextMesh.text = GetJsonDataRoom.airCon2.ToString("0.0") +"°C";
			airCon3TextMesh.text = GetJsonDataRoom.airCon3.ToString("0.0") +"°C";
			airCon4TextMesh.text = GetJsonDataRoom.airCon4.ToString("0.0") +"°C";
			airCon5TextMesh.text = GetJsonDataRoom.airCon5.ToString("0.0") +"°C";
			airCon6TextMesh.text = GetJsonDataRoom.airCon6.ToString("0.0") +"°C";
			airCon7TextMesh.text = GetJsonDataRoom.airCon7.ToString("0.0") +"°C";
			airCon8TextMesh.text = GetJsonDataRoom.airCon8.ToString("0.0") +"°C";
		}else{
			//if air conditioning toggle is off, remove all smoke effects
			ParticleSystem.MainModule settings1 = Smoke1.GetComponent<ParticleSystem>().main;
			settings1.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon1) );
			settings1.startSpeed = 0f;
			ParticleSystem.MainModule settings2 = Smoke2.GetComponent<ParticleSystem>().main;
			settings2.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon2) );
			settings2.startSpeed = 0f;
			ParticleSystem.MainModule settings3 = Smoke3.GetComponent<ParticleSystem>().main;
			settings3.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon3) );
			settings3.startSpeed = 0f;
			ParticleSystem.MainModule settings4 = Smoke4.GetComponent<ParticleSystem>().main;
			settings4.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon4) );
			settings4.startSpeed = 0f;
			ParticleSystem.MainModule settings5 = Smoke5.GetComponent<ParticleSystem>().main;
			settings5.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon5) );
			settings5.startSpeed = 0f;
			ParticleSystem.MainModule settings6 = Smoke6.GetComponent<ParticleSystem>().main;
			settings6.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon6) );
			settings6.startSpeed = 0f;
			ParticleSystem.MainModule settings7 = Smoke7.GetComponent<ParticleSystem>().main;
			settings7.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon7) );
			settings7.startSpeed = 0f;
			ParticleSystem.MainModule settings8 = Smoke8.GetComponent<ParticleSystem>().main;
			settings8.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonDataRoom.airCon8) );
			settings8.startSpeed = 0f;
			//and 3d texts
			airCon1TextMesh.text = "";
			airCon2TextMesh.text = "";
			airCon3TextMesh.text = "";
			airCon4TextMesh.text = "";
			airCon5TextMesh.text = "";
			airCon6TextMesh.text = "";
			airCon7TextMesh.text = "";
			airCon8TextMesh.text = "";
		}

	}

}
