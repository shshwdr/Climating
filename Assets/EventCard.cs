using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventCard : MonoBehaviour
{
    public TMP_Text description;

    public ProgressBar progressBar;

    private EventInfoData data;
    // Start is called before the first frame update
    void Start()
    {
        
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
