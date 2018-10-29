using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputModule {

    protected readonly float MAX_SPEED = 10f;

    public Vector2 position;

    public int shapeNum;

    public abstract void updateInput();

    private void Start()
    {
        position = new Vector2(0, 0);
        shapeNum = 1;
    }




}
