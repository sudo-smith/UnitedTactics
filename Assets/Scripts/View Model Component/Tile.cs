using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public const float stepHeight = 0.5f;
    public Point pos;
    public int height;
    private List<Tile> neighborTiles;
    public List<Tile> NeighborTiles {
        get {
            if (neighborTiles == null)
            {
                neighborTiles = new List<Tile>();
            }
            return neighborTiles;
        }
    }
    public Vector3 center
    {
        get
        {
            return new Vector3(pos.x, height * stepHeight, pos.y);
        }
    }

    //data that tile stores
    public GameObject content;
    public bool nextToEnemy;

    //path finding data
    [HideInInspector] public Tile prev;
    [HideInInspector] public int distance;

    void Match()
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, height * stepHeight, 1);
    }

    public void Grow()
    {
        height++;
        Match();
    }
    public void Shrink()
    {
        height--;
        Match();
    }

    public void Load(Point p, int h)
    {
        pos = p;
        height = h;
        nextToEnemy = false;
        Match();
    }

    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }
}
