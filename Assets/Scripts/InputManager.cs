using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private Vector2 Position;

    private readonly float MAX_SPEED = 10f;

    private int shapeNum;


    public enum RobotShape{
        SQUIGGLY_LINE, LINE, U_SHAPE
    }

    private int numberOfShapes = Enum.GetNames(typeof(RobotShape)).Length;


	// Use this for initialization
	void Start () {
        Position = new Vector2(0, 0);
        shapeNum = 1;
	}

    private RobotShape findShape()
    {
        return (RobotShape)shapeNum;
    }
	
	// Update is called once per frame
	void Update () {


        //Deals with the position of the robot
        //Input from arrow keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        float currentXVelocity = moveHorizontal * MAX_SPEED;
        float currentYVelocity = moveVertical * MAX_SPEED;

        Position[0] = Position[0] + currentXVelocity;
        Position[1] = Position[1] + currentYVelocity;

        //Deals with toggling between shapes
        if (Input.GetKeyUp("space"))
        {
            shapeNum++;
            if(shapeNum == numberOfShapes)
            {
                shapeNum = 0;
            }
        }





    }
}
