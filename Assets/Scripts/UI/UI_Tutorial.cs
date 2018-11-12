using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Tutorial : MonoBehaviour {

    public string levelSelectSceneName;
    public string backSceneName;
    public Button bSkipTutorial, bBack;

	// Use this for initialization
	void Start () {
        bSkipTutorial.onClick.AddListener(onSkipTutorial);
        bBack.onClick.AddListener(onBack);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void onSkipTutorial()
    {
        SceneManager.LoadScene(levelSelectSceneName);
    }

    void onBack()
    {
        SceneManager.LoadScene(backSceneName);
    }
}
