using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour, IPointerClickHandler
{
    public TabGroup TabGroup;

    public void OnPointerClick(PointerEventData eventData)
    {
        TabGroup.OnTabEnter(this);
    }
}
