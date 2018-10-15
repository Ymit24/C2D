using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType { CommandCenter, Barracks, TankFactory, PowerPlant }

[CreateAssetMenu]
public class BuildingData : GameObjectData
{
    [Header("Creation")]
    public BuildingType Type;

    [Header("UnitFactory")]
    public bool CanBuildUnits;
    public List<UnitType> UnitTypes;

    public static string NameFromType(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.CommandCenter:
                return "Command Center";
            case BuildingType.Barracks:
                return "Barracks";
            case BuildingType.TankFactory:
                return "Tank Factory";
            case BuildingType.PowerPlant:
                return "Power Plant";
            default:
                return "No Building Name";
        }
    }
}
