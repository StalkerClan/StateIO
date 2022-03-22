using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackToMainMenu : MonoBehaviour, IPointerClickHandler
{
    public  void OnPointerClick(PointerEventData eventData)
    {
        if (UIManager.Instance.LastActiveCanvas is UIGameplay)
        {
            (UIManager.Instance.LastActiveCanvas as UIGameplay).OnExitGamePlay();
        }
        else
        {
            GameManager.Instance.SwitchState(GameState.MainMenu);
            LevelManager.Instance.LevelGenerator.SetBuildingToDefault();
        }     
    }
}
