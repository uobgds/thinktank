﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public struct S_UI_Level
{
    public Sprite thumbnail;
    public string levelSceneName;
}

public class UI_SelectLevel : MonoBehaviour {

    public S_UI_Level[] levels;
    public string backSceneName;
    public Button bLeft, bRight, bSelect, bBack;
    public Image currentLevelImage;

    private int currentLevelIndex;
	// Use this for initialization
	void Start () {
        currentLevelIndex = 0;
        currentLevelImage.sprite = levels[currentLevelIndex].thumbnail;
        bLeft.onClick.AddListener(onLeftArrowClick);
        bRight.onClick.AddListener(onRightArrowClick);
        bSelect.onClick.AddListener(onSelectLevel);
        bBack.onClick.AddListener(onBack);
	}
	
    void onSelectLevel()
    {
        SceneManager.LoadScene(levels[currentLevelIndex].levelSceneName);
    }

    void onRightArrowClick()
    {
        if (currentLevelIndex < levels.Length - 1)
        {
            currentLevelIndex++;
            currentLevelImage.sprite = levels[currentLevelIndex].thumbnail;
        }
    }

    void onLeftArrowClick()
    {
        if (currentLevelIndex > 0)
        {
            currentLevelIndex--;
            currentLevelImage.sprite = levels[currentLevelIndex].thumbnail;
        }
    }

    void onBack()
    {
        SceneManager.LoadScene(backSceneName);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
