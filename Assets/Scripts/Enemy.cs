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

    public int m_priority; //each enemy has a priority - if the map is updated by someone of lower priority than you, you must recalculate your path

    [SerializeField]
    private float m_speed;

    private Vector3 m_nextPosition;
    private Vector3 m_currentPosition;

    private bool m_canReachTarget;

    private bool m_initialised;

    private float m_forceRecalcPathTimer;

    private void Start()
    {
        Initialise();
    }

    private void Initialise ()
    {
        m_pathfindingInformation = new Pathfinding.PathfindingInformation();

        m_currentPosition = RoundVector(transform.position);
        m_nextPosition = m_currentPosition;
        m_canReachTarget = true;

        Pathfinding.onPathfindingMapUpdated += OnPathfindingMapUpdated;

        StartCoroutine(Movement());
    }

    private void OnDestroy()
    {
        Pathfinding.onPathfindingMapUpdated -= OnPathfindingMapUpdated;
    }

    private void OnPathfindingMapUpdated (Vector2 givenTilePosition, Enemy sender)
    {
        if (sender && sender != this && sender.m_priority < m_priority
            && Pathfinding.Instance.m_initialised)
        {
            if (RoundVector(sender.transform.position, true) != RoundVector(transform.position, true))
            {
                FindNextPosition();
            }
            //if the sender is in the same position as you, need to wait
            else
            {
                m_pathfindingInformation.m_path.Clear();
            }
        }
    }

    private IEnumerator Movement ()
    {
        while (true)
        {
            m_forceRecalcPathTimer += Time.deltaTime;
            if (!Pathfinding.Instance.m_initialised)
            {
                yield return new WaitForEndOfFrame();
            }
            else if (!m_initialised)
            {
                m_initialised = true;
                Pathfinding.Instance.UpdatePathfindingMap(RoundVector(m_currentPosition, true), Pathfinding.TileState.Closed, this);
                FindNextPosition();
            }

            //every time we reach a new grid position, recalculate path, also force a path recalculation every second
            if ((CheckForUpdatePath() || m_forceRecalcPathTimer > 1f))
            {
                m_forceRecalcPathTimer = 0f;
                FindNextPosition();
            }
            if (m_pathfindingInformation.m_path.Count == 0)
            {
                Pathfinding.Instance.UpdatePathfindingMap(RoundVector(transform.position, true), Pathfinding.TileState.Closed, this);
            }

            //if distance from current pos to end tile is more than one tile away - otherwise, no movement
            else if (m_canReachTarget && !TilesAdjacent(m_pathfindingInformation.m_path[0], transform.position))
            {
                transform.position += Vector3.Normalize(m_nextPosition - transform.position) * Time.deltaTime * m_speed;
            }
            else
            {
                m_pathfindingInformation.m_path.Clear();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private bool TilesAdjacent (Vector3 vectorOne, Vector3 vectorTwo)
    {
        Vector2 arrayPosOne = RoundVector(vectorOne, true);
        Vector2 arrayPosTwo = RoundVector(vectorTwo, true);

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                //if current pos is adjacent to end tile
                if (arrayPosOne + new Vector2(x,y) == arrayPosTwo)
                {
                    //and if our actual position is close to our grid position, want to pause movement
                    if (Vector2.Distance(vectorTwo, arrayPosTwo * Pathfinding.Instance.m_tileSize) < 0.1f)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private Vector2 RoundVector (Vector3 givenVector, bool returnAsArrayPos = false)
    {
        Vector2 tempVector = new Vector2(Mathf.RoundToInt(givenVector.x / Pathfinding.Instance.m_tileSize),
                Mathf.RoundToInt(givenVector.y / Pathfinding.Instance.m_tileSize));

        if (!returnAsArrayPos)
        {
            return tempVector * Pathfinding.Instance.m_tileSize;
        }
        else
        {
            return tempVector;
        }
    }

    private void FindNextPosition()
    {
        m_nextPosition = GetNextPosition();

        //if we've changed position
        if (RoundVector(transform.position) != new Vector2(m_currentPosition.x, m_currentPosition.y))
        {
            Pathfinding.Instance.UpdatePathfindingMap(RoundVector(m_currentPosition, true), Pathfinding.TileState.Open, this);

            m_currentPosition = RoundVector(transform.position);

            Pathfinding.Instance.UpdatePathfindingMap(RoundVector(m_currentPosition, true), Pathfinding.TileState.Closed, this);
        }
        else
        {
            m_currentPosition = RoundVector(transform.position);
        }
    }

    private bool CheckForUpdatePath() //we want to re-find path every time we move to a new tile, so return whether or we have moved to a new tile
    {
        return new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y))
            == new Vector2(m_nextPosition.x, m_nextPosition.y);
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