using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

        if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // If true, the mouse is over a UI element, so ignore other input
                return;
            }
            
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.tag == "HexTile")
                {
                    hit.collider.GetComponentInParent<HexTileController>().OnClick();
                    // If true, the mouse is over a hex tile, so ignore other input
                    return;
                }
            }
        }
    }
}
