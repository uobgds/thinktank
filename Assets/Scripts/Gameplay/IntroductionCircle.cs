using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionCircle : MonoBehaviour
{

    [SerializeField]
    private float radius = 2;
    [SerializeField]
    private float startDelay = 5f;

    private float startTime = 0;

    private Transform thisTransform;

    private bool isReady = false;

    public bool ReadyToBeginGame()
    {
        return isReady;
    }

    private void Start()
    {
        startTime = Time.time + startDelay;
        thisTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = PlayerController.playerController.GetPosition();

        Vector3 circlePos = thisTransform.position;

        float sqrDist = (playerPos - circlePos).sqrMagnitude;

        if (sqrDist < radius * radius)
        {
            startTime = Time.time + startDelay;
        }

        isReady = startTime < Time.time;
    }
}
