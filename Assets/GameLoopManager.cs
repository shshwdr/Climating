using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : Singleton<GameLoopManager>
{ 
    bool UIPause = false;
    bool PlayerControlPause = false;

    private float speed = 1;
    public void UIPauseGame(bool pause)
    {
        UIPause = pause;
        UpdatePause();

    }
    public void PlayerControlPauseGame(bool pause)
    {
        PlayerControlPause = pause;
        UpdatePause();
        
    }

    public void ChangeSpeed(float speed)
    {
         this.speed = speed;
         if (UIPause || PlayerControlPause)
         {
             
         }
         else
         {
             Time.timeScale = speed;
         }
    }
    void UpdatePause()
    {
        if (UIPause || PlayerControlPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = speed;
        }
    }
}
