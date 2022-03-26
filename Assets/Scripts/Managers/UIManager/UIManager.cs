using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Transform Parent;

    public Dictionary<GlobalVariables.UIType, UICanvas> CanvasPrefabs = new Dictionary<GlobalVariables.UIType, UICanvas>();
    public Dictionary<GlobalVariables.UIType, UICanvas> CanvasDict = new Dictionary<GlobalVariables.UIType, UICanvas>();

    public UICanvas CurrentCanvas;
    public UICanvas LastActiveCanvas;

    private void Awake()
    {
        UICanvas[] canvases = Resources.LoadAll<UICanvas>("UI/");

        for (int i = 0; i < canvases.Length; i++)
        {
            CanvasPrefabs.Add(canvases[i].Type, canvases[i]);
        }
    }

    public UICanvas OpenUI(GlobalVariables.UIType type)
    {
        if (LastActiveCanvas != null)
        {
            LastActiveCanvas.OnClose();
        }

        UICanvas desiredCanvas = null;

        if (!CanvasDict.ContainsKey(type) || CanvasDict[type] == null)
        {
            desiredCanvas = Instantiate(CanvasPrefabs[type], Parent);
            CanvasDict.Add(type, desiredCanvas);
        }
        else
        {
            desiredCanvas = CanvasDict[type];
        }       

        desiredCanvas.OnInit();
        desiredCanvas.OnOpen();
        LastActiveCanvas = desiredCanvas;
        CurrentCanvas = desiredCanvas;
        return desiredCanvas;
    }

    public void CloseUI(GlobalVariables.UIType type)
    {
        UICanvas desiredCanvas = null;
        if (!CanvasDict.ContainsKey(type) || CanvasDict[type] == null)
        {
            return; 
        }
        else
        {
            desiredCanvas = CanvasDict[type];
        }
        desiredCanvas.OnClose();
    }

    public bool IsOpened(GlobalVariables.UIType type)
    {
        UICanvas desiredCanvas = null;
        if (!CanvasDict.ContainsKey(type) || CanvasDict[type] == null)
        {
            return false;
        }
        else
        {
            desiredCanvas = CanvasDict[type];
            return desiredCanvas.IsActive();
        }
    }

    public UICanvas GetUI(GlobalVariables.UIType type)
    {
        UICanvas desiredCanvas = null;
        if (!CanvasDict.ContainsKey(type) || CanvasDict[type] == null)
        {
            return null;
        }
        else
        {
            desiredCanvas = CanvasDict[type];
        }
        return desiredCanvas;
    }    
}
