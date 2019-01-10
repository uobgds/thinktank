using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntidoteSource : MonoBehaviour {

    public float rate = 1.618f;

    void OnTriggerStay(Collider other)
    {
        PlayerController pl = other.GetComponent<PlayerController>();

        if (pl == null)
        {
            return;
        }

        pl.FillAntidote(rate);
    }
}
