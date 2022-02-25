using UnityEngine;

public class State : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public GameObject owner;
    public Color stateColor;

    private void OnEnable()
    {
        spriteRenderer.color = owner.GetComponent<Building>().BuildingColor;
    }
}
