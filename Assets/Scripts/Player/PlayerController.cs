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
    [SerializeField]
    [Range(0,1)]
    private float myHealth;

    [SerializeField]
    private float decayRate = 0.015f;
    private bool moving;
    private Vector2 lastPos;
    [SerializeField]
    private float moveDrain = 0.3f;

    private Transform thisTransform;

    public static PlayerController player;

    private void Awake()
    {
        player = this;
        thisTransform = GetComponent<Transform>();
    }

    private void OnDestroy()
    {
        if (player == this)
        {
            player = null;
        }
    }

    public Vector3 GetPosition()
    {
        return thisTransform.position;
    }

    public float GetAntidotePercent()
    {
        return myAntidote;
    }

    public float GetHealthPercent()
    {
        return myHealth;
    }

    public void DamageHealth(float damage)
    {
        Debug.Log(damage);
        myHealth = Mathf.Clamp01(myHealth - damage);
    }

    internal float GetDrainSpeed()
    {
        if (moving)
        {
            return moveDrain * Time.deltaTime;
        }
        return 0;
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
        input.input.position = GameManager.myManager.GetTopLeft();
	}

    private void FixedUpdate()
    {
        myAntidote -= decayRate * Time.deltaTime;
        myAntidote = Mathf.Clamp01(myAntidote);
    }

    // Update is called once per frame
    void Update () {

        currentInput = input.getRobotInput();

        transform.position = currentInput.position;
        float dist = (lastPos - currentInput.position).sqrMagnitude;
        moving = !Mathf.Approximately(dist, 0);
        lastPos = currentInput.position;

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
