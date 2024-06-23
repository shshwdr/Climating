using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionSelectionPage : MenuBase
{
   public Transform selectionButtonParent;
   public TMP_Text actionName;
   public TMP_Text actionDescription;
   public TMP_Text actionCost;
   public TMP_Text effect;
   public TMP_Text durationEffect;
   public Button confirmButton;

   private HexTile hexTile;
   private TileActionInfo actionInfo;
   private void Awake()
   {
       confirmButton.onClick.AddListener(() =>
       {
           hexTile.startAction(actionInfo);
           Hide();
       });
   }

   public void Show(HexTile hexTile)
    {
        this.hexTile = hexTile;
        Show();
        var allAvailableActions = hexTile.info.tileActionInfoList;
        int i = 0;
        for(i = 0;i<allAvailableActions.Count;i++)
        //foreach (var actionButton in selectionButtonParent.GetComponentsInChildren<ActionButton>(true))
        {
            selectionButtonParent.GetChild(i).gameObject.SetActive(true);
            selectionButtonParent.GetChild(i).GetComponent<ActionButton>().Init(allAvailableActions[i]);
        }

        for (; i < selectionButtonParent.childCount; i++)
        {
            selectionButtonParent.GetChild(i).gameObject.SetActive(false);
        }
        
        Show(allAvailableActions[0]);
    }

    public void Show(TileActionInfo info)
    {
        actionInfo = info;
         actionName.text = info.actionName;
         actionDescription.text = info.actionDescription;
         actionCost.text = Utils.StringifyDictionary( info.actionCost) +" time: "+info.actionTime;
         effect.text ="effect: "+ Utils.StringifyDictionary( info.actionEffect);
         durationEffect.text = "durationEffect: "+Utils.StringifyDictionary( info.actionDurationEffect);
    }
}
