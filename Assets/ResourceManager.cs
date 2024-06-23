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

    public Resource(string name, float initialValue, float increasePerSecond)
    {
        Name = name;
        Value = initialValue;
        IncreasePerSecond = increasePerSecond;
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
        AddResource((new Resource("money", 10, 0)));
        AddResource((new Resource("tech", 5, 0)));
        AddResource((new Resource("human", 5, 0)));
        AddResource((new Resource("polution", 0, 0)));
    }

    private IEnumerator UpdateResourceValues()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            //float deltaTime = Time.deltaTime;
            foreach (var resource in resources)
            {
                resource.IncreaseValueByTime(1);
                Debug.Log($"{resource.Name}: {resource.Value}");
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
    
    public void ProduceResourceValueIncrease(Dictionary<string ,float> value)
    {
        foreach (var item in value)
        {
            ProduceResourceValueIncrease(item.Key, item.Value);
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
}
