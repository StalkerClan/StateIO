using UnityEngine.EventSystems;

public class BuildingSkinPicker : SkinPicker
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (this.Cosmetic.Unlocked)
        {
            StoreManager.Instance.UserStat.BuildingIcon = this.Cosmetic.Artwork;
        }
    }
}
