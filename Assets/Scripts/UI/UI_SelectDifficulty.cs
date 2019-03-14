using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_SelectDifficulty : MonoBehaviour {

    public string tutorialSceneName;
    public string backSceneName;
    public Button bEasy, bMedium, bHard, bBack;

	// Use this for initialization
	void Start () {
        bEasy.onClick.AddListener(setEasy);
        bMedium.onClick.AddListener(setMedium);
        bHard.onClick.AddListener(setHard);
        bBack.onClick.AddListener(onBack);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void setEasy()
    {
        GameSettings.difficulty = Difficulty.Easy;
        SceneManager.LoadScene("Easy_Level");
    }

    void setMedium()
    {
        GameSettings.difficulty = Difficulty.Medium;
        SceneManager.LoadScene("Medium_Level");
    }

    void setHard()
    {
        GameSettings.difficulty = Difficulty.Hard;
        SceneManager.LoadScene("Difficult_Level");
    }

    void onContinue()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }

    void onBack()
    {
        SceneManager.LoadScene(backSceneName);
    }
}
