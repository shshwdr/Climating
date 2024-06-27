using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    private bool isPause;
    public Button pauseButton;
    private int speedId = 1;
    private List<float> speeds = new List<float>() { 0.5f, 1, 2, 4 };
    public Button speedButton;

    private void Awake()
    {
        pauseButton.onClick.AddListener(pause);
        speedButton.onClick.AddListener(changeSpeed);
        speedButton.GetComponentInChildren<TMP_Text>().text = $"Speed: X{speeds[speedId]}";
    }

    public void pause()
    {
        isPause = !isPause;
        GameLoopManager.Instance.PlayerControlPauseGame(isPause);
        pauseButton.GetComponentInChildren<TMP_Text>().text = isPause ? "Resume" : "Pause";
    }

    public void changeSpeed()
    {
        speedId++;
        if (speedId >= speeds.Count)
        {
            speedId = 0;
        }

        speedButton.GetComponentInChildren<TMP_Text>().text = $"Speed: X{speeds[speedId]}";
        GameLoopManager.Instance.ChangeSpeed(speeds[speedId]);
    }
}