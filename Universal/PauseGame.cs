//Copyright 2018, James Milton, All rights reserved.
using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {
	public Transform canvas;
	public Transform pauseMenu; //contains pause menu UI
	public Transform helpMenu; //contains help menu UI
	public Transform dateMenu; //contains date and time menu UI
	public Transform animationMenu; //contains date menu UI
	// Start is the furst function called when the system starts
	void Start () {
		Time.timeScale = 0;
		Camera.main.GetComponent<camMouseLook>().paused = true;
		Cursor.visible = true;
		//when system is initiated, pause menu is active
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){ 
			Pause(); //when 'esc' key is pressed, open pause menu
		}
		if (Input.GetKeyDown(KeyCode.H)){ 
			Help(); //when 'h' key is pressed, open help menu
		}
	}
	public void Pause(){ //pause menu activated
		if (canvas.gameObject.activeInHierarchy == false){
			if (pauseMenu.gameObject.activeInHierarchy == false) {
				//deactivate all other active menus
				pauseMenu.gameObject.SetActive(true);
				helpMenu.gameObject.SetActive(false);
				dateMenu.gameObject.SetActive(false);
				animationMenu.gameObject.SetActive(false);
			}
			canvas.gameObject.SetActive(true);
			Time.timeScale = 0;
			Camera.main.GetComponent<camMouseLook>().paused = true;
			Cursor.visible = true;
		}else{ //pause menu deactivated
			canvas.gameObject.SetActive(false);
			Time.timeScale = 1;
			Camera.main.GetComponent<camMouseLook>().paused = false;
			Cursor.visible = false;
		}
	}
	public void Help(){ //help menu activated
		if (canvas.gameObject.activeInHierarchy == false){
			if (helpMenu.gameObject.activeInHierarchy == false) {
				//deactivate all other active menus
				helpMenu.gameObject.SetActive(true);
				pauseMenu.gameObject.SetActive(false);
				dateMenu.gameObject.SetActive(false);
				animationMenu.gameObject.SetActive(false);
			}
			canvas.gameObject.SetActive(true);
			Time.timeScale = 0;
			Camera.main.GetComponent<camMouseLook>().paused = true;
			Cursor.visible = true;
		}else{
			//help menu deactivated
			canvas.gameObject.SetActive(false);
			Time.timeScale = 1;
			Camera.main.GetComponent<camMouseLook>().paused = false;
			Cursor.visible = false;
		}
	}
	public void DateMenu(bool Open){ //date and time menu activated
		if (Open) {
			dateMenu.gameObject.SetActive (true);
			pauseMenu.gameObject.SetActive(false);
		}if (!Open) {
			//date and time menu deactivated and return to pause menu
			dateMenu.gameObject.SetActive (false);
			pauseMenu.gameObject.SetActive(true);
		}
	}	
	public void AnimationMenu(bool Open){ //date menu activated
		if (Open) {
			animationMenu.gameObject.SetActive (true);
			pauseMenu.gameObject.SetActive(false);
		}if (!Open) {
			//date menu deactivated and return to pause menu
			animationMenu.gameObject.SetActive (false);
			pauseMenu.gameObject.SetActive(true);
		}
	}
}

