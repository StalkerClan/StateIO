using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public UIController UIController;

    public void ShowUI(GameObject uiElement)
    {
        uiElement.SetActive(true);
    }

    public void HideUI(GameObject uiElement)
    {
        uiElement.SetActive(false);
    }
}
