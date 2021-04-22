using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class BoardCreator : MonoBehaviour
{

    [SerializeField] GameObject tileViewPrefab;
    [SerializeField] GameObject tileSelectionIndicatorPrefab;

    [SerializeField] GameObject borderVolumePrefab;

    Transform borderVolume
    {
        get
        {
            if (_borderVolume == null)
            {
                GameObject instance = Instantiate(borderVolumePrefab) as GameObject;
                _borderVolume = instance.transform;
            }
            return _borderVolume;
        }
    }
    Transform _borderVolume;

    Transform marker
    {
        get
        {
            if (_marker == null)
            {
                GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
                _marker = instance.transform;
            }
            return _marker;
        }
    }
    Transform _marker;

    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    [Range(-50, 50)]
    [SerializeField] int borderPosX = 0;
    [Range(-50, 50)]
    [SerializeField] int borderPosY = 0;

    [SerializeField] int width = 10;
    [SerializeField] int depth = 10;
    [SerializeField] int height = 8;

    [SerializeField] Point pos;
    [Range(1, 10)]
    [SerializeField] int markerSize = 1;

    [SerializeField] LevelData levelData;




    public void GrowRandArea()
    {
        Rect r = RandomRect();
        GrowRect(r);
    }
    public void ShrinkRandArea()
    {
        Rect r = RandomRect();
        ShrinkRect(r);
    }

    Rect RandomRect()
    {
        int x = UnityEngine.Random.Range(borderPosX, (borderPosX + width));
        int y = UnityEngine.Random.Range(borderPosY, (borderPosY + depth));
        int w = UnityEngine.Random.Range(1, (width + borderPosX) - x + 1);
        int h = UnityEngine.Random.Range(1, (depth + borderPosY) - y + 1);
        return new Rect(x, y, w, h);
    }

    Rect curRect;

    void GrowRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                GrowSingle(p);
            }
        }
    }

    void ShrinkRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                ShrinkSingle(p);
            }
        }
    }

    Tile Create()
    {
        GameObject instance = Instantiate(tileViewPrefab) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
            return tiles[p];

        Tile t = Create();
        t.Load(p, 0);
        tiles.Add(p, t);

        return t;
    }

    void GrowSingle(Point p)
    {
        Tile t = GetOrCreate(p);
        if (t.height < height)
            t.Grow();
    }

    void ShrinkSingle(Point p)
    {
        if (!tiles.ContainsKey(p))
            return;

        Tile t = tiles[p];
        t.Shrink();

        if (t.height <= 0)
        {
            tiles.Remove(p);
            DestroyImmediate(t.gameObject);
        }
    }

    public void Grow()
    {
        GrowRect(curRect);
    }

    public void GrowArea()
    {
        GrowRect(new Rect(borderPosX, borderPosY, width, depth));
    }

    public void ShrinkArea()
    {
        ShrinkRect(new Rect(borderPosX, borderPosY, width, depth));
    }

    public void Shrink()
    {
        ShrinkRect(curRect);
    }

    public void UpdateBorder()
    {
        borderVolume.localPosition = new Vector3(borderPosX - 0.5f, borderVolume.position.y, borderPosY - 0.5f);
        borderVolume.localScale = new Vector3(width, height * Tile.stepHeight, depth);
    }

    public void UpdateMarker()
    {
        Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = t != null ? t.center : new Vector3(pos.x, 0, pos.y);
        marker.localScale = new Vector3((markerSize * 2) - 1, 1, (markerSize * 2) - 1);
        int x = (int)pos.x - (markerSize - 1);
        int y = (int)pos.y - (markerSize - 1);
        int w, h;
        w = h = (2 * markerSize) - 1;
        curRect = new Rect(x, y, w, h);
    }

    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);
        tiles.Clear();
    }

    public void Save()
    {
        string filePath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory();

        LevelData board = ScriptableObject.CreateInstance<LevelData>();
        board.tiles = new List<Vector3>(tiles.Count);
        foreach (Tile t in tiles.Values)
            board.tiles.Add(new Vector3(t.pos.x, t.height, t.pos.y));

        string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, name);
        AssetDatabase.CreateAsset(board, fileName);
    }

    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");
        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        AssetDatabase.Refresh();
    }

    public void Load()
    {
        Clear();
        if (levelData == null)
        {
            Debug.Log("Level Data is null");
            return;
        }

        Debug.Log("Loading Level");
        foreach (Vector3 v in levelData.tiles)
        {
            Tile t = Create();
            t.Load(v);
            tiles.Add(t.pos, t);
        }
    }



}
