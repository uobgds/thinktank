using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SophieTestingScript : MonoBehaviour {

    public GameObject inp;
    InputManager input;

	// Use this for initialization
	void Start ()
    {
        input = inp.GetComponent<InputManager>();
        input.ChangeInputType(InputManager.InputType.KEYBOARD);
	}
	
	// Update is called once per frame
	void Update ()
    {
        InputManager.RobotInput myInput = input.getRobotInput();
        Debug.Log("My X : " + myInput.position[0]);
        Debug.Log("My Y : " + myInput.position[1]);
        Debug.Log("My Shape : " + myInput.shape);

    }
}
