using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInfoData
{
    public EventInfo eventInfo;
    public float timer;
}

public class EventCardManager : Singleton<EventCardManager>
{
    public List<EventInfoData> currentEventList = new List<EventInfoData>();
    // Start is called before the first frame update
    public void Init()
    {
        var taxData = new EventInfoData(){eventInfo = CSVManager.Instance.eventInfoDict["tax"],timer = 0};
        currentEventList.Add(taxData);
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = currentEventList.Count - 1; i >= 0; i--)
        {
            var eventt =  currentEventList[i];
            eventt.timer += Time.deltaTime;
            if (eventt.timer >= eventt.eventInfo.duration)
            {
                eventt.timer = 0;
                ResourceManager.Instance.ConsumeResourceValue(eventt.eventInfo.costWhenTimeFinish);
                if (eventt.eventInfo.repeat)
                {
                    
                }
                else
                {
                    currentEventList.RemoveAt(i);
                }
            }
        }
    }
}
