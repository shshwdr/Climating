using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HexTileController : MonoBehaviour
{
    public SpriteRenderer bkImage;
    
    public SpriteRenderer iconImage;
    public SpriteRenderer actionIcon;
    public bool isExplored => hexTile.isExplored;
    bool isActioned=> hexTile.action != null;
    TileActionInfo actionInfo=> hexTile.action;
    public Color unExploredColor = Color.gray;
    public GameObject preExploreView;
    public TMP_Text exploreCostLabel;
    public HexTile hexTile;

    public Sprite exploreImage;
    public Sprite buildImage;
    
    public ProgressBar progressBar;
    // Start is called before the first frame update
    

    public void Init(HexTile tile, bool isExplored = false)
    {
         hexTile = tile;
         var candidates = Resources.LoadAll<Sprite>("hexIconGroup/" + tile.info.type).ToList();
         bkImage.sprite = candidates.RandomItem();
         
       //  iconImage.sprite = icon;
         UpdateView();
         HidePreViews();
         progressBar.gameObject.SetActive(false);
    }

    public void ShowPreExploreView()
    {
        preExploreView.SetActive(true);
        exploreCostLabel.text = (hexTile.exploreCost).ToString() +" days";
    }

    int ExploreTime()
    {
        return (int)(hexTile.exploreCost * ResourceManager.Instance.updateTime);
    }

    public void HidePreViews()
    {
        HidePreExploreView();
    }
    public void HidePreExploreView()
    {
        preExploreView.SetActive(false);
    }
    
    public void UpdateView()
    {
        

        if (isExplored || isReadyToExplore())
        {
            bkImage.color = isExplored ? Color.white : unExploredColor;
           // bkImage.gameObject.SetActive(!isExplored);
            iconImage.gameObject.SetActive((isExplored));
            gameObject.SetActive(true);

            if (isActioned && !hexTile.isActioning)
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

    void FinishAction()
    {
        
        //get immediate effect
        ResourceManager.Instance.ProduceResourceValue(actionInfo.actionEffect);
        
        ResourceManager.Instance.UpdateIncreaseResourceValues();
       // ResourceManager.Instance.ProduceResourceValueIncrease(actionInfo.actionDurationEffect);
    }

    void FinishRemoveAction()
    {
        hexTile.action = null;
        
        ResourceManager.Instance.UpdateIncreaseResourceValues();
        UpdateView();
        
    }

    void FinishExploring()
    {
        
        foreach (var neighbor in HexGridManager.Instance.GetNeighbors(hexTile))
        {
            HexGridManager.Instance.hexTileToControllerDict[neighbor].UpdateView();
        }
                
        ResourceManager.Instance.UpdateIncreaseResourceValues();
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

    private float exploreTime =>hexTile.exploreTime;
    private float exploreTimer = 0;
    private void Update()
    {
        if (!hexTile.isExplored && hexTile.isExploring)
        {
            exploreTimer+=Time.deltaTime;
            progressBar.gameObject.SetActive(true);
            progressBar.GetComponent<Image>().sprite = exploreImage;
            progressBar.UpdateProgress(exploreTimer, hexTile.exploreTime);

            if (exploreTimer >= exploreTime)
            {
                exploreTimer = 0;
                hexTile.isExplored = true;
                hexTile.isExploring = false;
                progressBar.gameObject.SetActive(false);
                UpdateView();
                FinishExploring();
            }
        }
        
        if ( hexTile.isActioning)
        {
            exploreTimer+=Time.deltaTime;
            progressBar.GetComponent<Image>().sprite = buildImage;
            progressBar.gameObject.SetActive(true);
            progressBar.UpdateProgress(exploreTimer, hexTile.exploreTime);

            if (exploreTimer >= exploreTime)
            {
                exploreTimer = 0;
                hexTile.isActioning = false;
                progressBar.gameObject.SetActive(false);
                UpdateView();
                FinishAction();
            }
        }
        if ( hexTile.isRemovingAction)
        {
            exploreTimer+=Time.deltaTime;
            progressBar.GetComponent<Image>().sprite = buildImage;
            progressBar.gameObject.SetActive(true);
            progressBar.UpdateProgress(exploreTimer, hexTile.exploreTime);

            if (exploreTimer >= exploreTime)
            {
                exploreTimer = 0;
                hexTile.isRemovingAction = false;
                progressBar.gameObject.SetActive(false);
                UpdateView();
                FinishRemoveAction();
            }
        }
    }

    public void OnClick(bool force = false)
    {
        if (!isExplored)
        {
            if (isReadyToExplore())
            {
                hexTile.isExploring = true;
                exploreTimer = 0;
                hexTile.exploreTime = ExploreTime();
                return;
            }
        }

        if (hexTile.isExploring)
        {
            //todo can cancel exploring
            return;
        }else if (hexTile.isActioning)
        {
            //todo can cancel action
            return;
        }
        else if(isActioned)
        {
            //destroy previous action
            ActionSelectionPage.FindFirstInstance<ActionSelectionPage>().Show(hexTile);
            
        }
        else
        {
            //show action view
            ActionSelectionPage.FindFirstInstance<ActionSelectionPage>().Show(hexTile);
        }
        
        //GetComponent<HexTileBase>().OnClick();
    }
}
