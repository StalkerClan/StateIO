using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color targetBuildingColor;
    private BuildingSelector selector;
    private Building selectedBuilding;
    private Vector3 target;
    private Vector2 difference;
    private Vector2 modifiedSize = new Vector2(0f, 5.12f);
    private float angle;
    private float distance;
    private bool outOfBorder;

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public Color TargetBuildingColor { get => targetBuildingColor; set => targetBuildingColor = value; }
    public BuildingSelector Selector { get => selector; set => selector = value; }
    public Building SelectedBuilding { get => selectedBuilding; set => selectedBuilding = value; }
    public float Distance { get => distance; set => distance = value; }
    public bool OutOfBorder { get => outOfBorder; set => outOfBorder = value; }

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
        SpriteRenderer.size = modifiedSize;
    }

    public void DeSpawn()
    {
        ObjectPooler.Instance.ReturnGameObject(this.gameObject);
    }
}
