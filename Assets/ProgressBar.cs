using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image progressImage;
    

    public void UpdateProgress(float current, float max)
    {
        progressImage.fillAmount = current / max;
    }
}
