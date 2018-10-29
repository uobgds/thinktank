using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    InputModule input;

    public enum InputType { KEYBOARD };

    public void ChangeInputType(InputType type)
    {
        switch (type)
        {
            case InputType.KEYBOARD:
                input = new KeyboardInput();
                break;
        }
    }

    public enum RobotShape
    {
        SQUIGGLY_LINE, LINE, U_SHAPE
    }


    public static readonly int NUMBEROFSHAPES = Enum.GetNames(typeof(RobotShape)).Length;

    public struct RobotInput
    {
        public Vector2 position;
        public RobotShape shape;
    }

    public RobotInput getRobotInput()
    {
        RobotInput inputInfo = new RobotInput();
        inputInfo.position = input.position;
        inputInfo.shape = (RobotShape) input.shapeNum;
        return inputInfo;
    }
	
	// Update is called once per frame
	void Update () {
        input.updateInput();

    }
}
