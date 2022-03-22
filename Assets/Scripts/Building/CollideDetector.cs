using UnityEngine;

public class CollideDetector : MonoBehaviour
{
    public Building Building;
    public string buildingID;

    public string BuildingID { get => buildingID; set => buildingID = value; }

    private void Start()
    {       
        buildingID = Building.BuildingID;
    }
}
