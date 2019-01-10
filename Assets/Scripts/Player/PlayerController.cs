using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //BY SOPHIE


    InputManager input;
    InputManager.RobotInput currentInput;
    Animator anim;

    [SerializeField]
    private float myAntidote;

    public float GetAntidotePercent()
    {
        return myAntidote;
    }

    public void FillAntidote(float rate)
    {
        myAntidote += rate * Time.deltaTime;
        myAntidote = Mathf.Clamp01(myAntidote);
    }

    /// <summary>
    /// reduces the antidote by "amount"
    /// </summary>
    /// <param name="amount"></param>
    /// <returns> the amount of antidote subtracted</returns>
    public float SubtractAntidote(float amount)
    {
        float newAntidote = Mathf.Clamp01(myAntidote - amount);
        float diff = myAntidote - newAntidote;
        myAntidote = newAntidote;
        return diff;
    }


    // Use this for initialization
    void Start () {
        input = GetComponent<InputManager>();
        input.ChangeInputType(InputManager.InputType.KEYBOARD);
        anim = GetComponent<Animator>();
	}

 
	
	// Update is called once per frame
	void Update () {

        currentInput = input.getRobotInput();

        transform.position = currentInput.position;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentInput.rotation));

        switch (currentInput.shape)
        {
            case InputManager.RobotShape.LINE:
                anim.SetBool("snake", false);
                anim.SetBool("u", false);
                anim.SetBool("line", true);
                break;
            case InputManager.RobotShape.SQUIGGLY_LINE:
                anim.SetBool("u", false);
                anim.SetBool("line", false);
                anim.SetBool("snake", true);
                break;
            case InputManager.RobotShape.U_SHAPE:
                anim.SetBool("line", false);
                anim.SetBool("snake", false);
                anim.SetBool("u", true);
                break;
        }
	}
}
