using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseMenu : MenuBase
{
    public  Button restartButton;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
         restartButton.onClick.AddListener(() =>
         {
             GameManager.Instance.Restart();
         });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
