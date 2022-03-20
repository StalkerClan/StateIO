using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public GlobalVariables.UIType Type;

    public virtual void OnInit()
    {

    }

    public virtual void OnOpen()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void OnClose()
    {
        gameObject.SetActive(false);
    }

    public virtual bool IsActive()
    {
        return gameObject.activeInHierarchy ? true : false;
    }
}
