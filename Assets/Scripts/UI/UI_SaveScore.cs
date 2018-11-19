using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_SaveScore : MonoBehaviour {

    public int score;
    public string playerName;

    public Text scoreText;
    public InputField nameInput;

    public Button bSave;
    public Button bQuit;

    public string highScoresSceneName;
    public string quitSceneName;

	// Use this for initialization
	void Start () {
        Debug.Log("Player's score needs to be bound to UI score element!");
        scoreText.text = score.ToString();
        bSave.onClick.AddListener(onSave);
        bQuit.onClick.AddListener(onQuit);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onSave()
    {
        playerName = nameInput.text;
        SceneManager.LoadScene(highScoresSceneName);
    }

    public void onQuit()
    {
        SceneManager.LoadScene(quitSceneName);
    }
}
