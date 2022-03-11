using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public  class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameplay;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject exitButton;

    public GameObject MainMenu { get => mainMenu; set => mainMenu = value; }
    public GameObject Gameplay { get => gameplay; set => gameplay = value; }
    public GameObject StartButton { get => startButton; set => startButton = value; }

    public void TapToPlay()
    {
        startButton.SetActive(false);
        LevelManager.Instance.EnableGeneratingFighter();
        GameManager.Instance.SwitchState(GameState.GameStart);
    }

    public void ExitToMainMenu()
    {
        startButton.SetActive(true);
        LevelManager.Instance.SetBuildingToDefault();
        GameManager.Instance.SwitchState(GameState.MainMenu);
    }
}
