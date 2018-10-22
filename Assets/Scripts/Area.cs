using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour {

    public enum AreaType
    {
        AreaTypeOne, AreaTypeTwo, AreaTypeThree
    }

    [SerializeField]
    private AreaType m_areaType;
    private PlayerController m_playerController;

    public delegate void OnAreaStay(AreaType m_areaType);
    public static OnAreaStay onAreaStay;

    private void OnTriggerStay2D(Collider2D collision)
    {
        m_playerController = collision.GetComponent<PlayerController>();
        if (m_playerController)
        {
            if (onAreaStay != null)
            {
                onAreaStay(m_areaType);
            }
        }
    }

}