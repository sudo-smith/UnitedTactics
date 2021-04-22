using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Board : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
    private List<Unit> _allyUnits; //todo eventually probably more classification than ally/not ally
    private List<Unit> _enemyUnits; //todo eventually probably more classification than ally/not ally
    public List<Unit> AllyUnits
    {
        get {
            if (_allyUnits == null)
                _allyUnits = new List<Unit>();
            return _allyUnits;
        }
    }
    public List<Unit> EnemyUnits
    {
        get
        {
            if (_enemyUnits == null)
                _enemyUnits = new List<Unit>();
            return _enemyUnits;
        }
    }

    readonly Point[] dirs = new Point[4]
    {
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0)
    };

    readonly Color selectedTileColor = new Color(0, 1, 1, 1);
    readonly Color enemyMeleeTileColor = new Color(1, 0, 0, 1);
    readonly Color defaultTileColor = new Color(1, 1, 1, 1);

    public void Load(LevelData data)
    {
        for (int i = 0; i < data.tiles.Count; ++i)
        {
            GameObject instance = Instantiate(tilePrefab) as GameObject;
            Tile t = instance.GetComponent<Tile>();
            t.Load(data.tiles[i]);
            tiles.Add(t.pos, t);
        }
        setNeighborTiles();
    }

    public void setNeighborTiles()
    {
        foreach (Tile t in tiles.Values)
        {
            for (int i = 0; i < 4; i++)
            {
                Tile neighbor = GetTile(t.pos + dirs[i]);
                if(neighbor != null)
                {
                    t.NeighborTiles.Add(neighbor);
                }
            }
        }
    }

    public Tile GetTile(Point p)
    {
        return tiles.ContainsKey(p) ? tiles[p] : null;
    }

    void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }

    public List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();
        start.distance = 0;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue();
            foreach(Tile next in t.NeighborTiles)
            {
                if (next.distance <= t.distance + 1)
                    continue;
                if (addTile(t, next))
                {
                    next.distance = t.distance + 1;
                    next.prev = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }
            }
            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);
        }
        return retValue;
    }

    void ClearSearch()
    {
        foreach (Tile t in tiles.Values)
        {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }

    public void SelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
        {
            Color tileCol = tiles[i].nextToEnemy ? enemyMeleeTileColor : selectedTileColor;
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", tileCol);
        }
    }
    public void DeSelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
    }
}
