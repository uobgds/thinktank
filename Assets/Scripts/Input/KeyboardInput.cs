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

        switch((InputManager.RobotShape) shapeNum)
        {
            case InputManager.RobotShape.LINE:

                break;

            case InputManager.RobotShape.SQUIGGLY_LINE:

                moveHorizontal = Input.GetAxis("Horizontal");

                if (moveHorizontal < 0)
                {
                    rotation = rotation + 5;
                }

                if (moveHorizontal > 0)
                {
                    rotation = rotation - 5;
                }

                break;

            case InputManager.RobotShape.U_SHAPE:
                moveVertical = Input.GetAxis("Vertical");

                if (moveVertical <= 0)
                    break;

                float speed = moveVertical * MAX_SPEED;

                Vector2 directionVector = getForwardVector(speed);

                position[0] = position[0] + directionVector[0];
                position[1] = position[1] + directionVector[1];
                break;
        }
    }

}



