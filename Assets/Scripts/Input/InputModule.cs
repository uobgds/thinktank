using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class InputModule {

    protected readonly float MAX_SPEED = 0.1f;

    public Vector2 position;

    public float rotation;

    public int shapeNum;

    public abstract void updateInput();


   
  

    protected Vector2 getForwardVector(float speed)
    {
        float xComp = Mathf.Cos(rotation * Mathf.Deg2Rad);
        float yComp = Mathf.Sin(rotation * Mathf.Deg2Rad);

        Debug.Log(xComp + " " + yComp);

        return new Vector2(xComp * speed, yComp * speed);
    }




}
