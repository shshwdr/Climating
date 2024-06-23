using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    private TileActionInfo info;
    public Image icon;
    public Button button;
    public GameObject selectedImage;
    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            ActionSelectionPage.FindFirstInstance<ActionSelectionPage>().Show(info);
        });
    }

    public void Init(TileActionInfo info)
    {
        this.info = info;
        icon.sprite = Resources.Load<Sprite>("actionIcon/"+info.image);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
