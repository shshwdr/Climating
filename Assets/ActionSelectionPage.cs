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
   public TMP_Text durationCurrentEffect;
   public TMP_Text disableReason;
   public Button confirmButton;
   public Button removeButton;

   private bool isRemoving = false;

   private HexTile hexTile;
   private TileActionInfo actionInfo;
   private void Awake()
   {
       confirmButton.onClick.AddListener(() =>
       {
           hexTile.startAction(actionInfo);
           
           //consume cost
           ResourceManager.Instance.ConsumeResourceValue(actionInfo.actionCost);
           Hide();
       });
       
       removeButton.onClick.AddListener(() =>
       {
           hexTile.startStopAction(actionInfo);
           Hide();
       });
   }

   public void Show(HexTile hexTile)
    {
        this.hexTile = hexTile;
        Show();
        if (hexTile.action!=null)
        {
            isRemoving = true;
            for (int i = 0; i < selectionButtonParent.childCount; i++)
            {
                selectionButtonParent.GetChild(i).gameObject.SetActive(false);
            }
            Show(hexTile.action);
        }
        else
        {
            isRemoving = false;
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
        
    }

    public void Show(TileActionInfo info)
    {
        
        disableReason.gameObject.SetActive(false);
        actionInfo = info;
         actionName.text = info.actionName;
         actionDescription.text = info.actionDescription;
         if (isRemoving)
         {
             actionCost.text = "time: 2 day";
         }
         else
         {
             actionCost.text = "cost: "+Utils.StringifyDictionary( info.actionCost) +" time: "+info.actionTime+" day";
         }
         effect.text ="effect: "+ Utils.StringifyDictionary( info.actionEffect);
         durationEffect.text = "duration Effect: "+Utils.StringifyDictionary( info.actionDurationEffect);
         durationCurrentEffect.text = "current duration Effect multiplier: "+ this.hexTile.effectMultiplier(info);
         
         
        
         if (isRemoving)
         {
             removeButton.gameObject.SetActive(true);
             confirmButton.gameObject.SetActive(false);
         }
         else
         {
             removeButton.gameObject.SetActive(false);

             var reason = hexTile.disableActionReason(actionInfo);
             if (reason == null)
             {
                
                 confirmButton.gameObject.SetActive(true);
                 disableReason.gameObject.SetActive(false);
                 confirmButton.interactable =  ResourceManager.Instance.CanConsumeResourceValue(actionInfo.actionCost);
             }
             else
             {
                 disableReason.gameObject.SetActive(true);
                 disableReason.text = reason;
                 confirmButton.gameObject.SetActive(false);
             }
            
         }
    }
}
