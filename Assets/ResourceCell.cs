using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;

public class ResourceCell : MonoBehaviour
{
    public TMP_Text resourceName;
    private string resourceId;
    public int maxValue = 1000;
    public void Init(string resourceId)
    {
        this.resourceId = resourceId;

        UpdateValue();
        EventPool.OptIn("updateResource", UpdateValue);
    }


    void UpdateValue()
    {
        //float format to string
        
        resourceName.text =$"{Utils.getIconInString(resourceId)} "+Utils. formatFloat(ResourceManager.Instance.GetResource(resourceId).Value) + "\n+"+ Utils. formatFloat(ResourceManager.Instance.GetResource(resourceId).IncreasePerSecond)+$"/{Utils.getIconInString("day")}";
        if (GetComponentInChildren<ProgressBar>())
        {
            GetComponentInChildren<ProgressBar>().UpdateProgress(ResourceManager.Instance.GetResource(resourceId).Value, maxValue);
            if (ResourceManager.Instance.GetResource(resourceId).Value >= maxValue)
            {
                //gameover
                    GameManager.Instance.Lose();  
            }
        }
    }
}
