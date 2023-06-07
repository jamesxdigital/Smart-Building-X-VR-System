//Copyright 2018, James Milton, All rights reserved.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
    
	public void ChangeToRoom () {
        SceneManager.LoadScene("DiamondRoom");
    }
    public void ChangeToPod()
    {
		SceneManager.LoadScene("DiamondPod");
    }
}
