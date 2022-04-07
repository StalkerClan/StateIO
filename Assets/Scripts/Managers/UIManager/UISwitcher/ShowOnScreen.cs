using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowOnScreen : MonoBehaviour, ISubcriber
{
    public TextMeshProUGUI TextInfo;

    public virtual void SubcribeEvent()
    {

    }

    public virtual void UnsubcribeEvent()
    {

    }

    public virtual void ShowInfo(string data)
    {

    }
}
