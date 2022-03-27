using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector : MonoBehaviour
{
    [SerializeField] private Building building;

    public Building Building { get => building; set => building = value; }
}
