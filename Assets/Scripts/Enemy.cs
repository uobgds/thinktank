using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Blocker {

    public GameObject tempTarget;

    [SerializeField]
    private bool m_shouldPatrol;

    [SerializeField]
    private List<Transform> m_patrolPoints;

    private Pathfinding.PathfindingInformation m_pathfindingInformation;

    [SerializeField]
    private float m_speed;

    private Vector3 m_nextPosition;
    private Vector3 m_currentPosition;

    private bool m_canReachTarget;

    private bool m_initialised;

    private void Start()
    {
        Initialise();
    }

    private void Initialise ()
    {
        m_pathfindingInformation = new Pathfinding.PathfindingInformation();

        m_currentPosition = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        m_nextPosition = m_currentPosition;
        m_canReachTarget = true;

        Pathfinding.onPathfindingMapUpdated += OnPathfindingMapUpdated;

        StartCoroutine(Movement());
    }

    private void OnDestroy()
    {
        Pathfinding.onPathfindingMapUpdated -= OnPathfindingMapUpdated;
    }

    private void OnPathfindingMapUpdated (Vector2 givenTilePosition)
    {
        if (new Vector2(m_currentPosition.x, m_currentPosition.y) != givenTilePosition * Pathfinding.Instance.m_tileSize
            && Pathfinding.Instance.m_initialised)
        {
            FindNextPosition();
        }
    }

    private IEnumerator Movement ()
    {
        while (true)
        {
            if (!Pathfinding.Instance.m_initialised)
            {
                yield return new WaitForEndOfFrame();
            }
            else if (!m_initialised)
            {
                m_initialised = true;
                Pathfinding.Instance.UpdatePathfindingMap(m_currentPosition / Pathfinding.Instance.m_tileSize, Pathfinding.TileState.Closed);
                FindNextPosition();
            }

            if (CheckForUpdatePath())
            {
                FindNextPosition();
            }

            //if distance from current pos to end tile is more than one tile away - otherwise, no movement
            if (m_canReachTarget && Vector2.Distance(m_currentPosition, tempTarget.transform.position) > Pathfinding.Instance.m_tileSize
                && m_nextPosition != transform.position)
            {
                transform.position += Vector3.Normalize(m_nextPosition - transform.position) * Time.deltaTime * m_speed;
            }

            if (!m_canReachTarget)
            {
                yield return new WaitForSeconds(1f); //if can't reach target, only re-find path every second rather than every tick
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void FindNextPosition()
    {
        m_nextPosition = GetNextPosition();

        //if we've changed position
        if (new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)) != m_currentPosition)
        {
            Pathfinding.Instance.UpdatePathfindingMap(m_currentPosition / Pathfinding.Instance.m_tileSize, Pathfinding.TileState.Open);
            m_currentPosition = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            Pathfinding.Instance.UpdatePathfindingMap(m_currentPosition / Pathfinding.Instance.m_tileSize, Pathfinding.TileState.Closed);
        }
        else
        {
            m_currentPosition = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        }
    }

    private bool CheckForUpdatePath() //we want to re-find path every time we move to a new tile, so return whether or we have moved to a new tile
    {
        return new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)) == new Vector2(m_nextPosition.x, m_nextPosition.y);
    }

    private Vector2 GetNextPosition()
    {
        if (m_shouldPatrol)
        {
            m_nextPosition = new Vector2();
        }
        else
        {
            m_pathfindingInformation = Pathfinding.Instance.FindPath(transform.position, tempTarget.transform.position);
            if (m_pathfindingInformation.m_canReachTile && m_pathfindingInformation.m_path.Count > 0)
            {
                m_canReachTarget = true;

                m_nextPosition = m_pathfindingInformation.m_path[m_pathfindingInformation.m_path.Count - 1]; //get end position in path 
            }
            else
            {
                m_canReachTarget = false;
            }
        }
        return m_nextPosition;
    }

}