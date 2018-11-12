using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour {

    public string playGameSceneName;
    public Button bPlayGame, bViewHighScores;
    
    
	// Use this for initialization
	void Start () {
        bPlayGame.onClick.AddListener(playGame);
        bViewHighScores.onClick.AddListener(viewHighScores);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void playGame()
    {
        SceneManager.LoadScene(playGameSceneName);
    }
    
    void viewHighScores()
    {

    }
}
