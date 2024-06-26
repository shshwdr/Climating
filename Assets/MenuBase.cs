using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBase : MonoBehaviour
{
    public GameObject menu;
    public Button closeButton;
    public static T FindFirstInstance<T>() where T : MenuBase
    {
        T instance = FindObjectOfType<T>();
        if (instance == null)
        {
            Debug.LogWarning($"No instance of {typeof(T).Name} found in the scene.");
        }
        return instance;
    }
    protected virtual void Start()
    {
        Hide();
        if (closeButton)
        {
            closeButton.onClick.AddListener(() =>
            {
                Hide();
            });
        }
    }

    virtual public void Init()
    {
    }

    virtual public void Show()
    {
        menu.SetActive(true);
        GameLoopManager.Instance.UIPauseGame(true);
    }
    virtual public void Hide()
    {
        menu.SetActive(false);
        GameLoopManager.Instance.UIPauseGame(false);
        
    }
}