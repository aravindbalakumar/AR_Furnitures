using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extensions
{
    public static bool isValidString(this string incomingString)
    {
        return !(string.IsNullOrEmpty(incomingString) && string.IsNullOrWhiteSpace(incomingString));
    }

    public static bool IsPointerOverUI(Vector3 ScreenPosition)
    {
        var eventSystem = EventSystem.current;
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = ScreenPosition;
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}