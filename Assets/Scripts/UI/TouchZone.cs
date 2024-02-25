using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchZone : MonoBehaviour, IService, IPointerDownHandler
{
    public event Action Touch;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Touch");
        Touch?.Invoke();
    }
}
