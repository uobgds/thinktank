using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding {

    private static float m_tileSize;
    private static Vector2 m_start;
    private static Vector2 m_end;

        public static Vector2 m_position;

        private class Tile
        {
            public Tile m_previousTile;

            public static List<Tile> FindTilePath(List<Tile> givenPath)
            {
                if (m_previousTile == null)
                {
                    return givenPath;
                }
                else
                {
                    givenPath.Add(this);
                    m_previousTile.FindTilePath(givenPath);
                    return givenPath;
                }
            }

            public int m_pathCost;
            public int m_walkSpeedCost;

            public float m_directDistance;

            public Vector2 m_position;
        }

    }

    public static List<Vector2> FindPath (Vector2 start, Vector2 end)
    {
        m_start = new Vector2(Mathf.RoundToInt(start.x), Mathf.RoundToInt(start.y)) / m_tileSize;
        m_end = new Vector2(Mathf.RoundToInt(end.x), Mathf.RoundToInt(end.y)) / m_tileSize;

        return null;
    } 

    private static List<Vector2> AdjacentTiles
    {

    }

}

\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

 public List<Vector2> FindPath(Vector2 givenStartPosition, Vector2 givenEndPosition)
{
    StartCoroutine(FindPathCoroutine(givenStartPosition, givenEndPosition, PathfindingMap));
    return m_calculatedPath;
}

//NOTE: pass in real-world positions into here- not array positions, we will convert from real-world to array in the function
private IEnumerator FindPathCoroutine(Vector2 givenStartPosition, Vector2 givenEndPosition, int[,] givenTileArray)
{
    m_unableToReachTile = false;

    givenStartPosition /= GameController.Instance.TileSpacing;
    givenEndPosition /= GameController.Instance.TileSpacing;

    m_iterationCount = 0;
    m_visitedTiles = new int[PathfindingMap.GetLength(0), PathfindingMap.GetLength(1)];

    foreach (Transform trans in m_pathMarkerParent.GetComponentsInChildren<Transform>())
    {
        if (trans != m_pathMarkerParent.transform)
        {
            Destroy(trans.gameObject);
        }
    }
    m_openTiles = new List<TileDetails>();

    m_startTile = new TileDetails();
    m_endTile = new TileDetails();

    m_startTile.m_position = givenStartPosition;
    m_endTile.m_position = givenEndPosition;

    m_currentTile = m_startTile;

    m_visitedTiles[(int)m_currentTile.m_position.x, (int)m_currentTile.m_position.y] = 1;

    while (m_currentTile.m_position != m_endTile.m_position && m_iterationCount < m_allowedIterations)
    {
        m_iterationCount++;
        m_cheapestTileCost = float.MaxValue;

        foreach (TileDetails tileDetails in FindAdjacentTiles(m_currentTile, m_endTile, givenTileArray))
        {
            bool tileAlreadyExists = false;

            for (int i = 0; i < m_openTiles.Count; i++)
            {
                //if the tile already exists
                if (m_openTiles[i].m_position == tileDetails.m_position)
                {
                    m_openTiles[i].m_pathCost = tileDetails.m_pathCost;
                    tileAlreadyExists = true;
                    //basically, we return all these adjacent tiles yeh, but what was happening before was we would see that some of the tiles were already
                    //in the m_openTiles list, so we wouldn't add them again, BUT we forgot that this means their path cost never is updated, so they always 
                    //seemed really cheap to the algorithm
                }
            }

            if (!tileAlreadyExists)
            {
                m_openTiles.Insert(0, tileDetails); //put at front of list, as recently looked at adjacent tiles are likely to be most efficient to be looked at next
            }
        }

        foreach (TileDetails tileDetails in m_openTiles)
        {
            if ((tileDetails.m_pathCost + (int)tileDetails.m_directDistance) < m_cheapestTileCost)
            {
                m_cheapestTileCost = tileDetails.m_pathCost + (int)tileDetails.m_directDistance;
                m_currentTile = tileDetails;
            }
        }
        m_visitedTiles[(int)m_currentTile.m_position.x, (int)m_currentTile.m_position.y] = 1;
        m_openTiles.Remove(m_currentTile);

        #region Too scared to remove
        //for (int i = 0; i < m_openTiles.Count; i++)
        //{
        //    if (m_openTiles[i].m_position == m_currentTile.m_position)
        //    {
        //        m_visitedTiles[(int)m_openTiles[i].m_position.x, (int)m_openTiles[i].m_position.y] = 1;
        //        m_openTiles.RemoveAt(i);
        //        break;
        //    }
        //}
        #endregion

        if (m_showAllMoves && m_showPath)
        {
            SpawnMarker(m_currentTile.m_position, m_pathMarkerParent.transform, (m_currentTile.m_pathCost + "," + m_currentTile.m_directDistance).ToString());
        }

        if (m_iterationCount > 500) //this prevents too much work being carried out in one frame, so the game won't freeze
        {
            m_iterationCount = 0;
            yield return null;
        }

        if (m_openTiles.Count == 0) //explored all tiles, means we've tried to get to a blocked area
        {
            print("can't reach tile");
            m_unableToReachTile = true;
            break;
        }
    }

    if (m_unableToReachTile)
    {
        m_calculatedPath = new List<Vector2>();
    }

    else
    {
        m_calculatedPath = new List<Vector2>();

        foreach (TileDetails tileDetails in m_currentTile.FindTilePath(new List<TileDetails>()))
        {
            m_calculatedPath.Add(tileDetails.m_position * GameController.Instance.TileSpacing);

            if (!m_showAllMoves && m_showPath)
            {
                SpawnMarker(tileDetails.m_position, m_pathMarkerParent.transform, (m_currentTile.m_pathCost + "," + m_currentTile.m_directDistance).ToString());
            }
        }
    }
}

