using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExampleProject;
using System;

public class GameManager : MonoBehaviour
{

    public delegate void VoidEvent();
    public static event VoidEvent OnGameEnd;
    public static event VoidEvent OnGameBegin;

    public static GameManager myManager;


    private GameState gameState;

    public GameState GetGameState()
    {
        return gameState;
    }

    [SerializeField]
    private Vector2 goalLoc;

    [SerializeField]
    private IntroductionCircle introCircle;

    [SerializeField]
    private Vector2 enemySpawn;

    [SerializeField]
    private Vector2 yBounds;

    [SerializeField]
    private Vector2 xBounds;

    [SerializeField]
    private float completion;

    [SerializeField]
    private bool overrideDifficulty;

    [SerializeField]
    private Difficulty difficulty = Difficulty.Tutorial;

    [SerializeField]
    private HealthLossRatesByDifficulty healthLossRatesByDifficulty;

    [SerializeField]
    private BacteriaReplicationRatesByDifficulty bacteriaReplicationRatesByDifficulty;

    private Database db;

    public Vector2 ClampInRange(Vector2 myPosition)
    {
        float newX = Mathf.Clamp(myPosition[0], xBounds[0], xBounds[1]);
        float newY = Mathf.Clamp(myPosition[1], yBounds[0], yBounds[1]);


        return new Vector2(newX, newY);
    }

    public Vector2 GetTopLeft()
    {
        return new Vector3(xBounds[0], yBounds[1], 0);
    }


    // Use this for initialization
    void Awake()
    {
        myManager = this;
        gameState = GameState.Introduction;
        db = GetComponent<Database>();
        if (overrideDifficulty)
        {
            GameSettings.difficulty = difficulty;
        }
        // TODO implement this.
        //db.Start();
    }

    private void Start()
    {
        introCircle = FindObjectOfType<IntroductionCircle>();
    }

    void BeginGame()
    {
        gameState = GameState.Playing;
        if (OnGameBegin != null)
        {
            OnGameBegin();
        }
        
    }

    void EndGame()
    {
        if(OnGameEnd != null)
        {
            OnGameEnd();
        }
    }

	// Update is called once per frame
	void Update () {
        switch (gameState)
        {
            case GameState.Introduction:
                IntroUpdate();
                break;
            case GameState.Playing:
                PlayingUpdate();
                break;
            case GameState.Finished:
                break;
        }
        // check completion
       
        
	}

    private void PlayingUpdate()
    {
        CheckCompletion();
        if (completion == 1)
        {
            gameState = GameState.Finished;
            object time = null;//unimplemented
            object hp = null;//get health of host
            object destruction = null;//get destruction percentage of the virus
            string username = null;//unimplemented

            PlayerController.playerController.endGame();

            int score = CalculateScore(time, hp, destruction);
            db.InsertScore(username, score);
            db.GetHighScores(10); //gets top ten scores
        }
    }
    private void IntroUpdate()
    {
        // TODO check if player is in intro circle
        if (introCircle.ReadyToBeginGame())
        {
            BeginGame();
        }

    }

    //There is a score formula in the notes for this, right now health is all I have access to so I
    //am using that.
    private int CalculateScore(object time, object hp, object destruction)
    {
        return (int)(PlayerController.playerController.GetHealthPercent() * 100);
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

    public static float GetBacteriaReplicationRate()
    {
        switch (GameSettings.difficulty)
        {
            case Difficulty.Tutorial:
                return myManager.bacteriaReplicationRatesByDifficulty.TutorialReplicationRate;
            case Difficulty.Easy:
                return myManager.bacteriaReplicationRatesByDifficulty.EasyReplicationRate;
            case Difficulty.Medium:
                return myManager.bacteriaReplicationRatesByDifficulty.MediumReplicationRate;
            case Difficulty.Hard:
                return myManager.bacteriaReplicationRatesByDifficulty.HardReplicationRate;
            default:
                Debug.LogError("Difficulty is not set!");
                return 0f;
        }
    }

    void CheckCompletion()
    {
        float total = 0;
        List<GoalArea> goals = GoalArea.goals;
        for (int i = 0; i < goals.Count; i++)
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

[System.Serializable]
public struct BacteriaReplicationRatesByDifficulty
{
    public float TutorialReplicationRate;
    public float EasyReplicationRate;
    public float MediumReplicationRate;
    public float HardReplicationRate;
}