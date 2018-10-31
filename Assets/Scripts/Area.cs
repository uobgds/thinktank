using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour {

    [SerializeField]
    private AreaDetails m_areaDetails;
    //private PlayerController m_playerController;

    public delegate void OnAreaStay(AreaDetails m_areaDetails);
    public static OnAreaStay onAreaStay;

    private void OnTriggerStay2D(Collider2D collision)
    {
        //m_playerController = collision.GetComponent<PlayerController>();
        //if (m_playerController)
        //{
        //    if (onAreaStay != null)
        //    {
        //        onAreaStay(m_areaDetails);
        //    }
        //}
    }

}

[System.Serializable]
public class AreaDetails
{
    [SerializeField]
    private float m_damagePerSecond;

    public float DamagePerSecond
    {
        get
        {
            return m_damagePerSecond;
        }
        set
        {
            if (value > 0f)
            {
                m_damagePerSecond = value;
            }
            else
            {
                m_damagePerSecond = 0f;
            }
        }
    }
}