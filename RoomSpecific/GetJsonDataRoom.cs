//Copyright 2018, James Milton, All rights reserved.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetJsonDataRoom : MonoBehaviour {
	//text object displaying the date
	public Text date;
	//text object where error messages are displayed
	public Text extraInfo;
	//float objects for the obtained temperature data values
	public static float tempRightCentre;
	public static float tempRightRight;
	public static float tempRightLeft;
	public static float tempLeftCentre;
	public static float tempLeftRight;
	public static float tempLeftLeft;
	public static float tempCentre;
	public static float tempLeft;
	public static float tempRight;
	//float objects for the obtained co2 data values
	public static float co2left;
	public static float co2right;
	//float objects for the obtained air conditioning data values
	public static float airCon1;
	public static float airCon2;
	public static float airCon3;
	public static float airCon4;
	public static float airCon5;
	public static float airCon6;
	public static float airCon7;
	public static float airCon8;
	//end and start time  converted string values
	public static string startTimeString;
	public static string endTimeString;

	public void ApplyButton(){
		//obtain the start and end date from the DatePickerControl class
		DateTime endTimeDate = DatePickerControl.DateGlobal;
		DateTime startTimeDate = endTimeDate.AddDays(-3);
		//sterilise date object so can be passed to http query
		startTimeString = startTimeDate.Year+"-"+startTimeDate.Month.ToString("D2")+"-"+startTimeDate.Day.ToString("D2")+"T"+startTimeDate.Hour.ToString("D2")+":"+startTimeDate.Minute.ToString("D2")+":"+startTimeDate.Second.ToString("D2");
		endTimeString = endTimeDate.Year+"-"+endTimeDate.Month.ToString("D2")+"-"+endTimeDate.Day.ToString("D2")+"T"+endTimeDate.Hour.ToString("D2")+":"+endTimeDate.Minute.ToString("D2")+":"+endTimeDate.Second.ToString("D2");
		//if the selected date does not contain any data, throw an error message
		if ((getData ("BMS-L12O37S1") [1]) == "no data") {
			date.text = "Error: No data for this time";
			extraInfo.text = "";
			tempRightCentre = tempRightRight = tempRightLeft = tempLeftCentre = tempLeftRight = tempLeftLeft = tempCentre = co2left = co2right = airCon1 = airCon2 = airCon3 = airCon4 = airCon5 = airCon6 = airCon7 = airCon8 = 0f;
		} 
		//if the user is not connected to the universities network, throw and error
		else if ((getData ("BMS-L11O30S7") [1]) == "no connection") {
			date.text = "Error: No connection to shef.ac.uk network";
			extraInfo.text = "";
			tempRightCentre = tempRightRight = tempRightLeft = tempLeftCentre = tempLeftRight = tempLeftLeft = tempCentre = co2left = co2right = airCon1 = airCon2 = airCon3 = airCon4 = airCon5 = airCon6 = airCon7 = airCon8 = 0f;
		}
		//else try to obtain data from database. Code is inside a try and except because the database can be tempermental
		else {
			try{
				//obtain data valies for temperature sensors
				tempRightCentre = float.Parse (getData ("BMS-L12O37S1") [1], System.Globalization.CultureInfo.InvariantCulture);
				tempRightRight = float.Parse (getData ("BMS-L12O37S3") [1], System.Globalization.CultureInfo.InvariantCulture);
				tempRightLeft = float.Parse (getData ("BMS-L12O37S2") [1], System.Globalization.CultureInfo.InvariantCulture);
				tempLeftCentre = float.Parse (getData ("BMS-L11O30S1") [1], System.Globalization.CultureInfo.InvariantCulture);
				tempLeftRight = float.Parse (getData ("BMS-L11O30S6") [1], System.Globalization.CultureInfo.InvariantCulture);
				tempLeftLeft = float.Parse (getData ("BMS-L11O30S2") [1], System.Globalization.CultureInfo.InvariantCulture);
				tempCentre = float.Parse (getData ("BMS-L12O37S6") [1], System.Globalization.CultureInfo.InvariantCulture);
				//obtain data valies for left and right co2 sensors
				Debug.Log (getData ("BMS-L11O30S7") [1]);
				co2left = float.Parse (getData ("BMS-L11O30S7") [1], System.Globalization.CultureInfo.InvariantCulture);
				Debug.Log (co2left);
				co2right = float.Parse (getData ("BMS-L12O37S8") [1], System.Globalization.CultureInfo.InvariantCulture);
				//obtain data valies for air conditioning sensors
				airCon1 = float.Parse (getData ("BMS-L11O32S3") [1], System.Globalization.CultureInfo.InvariantCulture);
				airCon2 = float.Parse (getData ("BMS-L11O33S3") [1], System.Globalization.CultureInfo.InvariantCulture);
				airCon3 = float.Parse (getData ("BMS-L11O34S3") [1], System.Globalization.CultureInfo.InvariantCulture);
				airCon4 = float.Parse (getData ("BMS-L11O35S3") [1], System.Globalization.CultureInfo.InvariantCulture);
				airCon5 = float.Parse (getData ("BMS-L11O36S3") [1], System.Globalization.CultureInfo.InvariantCulture);
				airCon6 = float.Parse (getData ("BMS-L11O37S3") [1], System.Globalization.CultureInfo.InvariantCulture);
				airCon7 = float.Parse (getData ("BMS-L11O38S3") [1], System.Globalization.CultureInfo.InvariantCulture);
				airCon8 = float.Parse (getData ("BMS-L11O39S3") [1], System.Globalization.CultureInfo.InvariantCulture);
				//output date of closest available date from requested in the database
				date.text = "Closest date available: " + getData ("BMS-L11O42S21") [0].Replace("T"," ").Replace("Z","");
				extraInfo.text = "";
			}catch(Exception ){
				//if database acting up, throw an error
				date.text = "Error: The database has stopped cooperating, please try again";
				extraInfo.text = "";
			}

		}
	}
	//function which obtains the data from the database and outputs into an array
	static public string[] getData(string sensor)
	{ 
		try{
			string json;
			string query = "http://smartbms01.shef.ac.uk/sensor?id="+sensor+"&start="+startTimeString+"&end="+endTimeString;
			using (var webClient = new System.Net.WebClient()) {
				json = webClient.DownloadString(query).Split('[', ']')[3];
			}
			if(json.Length > 0){
				var jsonArray = json.Split('{');
				string latestJson = jsonArray[jsonArray.Length-1];
				string[] data = latestJson.Split(',');
				data = RemoveAt(data,1);

				for (var i = 0; i < data.Length; i++)
				{
					data[i] = data[i].Split('"', '"')[3];
				}
				return data;
			}
			else{
				//return special array if no data available
				string[] emptyData = {"","no data"};
				return emptyData;
			}
		}catch(Exception ){
			//return special array if user not connected to university network
			string[] badConnection = {"","no connection"};
			return badConnection;
		}

	}
	//json steriliser
	static public string[] RemoveAt(string[] source, int index)
	{
		string[] dest = new string[source.Length - 1];
		if( index > 0 )
			Array.Copy(source, 0, dest, 0, index);

		if( index < source.Length - 1 )
			Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

		return dest;
	} 
}
