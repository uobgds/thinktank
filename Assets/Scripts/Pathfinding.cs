using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public static Pathfinding Instance;

    public delegate void OnPathfindingMapUpdated(Vector2 position, Enemy sender);
    public static OnPathfindingMapUpdated onPathfindingMapUpdated;

    public enum TileState
    {
        Open, Closed
    }

    [SerializeField]
    private Vector2 m_mapSize;
    public float m_tileSize;
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

    public bool m_initialised;

    private bool m_unableToReachTile;

    private PathfindingInformation m_pathfindingInformation;

    public TileState[,] m_pathfindingMap;
    private int[,] m_visitedTiles;

    private TileState m_previousTileState;

    private class Tile
    {
        public Tile m_previousTile;
        public int m_pathCost = 1;
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

    public class PathfindingInformation
    {
        public List<Vector2> m_path = new List<Vector2>();
        public bool m_canReachTile = true;
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

        int priority = 0;

        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            enemy.m_priority = priority;
            priority++;
        }

        foreach (Blocker blocker in FindObjectsOfType<Blocker>())
        {
            Vector2 pos = new Vector2(Mathf.RoundToInt(blocker.transform.position.x), Mathf.RoundToInt(blocker.transform.position.y)) / m_tileSize;
            UpdatePathfindingMap(pos, TileState.Closed, null);
        }

        m_pathfindingInformation = new PathfindingInformation();

        m_initialised = true;
    }

    //NOTE: pass in array positions
    public void UpdatePathfindingMap (Vector2 givenTilePosition, TileState givenTileState, Enemy sender)
    {
        if (IsWithinMapBounds((int)givenTilePosition.x, (int)givenTilePosition.y))
        {
            m_previousTileState = m_pathfindingMap[(int)givenTilePosition.x, (int)givenTilePosition.y];
            m_pathfindingMap[(int)givenTilePosition.x, (int)givenTilePosition.y] = givenTileState;

            //if a tile has changed state (ie an open tile is now closed, or vice versa, inform enemies so they can recalulate paths)
            if (m_previousTileState != givenTileState && onPathfindingMapUpdated != null)
            {
                onPathfindingMapUpdated(givenTilePosition, sender);
            }
        }
    }

    private bool IsWithinMapBounds (int givenX, int givenY)
    {
        return givenX >= 0 && givenX < m_pathfindingMap.GetLength(0) &&
            givenY >= 0 && givenY < m_pathfindingMap.GetLength(1);
    }

    public PathfindingInformation FindPath(Vector2 givenStartPosition, Vector2 givenEndPosition)
    {
        StartCoroutine(FindPathCoroutine(givenStartPosition, givenEndPosition, m_pathfindingMap));
        return m_pathfindingInformation;
    }

    //NOTE: pass in real-world positions into here- not array positions, we will convert from real-world to array in the function
    private IEnumerator FindPathCoroutine(Vector2 givenStartPosition, Vector2 givenEndPosition, TileState[,] givenPathfindingMap)
    {
        m_start = new Vector2(Mathf.RoundToInt(givenStartPosition.x / m_tileSize), Mathf.RoundToInt(givenStartPosition.y / m_tileSize));
        m_end = new Vector2(Mathf.RoundToInt(givenEndPosition.x / m_tileSize), Mathf.RoundToInt(givenEndPosition.y / m_tileSize));

        m_visitedTiles = new int[givenPathfindingMap.GetLength(0), givenPathfindingMap.GetLength(1)];

        m_openTiles = new List<Tile>();

        m_startTile = new Tile();
        m_endTile = new Tile();

        m_startTile.m_position = m_start;
        m_endTile.m_position = m_end;

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

            if (m_iterationCount > 100) //this prevents too much work being carried out in one frame, so the game won't freeze
            {
                m_iterationCount = 0;
                yield return null;
            }

            if (m_openTiles.Count == 0) //explored all tiles, means we've tried to get to a blocked area
            {
                m_pathfindingInformation = new PathfindingInformation();
                m_pathfindingInformation.m_canReachTile = false;
                StopCoroutine("FindPathCoroutine");
            }
        }
    }

    private void CompileCoordinates ()
    {
        m_pathfindingInformation = new PathfindingInformation();

        foreach (Tile tile in m_currentTile.FindTilePath(new List<Tile>()))
        {
            if (tile.m_position != m_startTile.m_position)
            {
                m_pathfindingInformation.m_path.Add(tile.m_position * m_tileSize);
            }
        }

        //m_pathfindingInformation.m_path.Insert(0, m_endTile.m_position * m_tileSize);
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
                        m_adjacentTile.m_directDistance = Vector2.Distance(m_adjacentTile.m_position, givenTargetTile.m_position) / m_tileSize;

                        m_adjacentTile.m_previousTile = givenCurrentTile;

                        m_adjacentTile.m_pathCost += givenCurrentTile.m_pathCost;
                        m_adjacentTiles.Add(m_adjacentTile);
                    }

                    if (m_adjacentTile.m_position == m_endTile.m_position)
                    {
                        CompileCoordinates();
                        StopCoroutine("FindPathCoroutine");
                    }
                }

            }
        }
        return m_adjacentTiles;
    }

}