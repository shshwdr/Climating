using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexTileController : MonoBehaviour
{
    public SpriteRenderer bkImage;
    
    public SpriteRenderer iconImage;
    public SpriteRenderer actionIcon;
    private bool isExplored => hexTile.isExplored;
    bool isActioned=> hexTile.action != null;
    TileActionInfo actionInfo=> hexTile.action;
    public Color unExploredColor = Color.gray;

    public HexTile hexTile;
    // Start is called before the first frame update


    public void Init(HexTile tile, bool isExplored = false)
    {
         hexTile = tile;
       //  iconImage.sprite = icon;
         UpdateView();
    }
    public void UpdateView()
    {

        if (isExplored || isReadyToExplore())
        {
            bkImage.color = isExplored ? Color.white : unExploredColor;
            bkImage.gameObject.SetActive(!isExplored);
            iconImage.gameObject.SetActive((isExplored));
            gameObject.SetActive(true);

            if (isActioned)
            {
                actionIcon.sprite = Resources.Load<Sprite>("actionIcon/"+actionInfo.image);
                actionIcon.gameObject.SetActive(true);
            }
            else
            {
                
                actionIcon.gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    bool isReadyToExplore()
    {
        bool readyToExplore = false;
        foreach (var neighbor in HexGridManager.Instance.GetNeighbors (hexTile))
        {
            if(neighbor.isExplored)
            {
                readyToExplore = true;
                break;
            }
        }

        return readyToExplore;
    }

    public void OnClick(bool force = false)
    {
        if (!isExplored)
        {

            if (isReadyToExplore())
            {
                hexTile.isExplored = true;
                UpdateView();
                foreach (var neighbor in HexGridManager.Instance.GetNeighbors(hexTile))
                {
                    HexGridManager.Instance.hexTileToControllerDict[neighbor].UpdateView();
                }
            }
        }
        else if(isActioned)
        {
            //destroy previous action
            
        }
        else
        {
            //show action view
            ActionSelectionPage.FindFirstInstance<ActionSelectionPage>().Show(hexTile);
        }
        
        //GetComponent<HexTileBase>().OnClick();
    }
}
