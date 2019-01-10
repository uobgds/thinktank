using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntidoteDrainer : MonoBehaviour
{


    private float drainageRate = -0.5f;

    void OnTriggerStay(Collider other)
    {
        PlayerController pl = other.GetComponent<PlayerController>();

        if (pl == null)
        {
            return;
        }

        pl.FillAntidote(drainageRate);
        
    }
}
