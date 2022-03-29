using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector : MonoBehaviour
{
    [SerializeField] private CircleCollider2D col;
    [SerializeField] private Building building;
    [SerializeField] private Arrow selectorArrow;
    [SerializeField] private bool hasArrow;
    [SerializeField] private bool outOfRadius;
    [SerializeField] private bool visited;

    public CircleCollider2D Col { get => col; set => col = value; }
    public Building Building { get => building; set => building = value; }
    public Arrow SelectorArrow { get => selectorArrow; set => selectorArrow = value; }
    public bool HasArrow { get => hasArrow; set => hasArrow = value; }
    public bool OutOfRadius { get => outOfRadius; set => outOfRadius = value; }
    public bool Visited { get => visited; set => visited = value; }
}
