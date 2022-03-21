using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackToMainMenu : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.SwitchState(GameState.MainMenu);
        LevelManager.Instance.LevelGenerator.SetBuildingToDefault();
    }
}
