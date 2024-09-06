using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraphicRaycastController : Singleton<GraphicRaycastController>
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GraphicRaycaster raycaster;

    public bool Raycast(Vector2 position)
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = position;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        return results != null && results.Count > 0;
    }
}
