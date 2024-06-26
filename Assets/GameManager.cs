using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        CSVManager.Instance.Init();
        ResourceManager.Instance.Init();
        EventCardManager.Instance.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Lose()
    {
        LoseMenu.FindFirstInstance<LoseMenu>().Show();
    }
}
