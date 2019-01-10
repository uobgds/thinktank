using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntidoteSource : MonoBehaviour {

    public float rate = 1.618f;

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("SOURCE");
        PlayerController pl = other.GetComponent<PlayerController>();

        if (pl == null)
        {
            return;
        }

        pl.FillAntidote(rate);
    }
}
