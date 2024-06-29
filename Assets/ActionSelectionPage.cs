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
   public TMP_Text tileName;
   private bool isRemoving = false;

   public Image currentActionImage;
   private HexTile hexTile;
   private TileActionInfo actionInfo;
   private void Awake()
   {
       confirmButton.onClick.AddListener(() =>
       {
           if (ControllerManager.Instance.canBuild)
           {
               
               hexTile.startAction(actionInfo);
           
               //consume cost
               ResourceManager.Instance.ConsumeResourceValue(actionInfo.actionCost);
               Hide();
           }
           else
           {
               
               ToastManager.Instance.ShowToast("Builders are busy");
           }
       });
       
       removeButton.onClick.AddListener(() =>
       {
           if (ControllerManager.Instance.canBuild)
           {
           hexTile.startStopAction(actionInfo);
           Hide();
           }
           else
           {
               ToastManager.Instance.ShowToast("Builders are busy");
           }
       });
   }

   public void Show(HexTile hexTile)
    {
        this.hexTile = hexTile;
        Show();
        tileName.text = hexTile.info.tileName;
        if (hexTile.action!=null)
        {
            isRemoving = true;
            currentActionImage.gameObject.SetActive(true);
            currentActionImage.sprite = hexTile.action.sprite;
            for (int i = 0; i < selectionButtonParent.childCount; i++)
            {
                selectionButtonParent.GetChild(i).gameObject.SetActive(false);
            }
            Show(hexTile.action);
        }
        else
        {
            isRemoving = false;
            currentActionImage.gameObject.SetActive(false);
            var allAvailableActions = hexTile.info.tileActionInfoList;
            int i = 0;
            for(i = 0;i<allAvailableActions.Count;i++)
                //foreach (var actionButton in selectionButtonParent.GetComponentsInChildren<ActionButton>(true))
            {
                selectionButtonParent.GetChild(i).gameObject.SetActive(true);
                selectionButtonParent.GetChild(i).GetComponent<ActionButton>().Init(allAvailableActions[i]);
                
                var reason = hexTile.disableActionReason(allAvailableActions[i]);
                var color = Color.white;
                color.a = reason == null ? 1 : 0.5f;
                selectionButtonParent.GetChild(i).GetComponent<Button>().image.color =  color;
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
        for (int i = 0; i < selectionButtonParent.childCount; i++)
        {
            if (info == selectionButtonParent.GetChild(i).gameObject.GetComponent<ActionButton>().info)
            {
                
                selectionButtonParent.GetChild(i).gameObject.GetComponent<ActionButton>().Select();
            }
            else
            {
                
                selectionButtonParent.GetChild(i).gameObject.GetComponent<ActionButton>().UnSelect();
            }
        }
        
        disableReason.gameObject.SetActive(false);
        actionInfo = info;
         actionName.text = info.actionName;
         actionDescription.text = info.actionDescription;
         if (isRemoving)
         {
             actionCost.text = $"Time: 2 {Utils.getIconInString("day")}";
         }
         else
         {
             actionCost.text = "Cost: "+Utils.StringifyDictionary( info.actionCost) +"\nTime: "+info.actionTime+Utils.getIconInString("day");
         }
         //effect.text ="effect: "+ Utils.StringifyDictionary( info.actionEffect);
         durationEffect.text = $"Effect /{Utils.getIconInString("day")}: \n"+Utils.StringifyDictionary( info.actionDurationEffect);
         durationCurrentEffect.text = "Current Effect Multiplier: "+ this.hexTile.effectMultiplier(info);
         
         
        
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
                 //confirmButton.interactable =  ResourceManager.Instance.CanConsumeResourceValue(actionInfo.actionCost);
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
