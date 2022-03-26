using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenSettings : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.CloseUI(GlobalVariables.UIType.MainMenu);
        UIManager.Instance.OpenUI(GlobalVariables.UIType.Settings);

    }
}
