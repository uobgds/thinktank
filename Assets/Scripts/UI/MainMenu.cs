﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string levelOneSceneName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void playGame()
    {
        SceneManager.LoadScene(levelOneSceneName);
    }
    
    void levelSelect()
    {

    }
}
