using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : Singleton<StoreManager>
{
    public Transform BuildingScrollView;
    public Transform FighterScrollView;
    public Transform ColorScrollView;
    public GameObject ButtonPrefab;
    public OwnerStat UserStat;

    private void Awake()
    {
        UserStat = JSONSaving.Instance.UserStat;
        GetBuildingCosmetic();
        GetFighterCosmetic();
        GetColorCosmetic();
    }

    public void GetBuildingCosmetic()
    {
        Cosmetic[] cosmetics = Resources.LoadAll<Cosmetic>("Cosmetics/Buildings/");

        for (int i = 0; i < cosmetics.Length; i++)
        {
            GameObject newButton = Instantiate(ButtonPrefab, BuildingScrollView);
            SkinPicker skinPicker = newButton.GetComponent<SkinPicker>();
            skinPicker.Cosmetic = cosmetics[i];
            skinPicker.Image.sprite = cosmetics[i].Artwork;
        }
    }

    public void GetFighterCosmetic()
    {
        Cosmetic[] cosmetics = Resources.LoadAll<Cosmetic>("Cosmetics/Fighter/");

        for (int i = 0; i < cosmetics.Length; i++)
        {
            GameObject newButton = Instantiate(ButtonPrefab, FighterScrollView);
            SkinPicker skinPicker = newButton.GetComponent<SkinPicker>();
            skinPicker.Cosmetic = cosmetics[i];
            skinPicker.Image.sprite = cosmetics[i].Artwork;
        }
    }

    public void GetColorCosmetic()
    {
        Cosmetic[] cosmetics = Resources.LoadAll<Cosmetic>("Cosmetics/Color/");

        for (int i = 0; i < cosmetics.Length; i++)
        {
            GameObject newButton = Instantiate(ButtonPrefab, ColorScrollView);
            SkinPicker skinPicker = newButton.GetComponent<SkinPicker>();
            skinPicker.Cosmetic = cosmetics[i];
            skinPicker.Image.sprite = cosmetics[i].Artwork;
        }
    }
}
