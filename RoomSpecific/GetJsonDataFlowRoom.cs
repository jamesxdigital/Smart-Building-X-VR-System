//Copyright 2018, James Milton, All rights reserved.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetJsonDataFlowRoom : MonoBehaviour {
	//text object displaying the date
	public Text date;
	//text object where error messages are displayed
	public Text extraInfo;
	//text objects temperature data values
	public TextMesh tempRightCentreText;
	public TextMesh tempRightRightText;
	public TextMesh tempRightLeftText;
	public TextMesh tempLeftCentreText;
	public TextMesh tempLeftRightText;
	public TextMesh tempLeftLeftText;
	public TextMesh tempCentreText;
	//text objects co2 data values
	public TextMesh co2LeftMeshText;
	public TextMesh co2RightMeshText;
	//text objects air condioning data values
	public TextMesh airCon1TextMesh;
	public TextMesh airCon2TextMesh;
	public TextMesh airCon3TextMesh;
	public TextMesh airCon4TextMesh;
	public TextMesh airCon5TextMesh;
	public TextMesh airCon6TextMesh;
	public TextMesh airCon7TextMesh;
	public TextMesh airCon8TextMesh;
	//GameObject wall materials to manipulate
	public Material BackWall;
	public Material FrontWall;
	public Material LeftWall;
	public Material RightWall;
	//GameObject ceiling materials to manipulate
	public Material GridMain;
	public Material GridLeft;
	public Material GridRight;
	//Smoke GameObjects to manipulate
	public GameObject Smoke1;
	public GameObject Smoke2;
	public GameObject Smoke3;
	public GameObject Smoke4;
	public GameObject Smoke5;
	public GameObject Smoke6;
	public GameObject Smoke7;
	public GameObject Smoke8;
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
		if (getDataArray("BMS-L11O30S7")[1] == "bad connection"){
			date.text = "Error: No connection to shef.ac.uk network";
			//remove any text on scene
			extraInfo.text = tempRightCentreText.text = tempRightRightText.text = tempRightLeftText.text = tempLeftCentreText.text = tempLeftRightText.text = tempLeftLeftText.text = tempCentreText.text = co2LeftMeshText.text = co2RightMeshText.text = "";
			airCon1TextMesh.text = airCon2TextMesh.text = airCon3TextMesh.text = airCon4TextMesh.text = airCon5TextMesh.text = airCon6TextMesh.text = airCon7TextMesh.text = airCon8TextMesh.text = "";
			//remove gradients from wall 
			ApplyGradient (0f,0f,0f,0f,0f,0f,0f, false);
			//remove grids from ceiling 
			ApplyGrid (0f,0f, false);
			//remove smoke colour effects
			ApplySmokeColorChange(0f,0f,0f,0f,0f,0f,0f,0f,false); 
		}
		//if data available, commence annimation Coroutine
		else if (getDataArray ("BMS-L11O30S7").Length > 2) {
			StartCoroutine (GenerateGradient ());
		} 
		//if the selected date does not contain any data, throw an error message
		else {
			date.text = "Error: No data for this day";
			//remove any text on scene
			extraInfo.text = tempRightCentreText.text = tempRightRightText.text = tempRightLeftText.text = tempLeftCentreText.text = tempLeftRightText.text = tempLeftLeftText.text = tempCentreText.text = co2LeftMeshText.text = co2RightMeshText.text = "";
			airCon1TextMesh.text = airCon2TextMesh.text = airCon3TextMesh.text = airCon4TextMesh.text = airCon5TextMesh.text = airCon6TextMesh.text = airCon7TextMesh.text = airCon8TextMesh.text = "";
			//remove gradients from wall
			ApplyGradient (0f,0f,0f,0f,0f,0f,0f, false);
			//remove grids from ceiling 
			ApplyGrid (0f,0f, false);
			//remove smoke colour effects
			ApplySmokeColorChange(0f,0f,0f,0f,0f,0f,0f,0f,false); 
		}
	}


	IEnumerator GenerateGradient()
	{
		extraInfo.text = "Press 'C' to exit animation";
		//obtain temperature data for sensor for whole day and store in an array
		string[] L12O37S1 = getDataArray ("BMS-L12O37S1");
		string[] L12O37S3 = getDataArray ("BMS-L12O37S3");
		string[] L12O37S2 = getDataArray ("BMS-L12O37S2");
		string[] L11O30S1 = getDataArray ("BMS-L11O30S1");
		string[] L11O30S6 = getDataArray ("BMS-L11O30S6");
		string[] L11O30S2 = getDataArray ("BMS-L11O30S2");
		string[] L12O37S6 = getDataArray ("BMS-L12O37S6");
		//obtain co2 data for sensor for whole day and store in an array
		string[] L11O30S7 = getDataArray ("BMS-L11O30S7");
		string[] L12O37S8 = getDataArray ("BMS-L12O37S8");
		//obtain air condioning data for sensor for whole day and store in an array
		string[] L11O32S3 = getDataArray ("BMS-L11O32S3");
		string[] L11O33S3 = getDataArray ("BMS-L11O33S3");
		string[] L11O34S3 = getDataArray ("BMS-L11O34S3");
		string[] L11O35S3 = getDataArray ("BMS-L11O35S3");
		string[] L11O36S3 = getDataArray ("BMS-L11O36S3");
		string[] L11O37S3 = getDataArray ("BMS-L11O37S3");
		string[] L11O38S3 = getDataArray ("BMS-L11O38S3");
		string[] L11O39S3 = getDataArray ("BMS-L11O39S3");
		//begin iteration
		for (var i = 1; i < getDataArray("BMS-L12O37S1").Length; i=i+1){
			try{
				if (Input.GetKeyDown(KeyCode.C))
				{
					break;
				}
				//obtain indiviual pieces of temperature data from array
				string rowTempRightCentre = L12O37S1[i];
				string rowTempRightRight = L12O37S3[i];
				string rowTempRightLeft = L12O37S2[i];
				string rowTempLeftCentre = L11O30S1[i];
				string rowTempLeftRight = L11O30S6[i];
				string rowTempLeftLeft = L11O30S2[i];
				string rowTempCentre = L12O37S6[i];
				//obtain indiviual pieces of co2 data from array
				string rowCo2Left = L11O30S7[i];
				string rowCo2Right= L12O37S8[i];
				//obtain indiviual pieces of air conditoning data from array
				string rowAirCon1= L11O32S3[i];
				string rowAirCon2= L11O33S3[i];
				string rowAirCon3= L11O34S3[i];
				string rowAirCon4= L11O35S3[i];
				string rowAirCon5= L11O36S3[i];
				string rowAirCon6= L11O37S3[i];
				string rowAirCon7= L11O38S3[i];
				string rowAirCon8= L11O39S3[i];
				//obtain date and time values
				int pFrom = rowTempRightCentre.IndexOf("datetime:") + "datetime:".Length;
				int pTo = rowTempRightCentre.LastIndexOf("Z,time:");
				string dateJson = rowTempRightCentre.Substring(pFrom, pTo - pFrom);
				//obtain temperature json data
				pFrom = rowTempRightCentre.IndexOf("value:") + "value:".Length;
				pTo = rowTempRightCentre.LastIndexOf("}");
				string valJsonTempRightCentre = rowTempRightCentre.Substring(pFrom, pTo - pFrom);
				pFrom = rowTempRightRight.IndexOf("value:") + "value:".Length;
				pTo = rowTempRightRight.LastIndexOf("}");
				string valJsonTempRightRight = rowTempRightRight.Substring(pFrom, pTo - pFrom);
				pFrom = rowTempRightLeft.IndexOf("value:") + "value:".Length;
				pTo = rowTempRightLeft.LastIndexOf("}");
				string valJsonTempRightLeft = rowTempRightLeft.Substring(pFrom, pTo - pFrom);
				pFrom = rowTempLeftCentre.IndexOf("value:") + "value:".Length;
				pTo = rowTempLeftCentre.LastIndexOf("}");
				string valJsonTempLeftCentre = rowTempLeftCentre.Substring(pFrom, pTo - pFrom);
				pFrom = rowTempLeftRight.IndexOf("value:") + "value:".Length;
				pTo = rowTempLeftRight.LastIndexOf("}");
				string valJsonTempLeftRight = rowTempLeftRight.Substring(pFrom, pTo - pFrom);
				pFrom = rowTempLeftLeft.IndexOf("value:") + "value:".Length;
				pTo = rowTempLeftLeft.LastIndexOf("}");
				string valJsonTempLeftLeft = rowTempLeftLeft.Substring(pFrom, pTo - pFrom);
				pFrom = rowTempCentre.IndexOf("value:") + "value:".Length;
				pTo = rowTempCentre.LastIndexOf("}");
				string valJsonTempCentre = rowTempCentre.Substring(pFrom, pTo - pFrom);
				//obtain co2 json data
				pFrom = rowCo2Left.IndexOf("value:") + "value:".Length;
				pTo = rowCo2Left.LastIndexOf("}");
				string valJsonCo2Left = rowCo2Left.Substring(pFrom, pTo - pFrom);
				pFrom = rowCo2Right.IndexOf("value:") + "value:".Length;
				pTo = rowCo2Right.LastIndexOf("}");
				string valJsonCo2Right = rowCo2Right.Substring(pFrom, pTo - pFrom);
				//obtain airconditioning json data
				pFrom = rowAirCon1.IndexOf("value:") + "value:".Length;
				pTo = rowAirCon1.LastIndexOf("}");
				string valJsonAirCon1 = rowAirCon1.Substring(pFrom, pTo - pFrom);
				pFrom = rowAirCon2.IndexOf("value:") + "value:".Length;
				pTo = rowAirCon2.LastIndexOf("}");
				string valJsonAirCon2 = rowAirCon2.Substring(pFrom, pTo - pFrom);
				pFrom = rowAirCon3.IndexOf("value:") + "value:".Length;
				pTo = rowAirCon3.LastIndexOf("}");
				string valJsonAirCon3 = rowAirCon3.Substring(pFrom, pTo - pFrom);
				pFrom = rowAirCon4.IndexOf("value:") + "value:".Length;
				pTo = rowAirCon4.LastIndexOf("}");
				string valJsonAirCon4 = rowAirCon4.Substring(pFrom, pTo - pFrom);
				pFrom = rowAirCon5.IndexOf("value:") + "value:".Length;
				pTo = rowAirCon5.LastIndexOf("}");
				string valJsonAirCon5 = rowAirCon5.Substring(pFrom, pTo - pFrom);
				pFrom = rowAirCon6.IndexOf("value:") + "value:".Length;
				pTo = rowAirCon6.LastIndexOf("}");
				string valJsonAirCon6 = rowAirCon6.Substring(pFrom, pTo - pFrom);
				pFrom = rowAirCon7.IndexOf("value:") + "value:".Length;
				pTo = rowAirCon7.LastIndexOf("}");
				string valJsonAirCon7 = rowAirCon7.Substring(pFrom, pTo - pFrom);
				pFrom = rowAirCon8.IndexOf("value:") + "value:".Length;
				pTo = rowAirCon8.LastIndexOf("}");
				string valJsonAirCon8 = rowAirCon8.Substring(pFrom, pTo - pFrom);
				//apply visualisations from data retreived 
				//temperature gradient
				ApplyGradient (float.Parse(valJsonTempCentre, System.Globalization.CultureInfo.InvariantCulture),float.Parse(valJsonTempRightLeft, System.Globalization.CultureInfo.InvariantCulture),
					float.Parse(valJsonTempRightCentre, System.Globalization.CultureInfo.InvariantCulture),float.Parse(valJsonTempRightRight, System.Globalization.CultureInfo.InvariantCulture),
					float.Parse(valJsonTempLeftLeft, System.Globalization.CultureInfo.InvariantCulture),float.Parse(valJsonTempLeftCentre, System.Globalization.CultureInfo.InvariantCulture),
					float.Parse(valJsonTempLeftRight, System.Globalization.CultureInfo.InvariantCulture), true);
				//co2 grid
				ApplyGrid (float.Parse (valJsonCo2Left, System.Globalization.CultureInfo.InvariantCulture), float.Parse (valJsonCo2Right, System.Globalization.CultureInfo.InvariantCulture), true);
				//air conditioning smoke
				ApplySmokeColorChange(float.Parse (valJsonAirCon1, System.Globalization.CultureInfo.InvariantCulture), float.Parse (valJsonAirCon2, System.Globalization.CultureInfo.InvariantCulture), 
					float.Parse (valJsonAirCon3, System.Globalization.CultureInfo.InvariantCulture), float.Parse (valJsonAirCon4, System.Globalization.CultureInfo.InvariantCulture), 
					float.Parse (valJsonAirCon5, System.Globalization.CultureInfo.InvariantCulture), float.Parse (valJsonAirCon6, System.Globalization.CultureInfo.InvariantCulture),
					float.Parse (valJsonAirCon7, System.Globalization.CultureInfo.InvariantCulture), float.Parse (valJsonAirCon8, System.Globalization.CultureInfo.InvariantCulture),true); 
				//display date and time of animation in scene
				date.text = dateJson.Replace("T"," ").Replace("Z","");
				//display temperature data figures in scene


				tempRightCentreText.text = valJsonTempRightCentre.Substring(0,4) +"°C";
				tempRightRightText.text = valJsonTempRightRight.Substring(0,4) +"°C";
				tempRightLeftText.text = valJsonTempRightLeft.Substring(0,4) +"°C";
				tempLeftCentreText.text = valJsonTempLeftCentre.Substring(0,4) +"°C";
				tempLeftRightText.text = valJsonTempLeftRight.Substring(0,4) +"°C";
				tempLeftLeftText.text = valJsonTempLeftLeft.Substring(0,4) +"°C";
				tempCentreText.text = valJsonTempCentre.Substring(0,4) +"°C";
				//display co2 data figures in scene
				co2LeftMeshText.text = valJsonCo2Left.Substring(0,3) + " PPM";
				co2RightMeshText.text = valJsonCo2Right.Substring(0,3) + " PPM";
				//display air conditioning data figures in scene
				airCon1TextMesh.text = valJsonAirCon1.Substring(0,4) +"°C";
				airCon2TextMesh.text = valJsonAirCon2.Substring(0,4) +"°C";
				airCon3TextMesh.text = valJsonAirCon3.Substring(0,4) +"°C";
				airCon4TextMesh.text = valJsonAirCon4.Substring(0,4) +"°C";
				airCon5TextMesh.text = valJsonAirCon5.Substring(0,4) +"°C";
				airCon6TextMesh.text = valJsonAirCon6.Substring(0,4) +"°C";
				airCon7TextMesh.text = valJsonAirCon7.Substring(0,4) +"°C";
				airCon8TextMesh.text = valJsonAirCon8.Substring(0,4) +"°C";
			}catch(Exception ){
				//if database acting up, throw an error
				date.text = "Error: The database has stopped cooperating, please try again";
				break;
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
	public void ApplyGradient(float centre, float rightLeft, float rightCentre, float rightRight, float leftLeft, float leftCentre, float leftRight, bool active) { 
		tempToggle.gameObject.SetActive(true);
		if (tempToggle.isOn && active) {

			LeftWall.SetColor ("_ColourLeftGradient", tempToRGB (leftLeft));
			LeftWall.SetColor ("_ColourMiddleGradient", tempToRGB (leftCentre));
			LeftWall.SetColor ("_ColourRightGradient", tempToRGB (leftRight));

			RightWall.SetColor ("_ColourLeftGradient", tempToRGB (rightLeft));
			RightWall.SetColor ("_ColourMiddleGradient", tempToRGB (rightCentre));
			RightWall.SetColor ("_ColourRightGradient", tempToRGB (rightRight));

			BackWall.SetColor ("_ColourLeftGradient", tempToRGB (rightRight));
			BackWall.SetColor ("_ColourMiddleGradient", tempToRGB (rightRight));
			BackWall.SetColor ("_ColourRightGradient", tempToRGB (leftLeft));

			FrontWall.SetColor ("_ColourLeftGradient", tempToRGB (leftRight));
			FrontWall.SetColor ("_ColourRightGradient", tempToRGB (rightLeft));
		} else {
			
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
	public void ApplySmokeColorChange(float air1, float air2, float air3, float air4, float air5, float air6, float air7, float air8, bool active) { 
		AirConToggle.gameObject.SetActive(true);
		if (AirConToggle.isOn && active) {

			ParticleSystem.MainModule settings1 = Smoke1.GetComponent<ParticleSystem>().main;
			settings1.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air1) );
			settings1.startSpeed = 5.5f;

			ParticleSystem.MainModule settings2 = Smoke2.GetComponent<ParticleSystem>().main;
			settings2.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air2) );
			settings2.startSpeed = 5.5f;

			ParticleSystem.MainModule settings3 = Smoke3.GetComponent<ParticleSystem>().main;
			settings3.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air3) );
			settings3.startSpeed = 5.5f;

			ParticleSystem.MainModule settings4 = Smoke4.GetComponent<ParticleSystem>().main;
			settings4.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air4) );
			settings4.startSpeed = 5.5f;

			ParticleSystem.MainModule settings5 = Smoke5.GetComponent<ParticleSystem>().main;
			settings5.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air5) );
			settings5.startSpeed = 5.5f;

			ParticleSystem.MainModule settings6 = Smoke6.GetComponent<ParticleSystem>().main;
			settings6.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air6) );
			settings6.startSpeed = 5.5f;

			ParticleSystem.MainModule settings7 = Smoke7.GetComponent<ParticleSystem>().main;
			settings7.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air7) );
			settings7.startSpeed = 5.5f;

			ParticleSystem.MainModule settings8 = Smoke8.GetComponent<ParticleSystem>().main;
			settings8.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air8) );
			settings8.startSpeed = 5.5f;


		}else{
			ParticleSystem.MainModule settings1 = Smoke1.GetComponent<ParticleSystem>().main;
			settings1.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air1) );
			settings1.startSpeed = 0f;

			ParticleSystem.MainModule settings2 = Smoke2.GetComponent<ParticleSystem>().main;
			settings2.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air2) );
			settings2.startSpeed = 0f;

			ParticleSystem.MainModule settings3 = Smoke3.GetComponent<ParticleSystem>().main;
			settings3.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air3) );
			settings3.startSpeed = 0f;

			ParticleSystem.MainModule settings4 = Smoke4.GetComponent<ParticleSystem>().main;
			settings4.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air4) );
			settings4.startSpeed = 0f;

			ParticleSystem.MainModule settings5 = Smoke5.GetComponent<ParticleSystem>().main;
			settings5.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air5) );
			settings5.startSpeed = 0f;

			ParticleSystem.MainModule settings6 = Smoke6.GetComponent<ParticleSystem>().main;
			settings6.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air6) );
			settings6.startSpeed = 0f;

			ParticleSystem.MainModule settings7 = Smoke7.GetComponent<ParticleSystem>().main;
			settings7.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air7) );
			settings7.startSpeed = 0f;

			ParticleSystem.MainModule settings8 = Smoke8.GetComponent<ParticleSystem>().main;
			settings8.startColor = new ParticleSystem.MinMaxGradient( tempToRGB(air8) );
			settings8.startSpeed = 0f;

			airCon1TextMesh.text = airCon2TextMesh.text = airCon3TextMesh.text = airCon4TextMesh.text = airCon5TextMesh.text = airCon6TextMesh.text = airCon7TextMesh.text = airCon8TextMesh.text = "";
		}
	}
	//responce to clear button, removes all previosuly set visualisations
	public void ClearSettings() {
		ApplyGradient (0,0,0,0,0,0,0,false);
		ApplyGrid (0,0,false);
		ApplySmokeColorChange(0,0,0,0,0,0,0,0,false);
		date.text = "";
		extraInfo.text = tempRightCentreText.text = tempRightRightText.text = tempRightLeftText.text = tempLeftCentreText.text = tempLeftRightText.text = tempLeftLeftText.text = tempCentreText.text = co2LeftMeshText.text = co2RightMeshText.text = "";
		airCon1TextMesh.text = airCon2TextMesh.text = airCon3TextMesh.text = airCon4TextMesh.text = airCon5TextMesh.text = airCon6TextMesh.text = airCon7TextMesh.text = airCon8TextMesh.text = "";
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
