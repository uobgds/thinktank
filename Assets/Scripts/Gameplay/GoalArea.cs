using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour {

    public static List<GoalArea> goals = new List<GoalArea>();

    [SerializeField]
    private float rate = 0.5f;

    private float completion = 0;

    void Start()
    {
        goals.Add(this);
    }

    void OnDestroy()
    {
        goals.Remove(this);
    }

    public float GetSatisfaction()
    {
        return completion;
    }

    void OnTriggerStay(Collider other)
    {
        PlayerController pl = other.GetComponent<PlayerController>();
        GameManager gm = GameManager.myManager;

        if (pl == null || gm == null)
        {
            return;
        }

        float desiredLoss = Mathf.Min(1 - completion, rate * Time.deltaTime);

        float value = pl.SubtractAntidote(desiredLoss);
        completion = Mathf.Clamp01(completion + value);
    }
}
