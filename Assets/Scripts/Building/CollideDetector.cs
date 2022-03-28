using System;
using UnityEngine;

public class CollideDetector : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Building building;

    public Building Building { get => building; set => building = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }

    private void Start()
    {
        HideRadius(spriteRenderer.color);
    }

    public void HideRadius(Color tempColor)
    {
        tempColor.a = 0f;
        spriteRenderer.color = tempColor;
    }

    public void ShowRadius(Color tempColor)
    {
        tempColor.a = 0.2f;
        spriteRenderer.color = tempColor;
    }
}
