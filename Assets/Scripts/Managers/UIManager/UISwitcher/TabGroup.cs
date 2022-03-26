using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> TabButtons;
    public List<GameObject> Contents;
    public TabButton selectedTab;

    public void OnTabSubcribe(TabButton tabButton)
    {
        
    }

    public void OnTabEnter(TabButton tabButton)
    {
        selectedTab = tabButton;
        int index = TabButtons.IndexOf(selectedTab);
        for (int i = 0; i < TabButtons.Count; i++)
        {
            if (i != index)
            {
                Contents[i].SetActive(false);
            }
            else
            {
                Contents[i].SetActive(true);
            }
        }
    }

    public void OnTabExit(TabButton tabButton)
    {

    }
}
