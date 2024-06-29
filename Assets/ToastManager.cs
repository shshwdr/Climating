using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastData
{
    public string toast;
    public List<Sprite> sprites;
}
public class ToastManager : Singleton<ToastManager>
{
    public GameObject toastOb;

    public TMP_Text toastLabel;

    List<ToastData> toastList = new List<ToastData>();
    
    public float toastVisibleTime = 4f;
    public float toastShowTime = 0.5f;
    private float toastTimer = 0;

    //public List<Image> images;

    public void ShowToast(string toast)
    {
        toastList.Add( new ToastData(){toast = toast, sprites = null});
    }


    private void Update()
    {
        if (toastList.Count > 0)
        {
            if (toastTimer <= 0)
            {
                toastTimer = toastShowTime;
                toastLabel.text = toastList[0].toast;
                //dotween canvas group alpha show wait then hide

                // foreach (var image in images)
                // {
                //     image.gameObject.SetActive(false);
                // }
                // if (toastList[0].sprites != null)
                // {
                //
                //     int i = 0;
                //     for (; i < toastList[0].sprites.Count; i++)
                //     {
                //         images[i].sprite = toastList[0].sprites[i];
                //         images[i].gameObject.SetActive(true);
                //     }
                // }

                toastOb.GetComponent<CanvasGroup>().DOFade(1, toastShowTime);
                toastOb.SetActive(true);
                toastList.RemoveAt(0);
                toastTimer = toastVisibleTime;
            }
            else
            {
                toastTimer -= Time.deltaTime;
            }
        }
        else
        {
            if (toastTimer <= 0)
            {
                toastOb.SetActive(false);
            }

            toastTimer -= Time.deltaTime;
        }
    }
}