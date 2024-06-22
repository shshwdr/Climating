using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HexTile
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Z { get; private set; }
    public bool isExplored = false;
    public bool isLand = false;

    public HexTile(int x, int y, int z)
    {
        if (x + y + z != 0)
            throw new ArgumentException("Coordinates must satisfy x + y + z = 0");
        X = x;
        Y = y;
        Z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is HexTile tile &&
               X == tile.X &&
               Y == tile.Y &&
               Z == tile.Z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public static bool operator ==(HexTile left, HexTile right)
    {
        return EqualityComparer<HexTile>.Default.Equals(left, right);
    }

    public static bool operator !=(HexTile left, HexTile right)
    {
        return !(left == right);
    }
}

public class HexGrid
{
    public static readonly HexTile[] Directions =
    {
        new HexTile(1, -1, 0), new HexTile(1, 0, -1), new HexTile(0, 1, -1),
        new HexTile(-1, 1, 0), new HexTile(-1, 0, 1), new HexTile(0, -1, 1)
    };


    public int GetDistance(HexTile a, HexTile b)
    {
        return (Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y) + Mathf.Abs(a.Z - b.Z)) / 2;
    }

    // 平顶（Flat-Topped）布局的世界坐标转换
    public static Vector2 CubeToFlatTopWorldPosition(HexTile tile, float hexSize, float widthFactor,
        float heightFactor,float yOffsetFactor)
    {
        float x = widthFactor *hexSize * (3.0f / 2.0f * tile.X)+ yOffsetFactor * tile.Y;
        float y = heightFactor *hexSize * (Mathf.Sqrt(3.0f) * (tile.Z + tile.X / 2.0f));
        return new Vector2(x, y);
    }

    // 尖顶（Pointy-Topped）布局的世界坐标转换
    public static Vector2 CubeToPointyTopWorldPosition(HexTile tile, float hexSize, float widthFactor,
        float heightFactor)
    {
        float x = widthFactor * hexSize * (Mathf.Sqrt(3.0f) * (tile.X + tile.Z / 2.0f));
        float y = heightFactor * hexSize * (3.0f / 2.0f * tile.Z);
        return new Vector2(x, y);
        // float width = widthFactor * hexSize;
        // float height = heightFactor * hexSize;
        // float xPos = tile.X * width * 0.75f;
        // float yPos = tile.Z * height + (tile.X % 2 == 0 ? 0 : height / 2);
        // return new Vector2(xPos, yPos);
        // var x = tile.X;
        // var y = tile.Z;
        // float width = widthFactor * hexSize * 2;
        // float height = heightFactor * hexSize;
        // float xPos = x * width;
        // float yPos = y * height + (x % 2 == 0 ? 0 : height / 2);
        // return new Vector2(xPos, yPos);
    }
}

public class HexGridManager : Singleton<HexGridManager>
{
    public float widthFactor = 1f;
    public float heightFactor = 1f;
    public float yOffsetFactor  = 1f;
    public float hexSize = 1;
    List<string> shallowOceanTiles = new List<string>() { "CoralReef",/* "Estuary", "RedMangroves",*/ };
    List<string> landTiles = new List<string>() { "SandBeach" };

    public Dictionary<HexTile, HexTileController> hexTileToControllerDict = new Dictionary<HexTile, HexTileController>();
public Dictionary<int,HexTile> hexTileDict = new Dictionary<int,HexTile>();
    // Start is called before the first frame update
    public int mapSize = 1;

    public  List<HexTile> GetNeighbors(HexTile tile)
    {
        List<HexTile> neighbors = new List<HexTile>();
        foreach (var direction in HexGrid. Directions)
        {
            var hashCode = new HexTile(
                tile.X + direction.X,
                tile.Y + direction.Y,
                tile.Z + direction.Z
            ).GetHashCode();
            if (hexTileDict.ContainsKey(hashCode))
            {
                neighbors.Add( hexTileDict[hashCode]);
            }
        }

        return neighbors;
    }
    void Start()
    {
        for (int x = -mapSize; x <= mapSize; x++)
        {
            for (int z = -mapSize; z <= mapSize; z++)
            {
                var y = -x - z;
                var tile = new HexTile(x, y, z);

                hexTileToControllerDict[tile] = null;
                hexTileDict[tile.GetHashCode()] = tile;
                if (x == 0 && z == 0)
                {
                    tile.isExplored = true;
                    tile.isLand = true;
                }
            }
        }

        foreach (var tile in hexTileToControllerDict.Keys.ToList())
        {
            if (tile.isLand)
            {
                var shallowOceanTile = landTiles.RandomItem();
                tile.isExplored = true;
                GameObject hexPrefab = Resources.Load<GameObject>("hexTile/" + shallowOceanTile);
                var hex = Instantiate(hexPrefab,
                    HexGrid.CubeToFlatTopWorldPosition(tile, hexSize, widthFactor, heightFactor,yOffsetFactor ),
                    Quaternion.identity);
                hex.GetComponent<HexTileController>().Init(tile,
                    Instantiate((Resources.Load<Sprite>("hexIcon/" + shallowOceanTile))), true);
                hexTileToControllerDict[tile] = hex.GetComponent<HexTileController>();
            }
            else
            {
                var shallowOceanTile = shallowOceanTiles.RandomItem();
                GameObject hexPrefab = Resources.Load<GameObject>("hexTile/" + shallowOceanTile);
                var hex = Instantiate(hexPrefab,
                    HexGrid.CubeToFlatTopWorldPosition(tile, hexSize, widthFactor, heightFactor,yOffsetFactor ),
                    Quaternion.identity);
                hex.GetComponent<HexTileController>().Init(tile,
                    Instantiate((Resources.Load<Sprite>("hexIcon/" + shallowOceanTile))));
                hexTileToControllerDict[tile] = hex.GetComponent<HexTileController>();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}