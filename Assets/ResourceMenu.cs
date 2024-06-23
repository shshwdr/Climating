using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMenu : MonoBehaviour
{
    public Transform resourceParent;
    private void Start()
    {
        int i = 0;
        foreach (var resource in ResourceManager.Instance.Resources)
        {
            resourceParent.GetComponentsInChildren<ResourceCell>()[i].Init(resource.Name);
            i++;
        }
    }
}
