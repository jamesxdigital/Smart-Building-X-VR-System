//Copyright 2018, James Milton, All rights reserved.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterController : MonoBehaviour {
	//public float can be re-configured so character
	// translates quicker across the scene
	public float speed = 10.0F;

	// Update is called once per frame
	void Update () {
		//left, right input detection from keyboard or handheld controller
		float translation = Input.GetAxis("Vertical") * speed;
		//fowards, backwards input detection from keyboard or handheld controller
		float straffe = Input.GetAxis("Horizontal") * speed;
		//up, down input detection from keyboard or handheld controller
		float jump = Input.GetAxis("Jump") * speed/10;
		//movement applied over time
		translation *= Time.deltaTime;
		straffe *= Time.deltaTime;
		//applying transformations to GameObject
		transform.Translate(straffe, jump , translation);
		//when in pause mode, unlock cursor
		if (Input.GetKeyDown("escape"))
			Cursor.lockState = CursorLockMode.None;
	}
}
