using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkinPicker : MonoBehaviour, IPointerClickHandler
{
    public Image Image;
    public Button Button;
    public Cosmetic Cosmetic;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
