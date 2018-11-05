using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_SelectDifficulty : MonoBehaviour {

    public string tutorialSceneName;
    public Button bEasy, bMedium, bHard, bContinue;

	// Use this for initialization
	void Start () {
        bEasy.onClick.AddListener(setEasy);
        bMedium.onClick.AddListener(setMedium);
        bHard.onClick.AddListener(setHard);
        bContinue.onClick.AddListener(onContinueClick);
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

    void onContinueClick()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }
}
