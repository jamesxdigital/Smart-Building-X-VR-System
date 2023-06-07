//Copyright 2018, James Milton, All rights reserved.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetJsonDataFlow : MonoBehaviour {
	//text object displaying the date
	public Text date;
	//text object where error messages are displayed
	public Text extraInfo;
	//text objects temperature data values
	public TextMesh temperatureLeftText;
	public TextMesh temperatureRightText;
	//text objects co2 data values
	public TextMesh co2LeftText;
	public TextMesh co2RightText;
	//text objects air condioning data values
	public TextMesh airConLeftText;
	public TextMesh airConRightText;
	//GameObject materials to manipulate
	public Material WallShader;
	public Material GridMain;
	public Material GridLeft;
	public Material GridRight;
	//Smoke GameObjects to manipulate
	public GameObject SmokeLeft1;
	public GameObject SmokeLeft2;
	public GameObject SmokeLeft3;
	public GameObject SmokeRight1;
	public GameObject SmokeRight2;
	public GameObject SmokeRight3;
	//toggles for filtering which datafeeds to show
	public Toggle tempToggle;
	public Toggle Co2Toggle;
	public Toggle AirConToggle;
	//end and start time  converted string values
	public static string startTimeString;
	public static string endTimeString;

	public void ApplyButton(){
		//obtain the start and end date from the DatePickerControl class
		DateTime endTimeDate = new DateTime(DatePickerControl.DateGlobal.Year, DatePickerControl.DateGlobal.Month, DatePickerControl.DateGlobal.Day,0,0,0);
		DateTime startTimeDate = endTimeDate.AddDays(-1);
		//sterilise date object so can be passed to http query
		startTimeString = startTimeDate.Year+"-"+startTimeDate.Month.ToString("D2")+"-"+startTimeDate.Day.ToString("D2")+"T"+startTimeDate.Hour.ToString("D2")+":"+startTimeDate.Minute.ToString("D2")+":"+startTimeDate.Second.ToString("D2");
		endTimeString = endTimeDate.Year+"-"+endTimeDate.Month.ToString("D2")+"-"+endTimeDate.Day.ToString("D2")+"T"+endTimeDate.Hour.ToString("D2")+":"+endTimeDate.Minute.ToString("D2")+":"+endTimeDate.Second.ToString("D2");
		//if the user is not connected to the universities network, throw and error
		if (getDataArray("BMS-L11O42S21")[1] == "bad connection"){
			date.text = "Error: No connection to shef.ac.uk network";
			//remove any text on scene
			extraInfo.text = temperatureLeftText.text = temperatureRightText.text = co2LeftText.text = co2RightText.text = airConLeftText.text = airConRightText.text = "";
			//remove gradients from wall 
			ApplyGradient (0f,0f,false);
			//remove grids from ceiling 
			ApplyGrid (0f,0f,false);
			//remove smoke colour effects
			ApplySmokeColorChange(0f,0f,false); 

		}
		//if data available, commence annimation Coroutine
		else if (getDataArray ("BMS-L11O42S21").Length > 2) {
			StartCoroutine (GenerateGradient ());
		} 
		//if the selected date does not contain any data, throw an error message
		else {
			date.text = "Error: No data for this time";
			//remove any text on scene
			extraInfo.text = temperatureLeftText.text = temperatureRightText.text = co2LeftText.text = co2RightText.text = airConLeftText.text = airConRightText.text = "";
			//remove gradients from wall 
			ApplyGradient (0f,0f,false);
			//remove grids from ceiling 
			ApplyGrid (0f,0f,false);
			//remove smoke colour effects
			ApplySmokeColorChange(0f,0f,false); 
		}
	}
	IEnumerator GenerateGradient()
	{
		extraInfo.text = "Press 'C' to exit animation";
		//obtain data for sensor for whole day and store in an array
		string[] L11O42S21 = getDataArray ("BMS-L11O42S21");
		string[] L11O43S21 = getDataArray ("BMS-L11O43S21");
		string[] L11O42S1 = getDataArray ("BMS-L11O42S1");
		string[] L11O43S1 = getDataArray ("BMS-L11O43S1");
		string[] L11O43S3 = getDataArray ("BMS-L11O43S3");
		string[] L11O42S3 = getDataArray ("BMS-L11O42S3");
		//begin iteration
		for (var i = 1; i < getDataArray("BMS-L11O42S21").Length; i=i+1)
		{
			try{
				if (Input.GetKeyDown(KeyCode.C))
				{
					break;
				}
				//obtain indiviual pieces of data from array
				string rowTempLeft = L11O42S21[i];
				string rowTempRight= L11O43S21[i];
				string rowCo2Left = L11O42S1[i];
				string rowCo2Right= L11O43S1[i];
				string rowAirConLeft = L11O43S3[i];
				string rowAirConRight= L11O42S3[i];
				//obtain date and time values
				int pFrom = rowTempLeft.IndexOf("datetime:") + "datetime:".Length;
				int pTo = rowTempLeft.LastIndexOf("Z,time:");
				string dateJson = rowTempLeft.Substring(pFrom, pTo - pFrom);
				//obtain temperature json data
				pFrom = rowTempLeft.IndexOf("value:") + "value:".Length;
				pTo = rowTempLeft.LastIndexOf("}");
				string valJsonTempLeft = rowTempLeft.Substring(pFrom, pTo - pFrom);
				pFrom = rowTempRight.IndexOf("value:") + "value:".Length;
				pTo = rowTempRight.LastIndexOf("}");
				string valJsonTempRight = rowTempRight.Substring(pFrom, pTo - pFrom);
				//obtain co2 json data
				pFrom = rowCo2Left.IndexOf("value:") + "value:".Length;
				pTo = rowCo2Left.LastIndexOf("}");
				string valJsonCo2Left = rowCo2Left.Substring(pFrom, pTo - pFrom);
				pFrom = rowCo2Right.IndexOf("value:") + "value:".Length;
				pTo = rowCo2Right.LastIndexOf("}");
				string valJsonCo2Right = rowCo2Right.Substring(pFrom, pTo - pFrom);
				//obtain airconditioning json data
				pFrom = rowAirConLeft.IndexOf("value:") + "value:".Length;
				pTo = rowAirConLeft.LastIndexOf("}");
				string valJsonAirConLeft = rowAirConLeft.Substring(pFrom, pTo - pFrom);
				pFrom = rowAirConRight.IndexOf("value:") + "value:".Length;
				pTo = rowAirConRight.LastIndexOf("}");
				string valJsonAirConRight = rowAirConRight.Substring(pFrom, pTo - pFrom);
				//apply visualisations from data retreived 
				//temperature gradient
				ApplyGradient (float.Parse(valJsonTempLeft, System.Globalization.CultureInfo.InvariantCulture),float.Parse(valJsonTempRight, System.Globalization.CultureInfo.InvariantCulture), true);
				//co2 grid
				ApplyGrid (float.Parse (valJsonCo2Left, System.Globalization.CultureInfo.InvariantCulture), float.Parse (valJsonCo2Right, System.Globalization.CultureInfo.InvariantCulture), true);
				//air conditioning smoke
				ApplySmokeColorChange(float.Parse (valJsonAirConLeft, System.Globalization.CultureInfo.InvariantCulture), float.Parse (valJsonAirConRight, System.Globalization.CultureInfo.InvariantCulture), true); 
				//display date and time of animation in scene
				date.text = dateJson.Replace("T"," ").Replace("Z","");
				//display temperature data figures in scene
				temperatureLeftText.text = valJsonTempLeft.Substring(0,4) +"°C";
				temperatureRightText.text = valJsonTempRight.Substring(0,4) +"°C";
				//display co2 data figures in scene
				co2LeftText.text = valJsonCo2Left.Substring(0,3) + " PPM";
				co2RightText.text = valJsonCo2Right.Substring(0,3) + " PPM";
				//display air conditioning data figures in scene
				airConLeftText.text = valJsonAirConLeft.Substring(0,4) +"°C";
				airConRightText.text = valJsonAirConRight.Substring(0,4) +"°C";
			}catch(Exception ){
				//if database acting up, throw an error
				date.text = "Error: The database has stopped cooperating, please try again";
			}
			yield return null;
		}
		extraInfo.text = "Animation Complete";
	}
	//function to convert temperature float to a RGB value
	static public Color tempToRGB(float temp){
		float red = 198f/225f;
		float green = (1000f - (temp*40))/225f;
		float blue = 29f/225f;
		Color rgbColor = new Color(red,green, blue,1F);
		return rgbColor;
	}

	//function which takes temperature data and applies appropriate colour gradient to wall materials
	public void ApplyGradient(float left, float right, bool active) { 
		tempToggle.gameObject.SetActive(true);
		if (tempToggle.isOn && active) {
			WallShader.SetColor ("_ColourLeftGradient", tempToRGB (left));
			WallShader.SetColor ("_ColourRightGradient", tempToRGB (right));
		} else {
			WallShader.SetColor("_ColourLeftGradient", new Color(0.8F,0.8F, 0.8F,1F));
			WallShader.SetColor("_ColourRightGradient", new Color(0.8F,0.8F, 0.8F,1F));
		}

	}
	//function which takes co2 data calculates appropriate density grid properties for ceiling materials
	static public float[] calculateGrid(float co2) {
		if (co2 < 300f) {
			float[] gridSettings = { 0.025f, 0.8f };
			return gridSettings;
		}  else if (co2 > 300f && co2 < 400f) {
			float[] gridSettings = { 0.05f, 0.4f };
			return gridSettings;
		}  else if (co2 > 400f && co2 < 500f) {
			float[] gridSettings = { 0.1f, 0.2f };
			return gridSettings;
		}  else if (co2 > 500f && co2 < 600f) {
			float[] gridSettings = { 0.2f, 0.1f };
			return gridSettings;
		}  else {
			float[] gridSettings = { 0.4f, 0.05f };
			return gridSettings;	
		}

	}
	//function which takes co2 data and applies appropriate density grid to ceiling materials
	public void ApplyGrid(float left, float right, bool active) { 
		Co2Toggle.gameObject.SetActive(true);
		if (Co2Toggle.isOn && active) {
			GridMain.SetFloat ("_GridLineThickness", 0.2f);
			GridMain.SetFloat ("_GridLineSpacing", 2f);

			GridLeft.SetFloat ("_GridLineThickness", calculateGrid (left) [0]);
			GridLeft.SetFloat ("_GridLineSpacing", calculateGrid (left) [1]);

			GridRight.SetFloat ("_GridLineThickness", calculateGrid (right) [0]);
			GridRight.SetFloat ("_GridLineSpacing", calculateGrid (right) [1]);
		} else {
			GridMain.SetFloat ("_GridLineThickness", 0f);
			GridLeft.SetFloat ("_GridLineThickness", 0f);
			GridRight.SetFloat ("_GridLineThickness", 0f);
		}
	}

	//function which takes air conditioning data and applies appropriate colour and smoke density to smoke GameObjects
	public void ApplySmokeColorChange(float left, float right, bool active) { 
		AirConToggle.gameObject.SetActive(true);
		if (AirConToggle.isOn && active) {
			ParticleSystem.MainModule settings1 = SmokeLeft1.GetComponent<ParticleSystem>().main;
			settings1.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonData.airConLeft) );
			settings1.startSpeed = 5.5f;
			settings1 = SmokeLeft2.GetComponent<ParticleSystem>().main;
			settings1.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonData.airConLeft) );
			settings1.startSpeed = 5.5f;
			settings1 = SmokeLeft3.GetComponent<ParticleSystem>().main;
			settings1.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonData.airConLeft) );
			settings1.startSpeed = 5.5f;
			ParticleSystem.MainModule settings2 = SmokeRight1.GetComponent<ParticleSystem>().main;
			settings2.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonData.airConRight) );
			settings2.startSpeed = 5.5f;
			settings2 = SmokeRight2.GetComponent<ParticleSystem>().main;
			settings2.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonData.airConRight) );
			settings2.startSpeed = 5.5f;
			settings2 = SmokeRight3.GetComponent<ParticleSystem>().main;
			settings2.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(GetJsonData.airConRight) );
			settings2.startSpeed = 5.5f;
		}else{
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
		}
	}
	//responce to clear button, removes all previosuly set visualisations
	public void ClearSettings() {
		ApplyGradient (0,0,false);
		ApplyGrid (0,0,false);
		ApplySmokeColorChange(0,0,false);
		date.text = "";
		extraInfo.text = temperatureLeftText.text = temperatureRightText.text = co2LeftText.text = co2RightText.text = airConLeftText.text = airConRightText.text = "";
	}

	//function which obtains the data from the database and outputs into an array
	static public string[] getDataArray(string sensor)
	{ 
		try{
			string json;
			string query = "http://smartbms01.shef.ac.uk/sensor?id="+sensor+"&start="+startTimeString+"&end="+endTimeString;

			using (var webClient = new System.Net.WebClient()) {
				json = webClient.DownloadString(query).Split('[', ']')[3];
			}
			if(json.Length > 0){
				var jsonArray = json.Split('{');

				for (var i = 0; i < jsonArray.Length; i++)
				{
					jsonArray[i] = jsonArray[i].Replace("\"", "");
				}
				return jsonArray;
			}
			else{
				string[] emptyData = {"","no data"};
				return emptyData;
			}
		}catch(Exception ){
			string[] badConnection = {"","no connection"};
			return badConnection;
		}
	}
}
