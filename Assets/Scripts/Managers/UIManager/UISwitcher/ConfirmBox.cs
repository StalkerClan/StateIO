using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmBox : MonoBehaviour
{
    public void Confirm()
    {
        GameManager.Instance.SwitchState(GameState.MainMenu);     
    }

    public void Cancel()
    {
        if (UIManager.Instance.LastActiveCanvas is UIGameplay)
        {
            (UIManager.Instance.LastActiveCanvas as UIGameplay).OnPlaying();
        }
    }
}
