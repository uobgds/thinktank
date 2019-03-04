using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour {

    public static List<GoalArea> goals = new List<GoalArea>();

    [SerializeField]
    private float rate = 0.5f;

    private float completion = 0;

    private float replicationTimer;

    void Start()
    {
        goals.Add(this);
    }

    void Update()
    {
        replicationTimer += Time.deltaTime;
        if(replicationTimer >= GameManager.GetBacteriaReplicationRate())
        {
            replicate();
            replicationTimer = 0;
        }
    }

    private void replicate()
    {
        Vector3 spawnPos = GetCloneSpawnPosition();
        Debug.Log("Replicating at " + spawnPos);
        Instantiate(this.gameObject, spawnPos, transform.rotation);
    }

    private Vector3 GetCloneSpawnPosition()
    {
        float xRand = Random.Range(0f, 1f);
        float yRand = Random.Range(0f, 1f);
        int xSign = (xRand > 0.5f ? -1 : 1);
        int ySign = (yRand > 0.5f ? -1 : 1);
        float xPos = Mathf.Clamp(transform.position.x + xSign * Random.Range(0.5f, 3f), -3, 3);
        float yPos = Mathf.Clamp(transform.position.y + ySign * Random.Range(0.5f, 3f), -3, 3);

        Debug.Log(xRand + " : " + xSign + " ; " + yRand +" : " + ySign);
        return new Vector3(xPos, yPos);
    }

    void OnDestroy()
    {
        goals.Remove(this);
    }

    public float GetSatisfaction()
    {
        return completion;
    }

    void OnTriggerStay2D(Collider2D other)
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
        transform.localScale = new Vector3(1,1,1)*(1 - completion);
    }
}
