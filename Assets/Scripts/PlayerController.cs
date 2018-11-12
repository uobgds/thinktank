using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //BY SOPHIE

    InputManager input;
    InputManager.RobotInput currentInput;

	// Use this for initialization
	void Start () {
        input = GetComponent<InputManager>();
        input.ChangeInputType(InputManager.InputType.KEYBOARD);
	}
	
	// Update is called once per frame
	void Update () {
        currentInput = input.getRobotInput();

        transform.position = currentInput.position;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -currentInput.rotation));
	}
}
