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
public Sprite sprite =>Resources.Load<Sprite>("actionIcon/"+image);
public string startCheck;
}

public class EventInfo
{
    public bool isNegative;
    public string eventId;
    public string eventName;
    public string eventShortDescription;
    public Sprite sprite =>Resources.Load<Sprite>("event/"+eventId);
    public string eventDescription;
    public string eventDetailDescription;
    public Dictionary<string,float> costWhenTimeFinish;
    public Dictionary<string,float> durationEffect;
    public int duration;
    public bool repeat;
    public Dictionary<string,float> happenRequirement;
    public float happenChance;
    public Dictionary<string, float> costToFinishEvent1;
    public string nameToFinishEvent1;
    public Dictionary<string, float> costToFinishEvent2;
    public string nameToFinishEvent2;
    public Dictionary<string, float> costToFinishEvent3;
    public string nameToFinishEvent3;
    public List<Dictionary<string, float>> costToFinishEvents;
    public List<string> nameToFinishEvents;

    //eventId	duration	repeat	costWhenTimeFinish

}


public class CSVManager : Singleton<CSVManager>
{    
    public Dictionary<string,TileInfo > tileInfoDict = new Dictionary<string,TileInfo>();
    public Dictionary<string,TileActionInfo > tileActionInfoDict = new Dictionary<string,TileActionInfo>();
    public Dictionary<string,List<TileInfo>> tileInfoListByType = new Dictionary<string,List<TileInfo>>();
    public Dictionary<string,EventInfo > eventInfoDict = new Dictionary<string,EventInfo>();

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
        
        var eventInfos =  CsvUtil.LoadObjects<EventInfo>("event");
        foreach (var eventInfo in eventInfos)
        {
            eventInfoDict[eventInfo.eventId] = eventInfo;
            eventInfo.costToFinishEvents = new List<Dictionary<string, float>>();
            eventInfo.nameToFinishEvents = new List<string>();
            if(eventInfo.costToFinishEvent1!=null && eventInfo.nameToFinishEvent1.Length>0){
                eventInfo.costToFinishEvents.Add(eventInfo.costToFinishEvent1);
                eventInfo.nameToFinishEvents.Add(eventInfo.nameToFinishEvent1);
            }
            if(eventInfo.costToFinishEvent2!=null && eventInfo.nameToFinishEvent2.Length>0){
                eventInfo.costToFinishEvents.Add(eventInfo.costToFinishEvent2);
                eventInfo.nameToFinishEvents.Add(eventInfo.nameToFinishEvent2);
                
            }
            if(eventInfo.costToFinishEvent3!=null && eventInfo.nameToFinishEvent3.Length>0){
                eventInfo.costToFinishEvents.Add(eventInfo.costToFinishEvent3);
                eventInfo.nameToFinishEvents.Add(eventInfo.nameToFinishEvent3);
            }
        }
        
    }
}
