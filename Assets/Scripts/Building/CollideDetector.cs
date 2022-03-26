using System;
using UnityEngine;

public class CollideDetector : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Building building;

    private Color hideRange;
    private Color showRange;

    public Building Building { get => building; set => building = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }

    private void Start()
    {
        hideRange = showRange = spriteRenderer.color;
        hideRange.a = 0f;
        showRange.a = 0.2f;
        HideSelectedRange();
    }

    public void HideSelectedRange()
    {
        spriteRenderer.color = hideRange;
    }

    public void ShowSelectedRange()
    {
        spriteRenderer.color = showRange;
    }
}
