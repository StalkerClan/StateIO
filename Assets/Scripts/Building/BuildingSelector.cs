using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector : MonoBehaviour
{
    [SerializeField] private CircleCollider2D col;
    [SerializeField] private Building building;
    [SerializeField] private Arrow arrowInstance;
    [SerializeField] private bool hasArrow;
    [SerializeField] private bool deselected;
    [SerializeField] private bool outOfRadius;
    

    public CircleCollider2D Col { get => col; set => col = value; }
    public Building Building { get => building; set => building = value; }
    public Arrow ArrowInstance { get => arrowInstance; set => arrowInstance = value; }
    public bool HasArrow { get => hasArrow; set => hasArrow = value; }
    public bool Deselected { get => deselected; set => deselected = value; }
    public bool OutOfRadius { get => outOfRadius; set => outOfRadius = value; }
}
