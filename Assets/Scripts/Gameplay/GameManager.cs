using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExampleProject;
using System;

public class GameManager : MonoBehaviour {


    public static GameManager myManager;

    [SerializeField]
    private Vector2 goalLoc;

    [SerializeField]
    private Vector2 startPos;

    [SerializeField]
    private Vector2 enemySpawn;

    [SerializeField]
    private Vector2 yBounds;

    [SerializeField]
    private Vector2 xBounds;

    [SerializeField]
    private float completion;

    [SerializeField]
    private HealthLossRatesByDifficulty healthLossRatesByDifficulty;

    private Database db = new Database();

    public Vector2 ClampInRange(Vector2 myPosition)
    {
        float newX = Mathf.Clamp(myPosition[0], xBounds[0], xBounds[1]);
        float newY = Mathf.Clamp(myPosition[1], yBounds[0], yBounds[1]);


        return new Vector2(newX, newY);
    }

    public Vector2 GetTopLeft()
    {
        return new Vector3(xBounds[0],yBounds[1],0);
    } 
     

	// Use this for initialization
	void Start () {
        myManager = this;
        db.Start();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // check completion
        CheckCompletion();
        if(completion == 1)
        {
            object time = null;//unimplemented
            object hp = null;//get health of host
            object destruction = null;//get destruction percentage of the virus
            string username = null;//unimplemented

            int score = CalculateScore(time, hp, destruction);
            db.InsertScore(username, score);
            db.GetHighScores(10); //gets top ten scores
        }
	}

    private int CalculateScore(object time, object hp, object destruction)
    {
        throw new NotImplementedException();
    }

    public static float GetHealthLossRate()
    {
        switch (GameSettings.difficulty)
        {
            case Difficulty.Tutorial:
                return myManager.healthLossRatesByDifficulty.TutorialHealthLossRate;
            case Difficulty.Easy:
                return myManager.healthLossRatesByDifficulty.EasyHealthLossRate;
            case Difficulty.Medium:
                return myManager.healthLossRatesByDifficulty.MediumHealthLossRate;
            case Difficulty.Hard:
                return myManager.healthLossRatesByDifficulty.HardHealthLossRate;
            default:
                Debug.LogError("Difficulty is not set!");
                return 0f;
        }
    }

    void CheckCompletion()
    {
        float total = 0;
        List<GoalArea> goals = GoalArea.goals;
        for(int i = 0; i < goals.Count; i++)
        {
            total += goals[i].GetSatisfaction();
        }
        if (goals.Count == 0)
        {
            completion = 1;
        }
        else
        {
            completion = total / goals.Count;
        }
    }

    public float GetCompletionPercent()
    {
        return completion;
    }
}

[System.Serializable]
public struct HealthLossRatesByDifficulty
{
    public float TutorialHealthLossRate;
    public float EasyHealthLossRate;
    public float MediumHealthLossRate;
    public float HardHealthLossRate;
}