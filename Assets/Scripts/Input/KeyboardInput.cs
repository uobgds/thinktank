using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : InputModule {


    public override void updateInput()
    {
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
            shapeNum = ++shapeNum % InputManager.NUMBEROFSHAPES;

        }
    }

}
