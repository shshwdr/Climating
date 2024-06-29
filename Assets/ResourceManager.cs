using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
using UnityEngine;

public class Resource
{
    public string Name { get; private set; }
    public float Value { get; private set; }
    public float IncreasePerSecond { get; private set; }

    public float baseIncreasePerSecond;

    public void clearIncreaseValues()
    {
        IncreasePerSecond =baseIncreasePerSecond;
    }
    public Resource(string name, float initialValue, float increasePerSecond)
    {
        Name = name;
        Value = initialValue;
        baseIncreasePerSecond = increasePerSecond;
    }

    public void ChangeIncreasePerSecond(float value)
    {
        IncreasePerSecond += value;
    }

    public void IncreaseValueByTime(float deltaTime)
    {
        Value += IncreasePerSecond * deltaTime;
        EventPool.Trigger("updateResource");
    }
    public void IncreaseValue(float amount)
    {
        Value += amount;
        EventPool.Trigger("updateResource");
    }

    public void DecreaseValue(float amount)
    {
        Value -= amount;
        EventPool.Trigger("updateResource");

        if (Name == "money" && Value < 0)
        {
            GameManager.Instance.Lose();
        }
    }
}
public class ResourceManager : Singleton<ResourceManager>
{
    private List<Resource> resources = new List<Resource>();
public List<Resource> Resources => resources;
    public void Init()
    {
        // 开始更新资源值的协程
        StartCoroutine(UpdateResourceValues());
        AddResource((new Resource("money", 50, 0)));
        AddResource((new Resource("tech", 0, 0)));
        AddResource((new Resource("human", 40, 1)));
        AddResource((new Resource("polution", 0, 0)));
    }

    public float updateTime = 3;
    private IEnumerator UpdateResourceValues()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateTime);
            foreach (var resource in resources)
            {
                 resource.IncreaseValueByTime(1);

            }
        }
    }

    public void UpdateIncreaseResourceValues()
    {
        foreach (var resource in resources)
        {
            resource.clearIncreaseValues();
        }
        foreach (var tile in HexGridManager.Instance.hexTileDict.Values)
        {
            if (tile.actionWorks)
            {
                var multiplier = tile.effectMultiplier();
                var increase = tile.action.actionDurationEffect;
                ProduceResourceValueIncreaseWithMultiplier(increase, multiplier);

            }
        }
    }

    public void AddResource(Resource resource)
    {
        resources.Add(resource);
    }

    public void RemoveResource(Resource resource)
    {
        resources.Remove(resource);
    }

    public Resource GetResource(string name)
    {
        return resources.Find(r => r.Name == name);
    }

    public void ConsumeResourceValue(string name, float amount)
    {
        Resource resource = GetResource(name);
        if (resource != null)
        {
            resource.DecreaseValue(amount);
        }
    }
    public void ConsumeResourceValue(Dictionary<string ,float> value)
    {
        foreach (var item in value)
        {
            ConsumeResourceValue(item.Key, item.Value);
        }
    }
    public void ProduceResourceValue(Dictionary<string ,float> value)
    {
        foreach (var item in value)
        {
            ProduceResourceValue(item.Key, item.Value);
        }
    }
    public void ProduceResourceValueWithMultiplier(Dictionary<string ,float> value, int multiplier)
    {
        foreach (var item in value)
        {
            ProduceResourceValue(item.Key, item.Value * multiplier);
        }
    }
    
    public void ProduceResourceValueIncrease(Dictionary<string ,float> value)
    {
        foreach (var item in value)
        {
            ProduceResourceValueIncrease(item.Key, item.Value);
        }
    }
    public void ProduceResourceValueIncreaseWithMultiplier(Dictionary<string ,float> value, int multiplier)
    {
        foreach (var item in value)
        {
            ProduceResourceValueIncrease(item.Key, item.Value*multiplier);
        }
    }
    public void ProduceResourceValue(string name, float amount)
    {
        Resource resource = GetResource(name);
        if (resource != null)
        {
            resource.IncreaseValue(amount);
        }
    }
    public void ProduceResourceValueIncrease(string name, float amount)
    {
        Resource resource = GetResource(name);
        if (resource != null)
        {
            resource.ChangeIncreasePerSecond(amount);
        }
    }

    public bool CanConsumeResourceValue(Dictionary<string, float> value)
    {
        foreach (var item in value)
        {
            Resource resource = GetResource(item.Key);
            if (resource != null)
            {
                if (resource.Value < item.Value)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }
    
    public float GetResourceValue(string name)
    {
        Resource resource = GetResource(name);
        if (resource != null)
        {
            return resource.Value;
        }
        else
        {
            return 0;
        }
    }

    public bool CanConsumeResourceValue(string name, float amount)
    {
        return CanConsumeResourceValue(new Dictionary<string, float>() { { name, amount } });
    }
}
