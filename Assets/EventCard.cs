using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventCard : MonoBehaviour
{
    public TMP_Text description;

    public ProgressBar progressBar;

    private EventInfoData data;

    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            EventPage.FindFirstInstance<EventPage>().Show(data);
        });
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.UpdateProgress(data.eventInfo.duration - data.timer,data.eventInfo.duration);
    }

    public void Init(EventInfoData data)
    {
        this.data = data;
        description.text = data.eventInfo.eventDescription;
    }
}
