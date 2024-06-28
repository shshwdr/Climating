using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

public class EventInfoData
{
    public EventInfo eventInfo;
    public float timer;
}

public class EventCardManager : Singleton<EventCardManager>
{
    public List<EventInfoData> currentEventList = new List<EventInfoData>();

    private float checkEventTimer = 0;

    private float checkEventTime = 1;

    // Start is called before the first frame update
    public void Init()
    {
        var taxData = new EventInfoData() { eventInfo = CSVManager.Instance.eventInfoDict["tax"], timer = 0 };
        currentEventList.Add(taxData);
    }

    public void DealEvent(EventInfoData eventData)
    {
        currentEventList.Remove(eventData);
        EventPool.Trigger("updateEvent");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = currentEventList.Count - 1; i >= 0; i--)
        {
            var eventt = currentEventList[i];
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
                    
                    EventPool.Trigger("updateEvent");
                }
            }
        }

        checkEventTimer += Time.deltaTime;
        if (checkEventTimer >= checkEventTime)
        {
            checkEventTimer = 0;
            List<EventInfo> newEventCandidateList = new List<EventInfo>();
            List<float> probability = new List<float>();
            foreach (var info in CSVManager.Instance.eventInfoDict.Values)
            {
                bool isValid = true;
                if (info.eventId == "tax")
                {
                    continue;
                }
                // if (info.happenRequirement == null || info.happenRequirement.Count == 0)
                // {
                //     continue;
                // }

                bool hasEvent = false;
                foreach (var currentEvent in currentEventList)
                {
                    if (currentEvent.eventInfo.eventId == info.eventId)
                    {
                        hasEvent = true;
                        break;
                    }
                }

                if (hasEvent)
                {
                    continue;
                }

                foreach (var pair in info.happenRequirement)
                {
                    var keys = pair.Key.Split(('_'));
                    switch (keys[0])
                    {
                        case "resourceCount":
                            // if (ResourceManager.Instance.GetResourceValue("polution") <= pair.Value)
                            // {
                            //     isValid = false;
                            // }
                            break;
                    }
                }

                if (isValid)
                {
                    newEventCandidateList.Add(info);
                    probability.Add(info.happenChance);
                }
            }

            if (newEventCandidateList.Count == 0)
            {
                return;
            }

            var selectedId = RandomUtil.RandomBasedOnProbabilityMaxWith100(probability);
            if (selectedId != -1)
            {
                var taxData = new EventInfoData() { eventInfo = newEventCandidateList[selectedId], timer = 0 };
                currentEventList.Add(taxData);
                EventPool.Trigger("updateEvent");
            }
        }
    }
    
    

    public Dictionary<string, float> DurationEffect()
    {
        var res = new Dictionary<string, float>();
        foreach (var currentEvent in currentEventList)
        {
            if (currentEvent.eventInfo.durationEffect != null)
            {
                foreach (var pair in currentEvent.eventInfo.durationEffect)
                {
                    if (res.ContainsKey(pair.Key))
                    {
                        res[pair.Key] += pair.Value;
                    }
                    else
                    {
                        res[pair.Key] = pair.Value;
                    }
                }
            }
        }

        return res;
    }
}