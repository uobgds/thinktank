using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour {

    [SerializeField]
    private float rate = 0.5f;

    private float completion = 0;

    void Start()
    {
        // TODO register this goal area
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
        completion += value;
    }
}
