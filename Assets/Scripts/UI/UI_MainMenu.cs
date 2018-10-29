using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour {

    public string levelOneSceneName;
    public Button playGameButton, viewHighScoresButton;
    
    
	// Use this for initialization
	void Start () {
        playGameButton.onClick.AddListener(playGame);
        viewHighScoresButton.onClick.AddListener(viewHighScores);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void playGame()
    {
        SceneManager.LoadScene(levelOneSceneName);
    }
    
    void viewHighScores()
    {

    }
}
