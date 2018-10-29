using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectDifficulty : MonoBehaviour {

    public Button easy, medium, hard;

	// Use this for initialization
	void Start () {
        easy.onClick.AddListener(setEasy);
        medium.onClick.AddListener(setMedium);
        hard.onClick.AddListener(setHard);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void setEasy()
    {
        GameSettings.difficulty = Difficulty.Easy;
    }

    void setMedium()
    {
        GameSettings.difficulty = Difficulty.Medium;
    }

    void setHard()
    {
        GameSettings.difficulty = Difficulty.Hard;
    }
}
