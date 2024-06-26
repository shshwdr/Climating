using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HexTile
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Z { get; private set; }
    public bool isExplored = false;
    public bool isLand = false;
    public bool isExploring = false;
    public bool isActioning = false;
    public bool isRemovingAction = false;
    public TileActionInfo action = null;
    public bool actionWorks => action != null&& !isActioning && !isRemovingAction;
    public TileInfo info;
    public int exploreCost;
    public int exploreTime;
    public void startAction(TileActionInfo actionInfo)
    {
        action = actionInfo;
        isActioning = true;
        exploreTime = actionInfo.actionTime;
        HexGridManager.Instance.hexTileToControllerDict[this].UpdateView();
    }
    public void startStopAction(TileActionInfo actionInfo)
    {
        isRemovingAction = true;
        exploreTime = 2;
        
        HexGridManager.Instance.hexTileToControllerDict[this].UpdateView();
        ResourceManager.Instance.UpdateIncreaseResourceValues();
    }

    public int effectMultiplier()
    {
        return effectMultiplier(action);
    }

    public int effectMultiplier(TileActionInfo action)
    {
        int start = 1;
        switch (action.adjacentAffectType)
        {
            case "sameTileBenefit":
                foreach (var neighbor in HexGridManager.Instance.GetNeighbors(this))
                {
                    if (neighbor.isExplored && neighbor.info.tileId == info.tileId)
                    {
                        start++;
                    }
                }

                return start;
            case "differentTileBenefit":
                
                foreach (var neighbor in HexGridManager.Instance.GetNeighbors(this))
                {
                    if (neighbor.isExplored && neighbor.info.tileId != info.tileId)
                    {
                        start++;
                    }
                }

                return start;
            case "food":
                foreach (var neighbor in HexGridManager.Instance.hexTileDict.Values)
                {
                    if (neighbor.isExplored && neighbor.action!=null && !neighbor.isActioning  && neighbor.action.isFood)
                    {
                        start++;
                    }
                }

                return start;
            case "accessory":
                foreach (var neighbor in HexGridManager.Instance.hexTileDict.Values)
                {
                    if (neighbor.isExplored && neighbor.action!=null && !neighbor.isActioning  && neighbor.action.isAccessory)
                    {
                        start++;
                    }
                }

                return start;
            
                
        }
        return 1;
        
    }
    
    
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
                tile.exploreCost = math.max(1, (math.abs(x)+math.abs(z))/2);
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
                var info = CSVManager.Instance.tileInfoListByType["land"].RandomItem();
                var shallowOceanTile = info.tileId;
                tile.isExplored = true;
                tile.info = info;
                GameObject hexPrefab = Resources.Load<GameObject>("hexTile/" + shallowOceanTile);
                var hex = Instantiate(hexPrefab,
                    HexGrid.CubeToFlatTopWorldPosition(tile, hexSize, widthFactor, heightFactor,yOffsetFactor ),
                    Quaternion.identity);
                hex.GetComponent<HexTileController>().Init(tile, true);
                hexTileToControllerDict[tile] = hex.GetComponent<HexTileController>();
            }
            else
            {
                var info = CSVManager.Instance.tileInfoListByType["shallowWater"].RandomItem();
                var shallowOceanTile = info.tileId;
                tile.info = info;
                GameObject hexPrefab = Resources.Load<GameObject>("hexTile/" + shallowOceanTile);
                var hex = Instantiate(hexPrefab,
                    HexGrid.CubeToFlatTopWorldPosition(tile, hexSize, widthFactor, heightFactor,yOffsetFactor ),
                    Quaternion.identity);
                hex.GetComponent<HexTileController>().Init(tile);
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