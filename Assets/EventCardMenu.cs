using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

public class EventCardMenu : MonoBehaviour
{
    public Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        UpdateMenu();
        EventPool.OptIn("updateEvent",UpdateMenu);
    }

    // Update is called once per frame
    void UpdateMenu()
    {
        int i = 0;
        var cards = parent.GetComponentsInChildren<EventCard>(true);
        for (; i < EventCardManager.Instance.currentEventList.Count; i++)
        {
            cards[i].Init(EventCardManager.Instance.currentEventList[i]);
            cards[i].gameObject.SetActive(true);
        }
        for (; i < cards.Length; i++)
        {
            cards[i].gameObject.SetActive(false);
        }
    }
}
