using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPage : MenuBase
{
    public TMP_Text eventName;
    public TMP_Text eventDescription;
    public TMP_Text eventDetailDescription;

    public List<Button> buttons;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    public void Show(EventInfoData eventData)
    {
         eventName.text = eventData.eventInfo.eventName;
         eventDescription.text = eventData.eventInfo.eventDescription;
         eventDetailDescription.text = eventData.eventInfo.eventDetailDescription;
          Show();
        
    }
}
