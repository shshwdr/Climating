using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sinbad;
using Unity.Mathematics;
using UnityEngine;

public class TileInfo
{
    public string tileId;
    public string tileName;
    public string tileDescription;
    public string type;
    public bool unlock;
    
    public List<TileActionInfo> tileActionInfoList; 
}

public class TileActionInfo
{
public    string actionId;
public string actionName;
public string actionDescription;
public Dictionary<string,float> actionDurationEffect;
public Dictionary<string,float> actionEffect;
public bool isFood;
public bool isAccessory;
public List<string> tile;
public List<string> tileType;
public Dictionary<string,float> actionCost; 
public int actionTime;
public string adjacentAffectType;
public string image;
}



public class CSVManager : Singleton<CSVManager>
{    
    public Dictionary<string,TileInfo > tileInfoDict = new Dictionary<string,TileInfo>();
    public Dictionary<string,TileActionInfo > tileActionInfoDict = new Dictionary<string,TileActionInfo>();
    public Dictionary<string,List<TileInfo>> tileInfoListByType = new Dictionary<string,List<TileInfo>>();

    // Start is called before the first frame update
    public void Init()
    {
        
        var tileActionInfos =  CsvUtil.LoadObjects<TileActionInfo>("tileAction");
        foreach (var tileActionInfo in tileActionInfos)
        {
            tileActionInfoDict[tileActionInfo.actionId] = tileActionInfo;
        }
        
        var tileInfos =  CsvUtil.LoadObjects<TileInfo>("tile");
        foreach (var tileInfo in tileInfos)
        {
            if(tileInfo.unlock){
                tileInfoDict[tileInfo.tileId] = tileInfo;
                if(!tileInfoListByType.ContainsKey(tileInfo.type)){
                    tileInfoListByType[tileInfo.type] = new List<TileInfo>();
                }
                tileInfoListByType[tileInfo.type].Add(tileInfo);
                tileInfo.tileActionInfoList = tileActionInfoDict.Values.Where(x=>x.tileType.Contains(tileInfo.type) || x.tile.Contains(tileInfo.tileId)).ToList();
            }
        }
        
    }
}
