using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;

public class ResourceCell : MonoBehaviour
{
    public TMP_Text resourceName;
    private string resourceId;
    public void Init(string resourceId)
    {
        this.resourceId = resourceId;

        UpdateValue();
        EventPool.OptIn("updateResource", UpdateValue);
    }

    void UpdateValue()
    {
        resourceName.text =resourceId+": "+ ResourceManager.Instance.GetResource(resourceId).Value.ToString() + " +"+ ResourceManager.Instance.GetResource(resourceId).IncreasePerSecond.ToString()+"/sec";
    }
}
