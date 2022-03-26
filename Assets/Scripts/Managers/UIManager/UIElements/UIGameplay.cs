using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplay : UICanvas
{
    public GameObject PExit;
    public GameObject PConfirm;

    public void OnPlaying()
    {
        PConfirm.SetActive(false);
        PExit.SetActive(true);
    }

    public void OnExitGamePlay()
    {
        PConfirm.SetActive(true);
        PExit.SetActive(false);
    }
}
