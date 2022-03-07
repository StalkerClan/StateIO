using UnityEngine;

public class CollideDetector : MonoBehaviour
{
    public Building building;

    public string buildingID;

    public string BuildingID { get => buildingID; set => buildingID = value; }

    private void Start()
    {       
        buildingID = building.BuildingID;
    }
}
