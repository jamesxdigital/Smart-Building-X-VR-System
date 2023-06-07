//Copyright 2018, James Milton, All rights reserved.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CeilingCO2Grid : MonoBehaviour {
	//Materials of the 3 ceiling GameObject layers
	public Material GridMain;
	public Material GridLeft;
	public Material GridRight;
	//3D texts which are visible in the system that give actual reading
	public TextMesh co2LeftText;
	public TextMesh co2RightText;
	//Toggle whether CO2 visualisation is switched on or off
	public Toggle co2Toggle;

	//when given a PPM CO2 reading, will output an array in the format [GridLineThickness, GridLineSpacing]
	static public float[] calculateGrid(float co2) {
		if (co2 < 300f) { //very sparse grid
			float[] gridSettings = { 0.025f, 0.8f };
			return gridSettings;
		}  else if (co2 > 300f && co2 < 400f) { //sparse grid
			float[] gridSettings = { 0.05f, 0.4f };
			return gridSettings;
		}  else if (co2 > 400f && co2 < 500f) { //medium grid
			float[] gridSettings = { 0.1f, 0.2f };
			return gridSettings;
		}  else if (co2 > 500f && co2 < 600f) {
			float[] gridSettings = { 0.2f, 0.1f }; //dence grid
			return gridSettings;
		}  else {
			float[] gridSettings = { 0.4f, 0.05f }; //very dence grid
			return gridSettings;	
		}

	}

	//when co2 visualisation is switched on, data is obtained from BMS database and passed through 
	// calculateGrid().  The received settings are then applied to the GameObject's material properties
	public void ApplyGrid() { 
		co2Toggle.gameObject.SetActive(true);
		if (co2Toggle.isOn) {
			//reference grid's properties always remain constant
			GridMain.SetFloat ("_GridLineThickness", 0.2f);
			GridMain.SetFloat ("_GridLineSpacing", 2f);
			//left side grid obtaining CO2 data from left side CO2 sensor
			GridLeft.SetFloat ("_GridLineThickness", calculateGrid (GetJsonData.co2left) [0]);
			GridLeft.SetFloat ("_GridLineSpacing", calculateGrid (GetJsonData.co2left) [1]);
			//right side grid obtaining CO2 data from right side CO2 sensor
			GridRight.SetFloat ("_GridLineThickness", calculateGrid (GetJsonData.co2right) [0]);
			GridRight.SetFloat ("_GridLineSpacing", calculateGrid (GetJsonData.co2right) [1]);
			//obtaining CO2 data and writting to 3D test in scene
			co2LeftText.text = GetJsonData.co2left.ToString ("0") + " PPM";
			co2RightText.text = GetJsonData.co2right.ToString("0") +" PPM";

		} else {
			//if CO2 toggle is off, remove all grids
			GridMain.SetFloat ("_GridLineThickness", 0f);
			GridLeft.SetFloat ("_GridLineThickness", 0f);
			GridRight.SetFloat ("_GridLineThickness", 0f);
			//and remove all text
			co2LeftText.text = "";
			co2RightText.text = "";
		}
	}
	//when visualisation is finished, reset the grid
	void OnApplicationQuit(){
		GridMain.SetFloat ("_GridLineThickness", 0f);
		GridLeft.SetFloat ("_GridLineThickness", 0f);
		GridRight.SetFloat ("_GridLineThickness", 0f);
	}
}
