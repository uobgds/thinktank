using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private Vector2 position;

    private readonly float MAX_SPEED = 10f;

    private int shapeNum;


    public enum RobotShape{
        SQUIGGLY_LINE, LINE, U_SHAPE
    }

    private int numberOfShapes = Enum.GetNames(typeof(RobotShape)).Length;

    public struct RobotInput
    {
        public Vector2 position;
        public RobotShape shape;
    }

    public RobotInput getRobotInput()
    {
        RobotInput input = new RobotInput();
        input.position = position;
        input.shape = (RobotShape)shapeNum;
        return input;
    }


	// Use this for initialization
	void Start () {
        position = new Vector2(0, 0);
        shapeNum = 1;
	}
	
	// Update is called once per frame
	void Update () {


        //Deals with the position of the robot
        //Input from arrow keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        float currentXVelocity = moveHorizontal * MAX_SPEED;
        float currentYVelocity = moveVertical * MAX_SPEED;

        position[0] = position[0] + currentXVelocity;
        position[1] = position[1] + currentYVelocity;

        //Deals with toggling between shapes
        if (Input.GetKeyUp("space"))
        {
            shapeNum = ++shapeNum % numberOfShapes;
            
        }





    }
}
