using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputModule {

    protected readonly float MAX_SPEED = 0.1f;

    public Vector2 position;

    public float rotation;

    public int shapeNum;

    public abstract void updateInput();

    private void Start()
    {
        position = new Vector2(0, 0);
        shapeNum = 1;
    }




}
