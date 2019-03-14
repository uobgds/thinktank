using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_HighScores : MonoBehaviour
{
    public GameObject panelHighScores;
    public GameObject panelPlayerScorePrefab;
    public int maxHighScoresLimit;

    private ArrayList highScores;

    // Start is called before the first frame update
    void Start()
    {
        highScores = GameManager.db.GetHighScores(maxHighScoresLimit);
        
        for(int i = 0; i < highScores.Count; i++)
        {
            string a = (string) highScores[i];
            string name = a.Split(' ')[0];
            string score = a.Split(' ')[1];
            string place = (i+1).ToString();
            string difficulty = a.Split(' ')[3];

            string difficultyFilter = GameSettings.difficulty.ToString();
        //    Debug.Log(difficulty);
            GameObject instantiatedPlayerScore = Instantiate(panelPlayerScorePrefab, panelHighScores.transform);
            instantiatedPlayerScore.GetComponentsInChildren<Text>()[0].text = place;
            instantiatedPlayerScore.GetComponentsInChildren<Text>()[1].text = name;
            instantiatedPlayerScore.GetComponentsInChildren<Text>()[2].text = score;

            GetComponentInChildren<Button>().onClick.AddListener(backToMainMenu);

        }
    }
    private void backToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