private List<TileDetails> FindAdjacentTiles(TileDetails givenCurrentTile, TileDetails givenTargetTile, int[,] givenTileArray)
{
    m_adjacentTiles = new List<TileDetails>();

    for (int x = -1; x < 2; x++) //check a 3x3 grid around the givenCurrentTile
    {
        for (int y = -1; y < 2; y++)
        {
            m_adjacentTile = new TileDetails();

            m_adjacentTile.m_position.x = (int)(x + givenCurrentTile.m_position.x);
            m_adjacentTile.m_position.y = (int)(y + givenCurrentTile.m_position.y);

            if (IsInMapBounds(m_adjacentTile.m_position) && m_visitedTiles[(int)m_adjacentTile.m_position.x, (int)m_adjacentTile.m_position.y] == 0) //if tile is not already visited
            {
                m_adjacentTile.m_walkSpeedCost = givenTileArray[(int)m_adjacentTile.m_position.x, (int)m_adjacentTile.m_position.y];
                //tile is within the map bounds and is not an obstacle
                if (m_adjacentTile.m_walkSpeedCost != -1
                    && m_adjacentTile.m_position != givenCurrentTile.m_position)
                {
                    //quick note: I do all this stuff here (tile cost etc) rather than above, as above tiles are not guaranteed to make it on the final list, so might as well only 
                    //burn cpu time on tiles of use

                    m_adjacentTile.m_directDistance = Vector2.Distance(m_adjacentTile.m_position, givenTargetTile.m_position) * 50f;

                    //m_adjacentTile.m_directDistance = BlockDistance(m_adjacentTile.m_position, givenTargetTile.m_position);

                    m_adjacentTile.m_previousTile = givenCurrentTile;

                    m_adjacentTile.m_pathCost += m_adjacentTile.m_walkSpeedCost + givenCurrentTile.m_pathCost;
                    m_adjacentTiles.Add(m_adjacentTile);
                }
            }

        }
    }
    return m_adjacentTiles;
}

private bool IsInMapBounds(Vector2 givenPosition)
{
    return givenPosition.x >= 0
        && givenPosition.x < PathfindingMap.GetLength(0)
        && givenPosition.y >= 0
        && givenPosition.y < PathfindingMap.GetLength(1);
}