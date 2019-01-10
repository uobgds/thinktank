﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Vector2 ClampInRange(Vector2 myPosition)
    {
        float newX = Mathf.Clamp(myPosition[0], xBounds[0], xBounds[1]);
        float newY = Mathf.Clamp(myPosition[1], yBounds[0], yBounds[1]);


        return new Vector2(newX, newY);
    }
     

	// Use this for initialization
	void Start () {
        myManager = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}