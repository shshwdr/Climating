using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPage : MenuBase
{
    public Image image;
    public TMP_Text eventName;
    public TMP_Text eventDescription;
    public TMP_Text eventDetailDescription;
    public List<Button> eventOptions;
    private EventInfoData eventData;
    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
        for (int i = 0; i < eventOptions.Count; i++)
        {
            var captureI = i;
            eventOptions[captureI].onClick.AddListener(() =>
            {
                ResourceManager.Instance.ConsumeResourceValue(eventData.eventInfo.costToFinishEvents[captureI]);
                EventCardManager.Instance.DealEvent(eventData);
                Hide();
            });
        }
    }

    public void Show(EventInfoData eventData)
    {
        this.eventData = eventData;
        image.sprite = eventData.eventInfo.sprite;
         eventName.text = eventData.eventInfo.eventName;
         var description = eventData.eventInfo.eventDescription.ReplaceDoubleQuotes();
         
         eventDescription.text = description;
         eventDetailDescription.text = eventData.eventInfo.eventDetailDescription;

         foreach (var option in eventOptions)
         {
             option.gameObject.SetActive(false);
         }
         for (int i = 0; i < eventData.eventInfo.nameToFinishEvents.Count; i++)
         {
             
             eventOptions[i].gameObject.SetActive(true);
             eventOptions[i].interactable =
                 ResourceManager.Instance.CanConsumeResourceValue(eventData.eventInfo.costToFinishEvents[i]);
             var text = eventData.eventInfo.nameToFinishEvents[i] + " cost: ";
             text+=Utils.StringifyDictionary(eventData.eventInfo.costToFinishEvents[i]);
             eventOptions[i].GetComponentInChildren<TMP_Text>().text = text;
             var captureI = i;
         }
         
          Show();
        
    }
}
