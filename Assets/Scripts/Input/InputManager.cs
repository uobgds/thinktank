using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public InputModule input;

    [SerializeField]
    private GameObject gameInfo;

    private GameManager gameManager;

    public enum InputType { KEYBOARD };


    // Use this for initialization
    void Start()
    {
        gameManager = gameInfo.GetComponent<GameManager>();
    }

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
        LINE, SQUIGGLY_LINE, U_SHAPE
    }


    public static readonly int NUMBEROFSHAPES = Enum.GetNames(typeof(RobotShape)).Length;

    public struct RobotInput
    {
        public Vector2 position;
        public RobotShape shape;
        public float rotation;
    }

    public RobotInput getRobotInput()
    {
        RobotInput inputInfo = new RobotInput();
        inputInfo.position = input.position;
        inputInfo.shape = (RobotShape) input.shapeNum;
        inputInfo.rotation = input.rotation;
        return inputInfo;
    }
	
	// Update is called once per frame
	void Update () {
        if (input != null)
        {
            input.updateInput();
        }

    }
}
