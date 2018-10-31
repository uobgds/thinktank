using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public static Pathfinding Instance;

    public enum TileState
    {
        Open, Closed
    }

    [SerializeField]
    private Vector2 m_mapSize;
    [SerializeField]
    private float m_tileSize;
    private Vector2 m_start;
    private Vector2 m_end;
    private Vector2 m_position;

    private List<Tile> m_adjacentTiles;
    private Tile m_adjacentTile;

    private List<Tile> m_openTiles;
    private Tile m_startTile;
    private Tile m_endTile;
    private Tile m_currentTile;
    private float m_cheapestTileCost;

    private int m_iterationCount;

    private bool m_unableToReachTile;

    private List<Vector2> m_calculatedPath;

    private TileState[,] m_pathfindingMap;
    private int[,] m_visitedTiles;

    private class Tile
    {
        public Tile m_previousTile;
        public int m_pathCost;
        public float m_directDistance;
        public Vector2 m_position;
        public TileState tileState;

        public List<Tile> FindTilePath(List<Tile> givenPath)
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
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start ()
    {
        m_pathfindingMap = new TileState[(int)m_mapSize.x, (int)m_mapSize.y];

        foreach (Blocker blocker in FindObjectsOfType<Blocker>())
        {
            UpdatePathfindingMap(blocker.transform.position, TileState.Closed);
        }
    }

    //NOTE: pass in real-world positions into here- not array positions, we will convert from real-world to array in the function
    private void UpdatePathfindingMap (Vector2 givenPosition, TileState givenTileState)
    {
        Vector2 pos = new Vector2(Mathf.RoundToInt(givenPosition.x), Mathf.RoundToInt(givenPosition.y)) / m_tileSize;
        if (IsWithinMapBounds((int)pos.x, (int)pos.y))
        {
            m_pathfindingMap[(int)pos.x, (int)pos.y] = givenTileState;
        }
    }

    private bool IsWithinMapBounds (int givenX, int givenY)
    {
        return givenX >= 0 && givenX < m_pathfindingMap.GetLength(0) &&
            givenY >= 0 && givenY < m_pathfindingMap.GetLength(1);
    }

    public List<Vector2> FindPath(Vector2 givenStartPosition, Vector2 givenEndPosition)
    {
        m_calculatedPath = new List<Vector2>();
        StartCoroutine(FindPathCoroutine(givenStartPosition, givenEndPosition, m_pathfindingMap));
        return m_calculatedPath;
    }

    //NOTE: pass in real-world positions into here- not array positions, we will convert from real-world to array in the function
    private IEnumerator FindPathCoroutine(Vector2 givenStartPosition, Vector2 givenEndPosition, TileState[,] givenPathfindingMap)
    {
        m_start = new Vector2(Mathf.RoundToInt(givenStartPosition.x), Mathf.RoundToInt(givenStartPosition.y)) / m_tileSize;
        m_end = new Vector2(Mathf.RoundToInt(givenEndPosition.x), Mathf.RoundToInt(givenEndPosition.y)) / m_tileSize;

        m_visitedTiles = new int[givenPathfindingMap.GetLength(0), givenPathfindingMap.GetLength(1)];

        m_openTiles = new List<Tile>();

        m_startTile = new Tile();
        m_endTile = new Tile();

        m_startTile.m_position = givenStartPosition;
        m_endTile.m_position = givenEndPosition;

        m_currentTile = m_startTile;

        m_visitedTiles[(int)m_currentTile.m_position.x, (int)m_currentTile.m_position.y] = 1;

        m_iterationCount = 0;

        while (m_currentTile.m_position != m_endTile.m_position)
        {
            m_iterationCount++;
            m_cheapestTileCost = float.MaxValue;

            foreach (Tile tile in FindAdjacentTiles(m_currentTile, m_endTile, givenPathfindingMap))
            {
                bool tileAlreadyExists = false;

                for (int i = 0; i < m_openTiles.Count; i++)
                {
                    //if the tile already exists
                    if (m_openTiles[i].m_position == tile.m_position)
                    {
                        m_openTiles[i].m_pathCost = tile.m_pathCost;
                        tileAlreadyExists = true;
                    }
                }

                if (!tileAlreadyExists)
                {
                    m_openTiles.Insert(0, tile); //put at front of list, as recently looked at adjacent tiles are likely to be most efficient to be looked at next
                }
            }

            foreach (Tile tile in m_openTiles)
            {
                if ((tile.m_pathCost + (int)tile.m_directDistance) < m_cheapestTileCost)
                {
                    m_cheapestTileCost = tile.m_pathCost + (int)tile.m_directDistance;
                    m_currentTile = tile;
                }
            }
            m_visitedTiles[(int)m_currentTile.m_position.x, (int)m_currentTile.m_position.y] = 1;
            m_openTiles.Remove(m_currentTile);

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

            foreach (Tile tileDetails in m_currentTile.FindTilePath(new List<Tile>()))
            {
                m_calculatedPath.Add(tileDetails.m_position * m_tileSize);
            }
        }
    }

    private List<Tile> FindAdjacentTiles(Tile givenCurrentTile, Tile givenTargetTile, TileState[,] givenPathfindingMap)
    {
        m_adjacentTiles = new List<Tile>();

        for (int x = -1; x < 2; x++) //check a 3x3 grid around the givenCurrentTile
        {
            for (int y = -1; y < 2; y++)
            {
                m_adjacentTile = new Tile();

                m_adjacentTile.m_position.x = (int)(x + givenCurrentTile.m_position.x);
                m_adjacentTile.m_position.y = (int)(y + givenCurrentTile.m_position.y);

                if (IsWithinMapBounds((int)m_adjacentTile.m_position.x, (int)m_adjacentTile.m_position.y) 
                    && m_visitedTiles[(int)m_adjacentTile.m_position.x, (int)m_adjacentTile.m_position.y] == 0) //if tile is not already visited
                {
                    m_adjacentTile.tileState = givenPathfindingMap[(int)m_adjacentTile.m_position.x, (int)m_adjacentTile.m_position.y];
                    //tile is within the map bounds and is not an obstacle
                    if (m_adjacentTile.tileState != TileState.Closed
                        && m_adjacentTile.m_position != givenCurrentTile.m_position)
                    {
                        m_adjacentTile.m_directDistance = Vector2.Distance(m_adjacentTile.m_position, givenTargetTile.m_position) * 50f;

                        m_adjacentTile.m_previousTile = givenCurrentTile;

                        m_adjacentTile.m_pathCost += givenCurrentTile.m_pathCost;
                        m_adjacentTiles.Add(m_adjacentTile);
                    }
                }

            }
        }
        return m_adjacentTiles;
    }

}