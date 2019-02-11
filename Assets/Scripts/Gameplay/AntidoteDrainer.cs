using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntidoteDrainer : MonoBehaviour
{

 //   [SerializeField]
 //   private float drainageRate = -0.125f;

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController pl = other.GetComponent<PlayerController>();

        if (pl == null)
        {
            return;
        }
        
        pl.DamageHealth(GameManager.GetHealthLossRate() * Time.deltaTime);
 //       float speed = pl.GetDrainSpeed();
 //       drainageRate -= speed;
 //       pl.FillAntidote(drainageRate);
        
    }
}
