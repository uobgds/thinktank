using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [System.Serializable]
    private class PatrolPoint
    {
        [SerializeField]
        private Trans 
    }

    [SerializeField]
    private bool m_shouldPatrol;

    private Vector3 m_nextPosition;

    [SerializeField]
    private List<Vector2> m_patrolPoints;

    [SerializeField]
    private float m_speed;

    private void Update()
    {
        Movement();
    }

    private void Movement ()
    {
        m_nextPosition = GetNextPosition();
        transform.position += (transform.position - m_nextPosition) * Time.deltaTime * m_speed;
    }

    private Vector2 GetNextPosition ()
    {
        if (m_shouldPatrol)
        {
            return new Vector2();
        }
        else
        {
            return Pathfinding.FindPath(transform.position, PlayerController.Instance.transform.position)[0];
        }
    }

}