//Copyright 2018, James Milton, All rights reserved.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetJsonData : MonoBehaviour {
	//text object displaying the date
	public Text date;
	//text object where error messages are displayed
	public Text extraInfo;
	//float objects for the obtained data values
	public static float tempLeft;
	public static float tempRight;
	public static float co2left;
	public static float co2right;
	public static float airConLeft;
	public static float airConRight;
	//end and start time floats
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
		if ((getData ("BMS-L11O42S21") [1]) == "no data") {
			date.text = "Error: No data for this time";
			extraInfo.text = "";
			tempLeft = tempRight = co2left = co2right = airConLeft = airConRight = 0f;
		} 
		//if the user is not connected to the universities network, throw and error
		else if ((getData ("BMS-L11O42S21") [1]) == "no connection") {
			date.text = "Error: No connection to shef.ac.uk network";
			extraInfo.text = "";
			tempLeft = tempRight = co2left = co2right = airConLeft = airConRight = 0f;
		}
		//else try to obtain data from database. Code is inside a try and except because the database can be tempermental
		else {
			try{
				//obtain data valies for left and right temperature sensors
				tempLeft = float.Parse (getData ("BMS-L11O42S21") [1], System.Globalization.CultureInfo.InvariantCulture);
				tempRight = float.Parse (getData ("BMS-L11O43S21") [1], System.Globalization.CultureInfo.InvariantCulture);
				//obtain data valies for left and right co2 sensors
				co2left = float.Parse (getData ("BMS-L11O42S1") [1], System.Globalization.CultureInfo.InvariantCulture);
				co2right = float.Parse (getData ("BMS-L11O43S1") [1], System.Globalization.CultureInfo.InvariantCulture);
				//obtain data valies for left and right air conditioning sensors
				airConLeft = float.Parse (getData ("BMS-L11O43S3") [1], System.Globalization.CultureInfo.InvariantCulture);
				airConRight = float.Parse (getData ("BMS-L11O42S3") [1], System.Globalization.CultureInfo.InvariantCulture);
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
			//retreive json data
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
