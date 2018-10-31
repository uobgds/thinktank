using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Blocker {

    [SerializeField]
    private bool m_shouldPatrol;

    private Vector3 m_nextPosition;

    [SerializeField]
    private List<Transform> m_patrolPoints;

    [SerializeField]
    private float m_speed;

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        m_nextPosition = GetNextPosition();
        transform.position += (transform.position - m_nextPosition) * Time.deltaTime * m_speed;
    }

    private Vector2 GetNextPosition()
    {
        if (m_shouldPatrol)
        {
            return new Vector2();
        }
        else
        {
            return Pathfinding.Instance.FindPath(transform.position, new Vector2())[0];
        }
    }

}