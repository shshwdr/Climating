using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerManager : Singleton<ControllerManager>
{
    public float zoomSpeed = 10.0f; // Speed of zooming in and out
    public float minZoom = 5.0f; // Minimum camera orthographic size
    public float maxZoom = 20.0f; // Maximum camera orthographic size
    public float panSpeed = 20.0f; // Speed of panning

    public bool canExplore=> exploreCount < maxExploreCount;
    public bool canBuild=> buildCount < maxBuildCount;
    public int maxBuildCount = 1;
    public int maxExploreCount = 1;
    [HideInInspector]
    public int buildCount = 0;
    [HideInInspector]
    public int exploreCount = 0;

    private Vector3 dragOrigin;
    private HexTileController currentSelectedTile;
    void Update()
    {
        
        // Zoom in and out with the mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            Camera.main.orthographicSize -= scroll * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }

        // Pan with the right mouse button
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
            Vector3 move = new Vector3(difference.x * panSpeed, difference.y * panSpeed, 0);
            Camera.main.transform.Translate(move, Space.World);
            dragOrigin = Input.mousePosition;
        }
        
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
                    var currentHitTile = hit.collider.GetComponent<HexTileController>();
                    if (currentHitTile.isExplored)
                    {
                        if (currentSelectedTile)
                        {
                            currentSelectedTile.HidePreExploreView();
                            currentSelectedTile = null;
                        }
                        currentHitTile.OnClick(true);
                    }
                    else
                    {
                        
                        if (currentSelectedTile == currentHitTile)
                        {
                            currentSelectedTile.OnClick(true);
                            currentSelectedTile.HidePreExploreView();
                            currentSelectedTile = null;
                        }
                        else
                        {
                            if (currentSelectedTile)
                            {
                                currentSelectedTile.HidePreExploreView();
                            }
                            currentSelectedTile = currentHitTile;
                            currentSelectedTile.ShowPreExploreView();
                        }
                    }
                    // If true, the mouse is over a hex tile, so ignore other input
                    return;
                }
            }
        }
    }
    
    public Vector2 panLimitMin; // Minimum pan boundary
    public Vector2 panLimitMax; // Maximum pan boundary

    void LateUpdate()
    {
        // Clamp the camera position within the pan limits
        Vector3 pos = Camera.main.transform.position;
        pos.x = Mathf.Clamp(pos.x, panLimitMin.x, panLimitMax.x);
        pos.y = Mathf.Clamp(pos.y, panLimitMin.y, panLimitMax.y);
        Camera.main.transform.position = pos;
    }
}
