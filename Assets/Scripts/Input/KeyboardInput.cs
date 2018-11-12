using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : InputModule {


    public override void updateInput()
    {

        float moveHorizontal;
        float moveVertical;

        //Deals with toggling between shapes
        if (Input.GetKeyUp("space"))
        {
            shapeNum = ++shapeNum % InputManager.NUMBEROFSHAPES;

        }

        //Deals with the rotation of the object, goes round in jumps up 90 degrees
        if (Input.GetKeyUp(KeyCode.R))
        {
            rotation = (rotation + 90) % 360;
        }

        switch((InputManager.RobotShape) shapeNum)
        {
            case InputManager.RobotShape.LINE:

                break;

            case InputManager.RobotShape.SQUIGGLY_LINE:

                moveHorizontal = Input.GetAxis("Horizontal");

                if (moveHorizontal < 0)
                {
                    rotation = rotation - 5;
                }

                if (moveHorizontal > 0)
                {
                    rotation = rotation + 5;
                }

                break;

            case InputManager.RobotShape.U_SHAPE:
                moveHorizontal = Input.GetAxis("Horizontal");
                moveVertical = Input.GetAxis("Vertical");

                float currentXVelocity = moveHorizontal * MAX_SPEED;
                float currentYVelocity = moveVertical * MAX_SPEED;

                position[0] = position[0] + currentXVelocity;
                position[1] = position[1] + currentYVelocity;
                break;

        }
    }

}



