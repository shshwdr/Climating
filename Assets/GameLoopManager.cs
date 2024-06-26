using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : Singleton<GameLoopManager>
{ 
    bool UIPause = false;
    bool PlayerControlPause = false;

    public void UIPauseGame(bool pause)
    {
        UIPause = pause;
        
    }
    public void PlayerControlPauseGame(bool pause)
    {
        PlayerControlPause = pause;
        
    }

    void UpdatePause()
    {
        if (UIPause || PlayerControlPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
