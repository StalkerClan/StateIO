using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Building selectedBuilding;
    private Vector3 target;
    private Vector2 difference;
    private Vector2 modifiedSize = new Vector2(0f, 5.12f);
    private float angle;
    private float distance;

    public Building SelectedBuilding { get => selectedBuilding; set => selectedBuilding = value; }

    private void Update()
    {
        RotateByMousePosition();
    }

    public void RotateByMousePosition()
    {
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        difference = target - transform.position;
        angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        distance = Vector2.Distance(target, transform.position);
        modifiedSize.x = distance * 5f;
        spriteRenderer.size = modifiedSize;
    }

    public void DeSpawn()
    {
        ObjectPooler.Instance.ReturnGameObject(this.gameObject);
    }
}
