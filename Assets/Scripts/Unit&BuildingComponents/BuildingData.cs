using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType { CommandCenter, Barracks, TankFactory, PowerPlant }

[CreateAssetMenu]
public class BuildingData : ScriptableObject
{
    [Header("Creation")]
    public string BuildingName;
    public BuildingType Type;
    public int Cost;
    public Sprite Graphic;

    [Header("Settings")]
    public float HealthbarHeight;
    public Vector2 BoxColliderSize;

    [Header("Health")]
    public float MaxHealth;
    public float MaxHealthbarWidth;

    [Header("Attack")]
    public bool CanAttack;
    public float Range;
    public float Damage;
    public float AttacksPerSecond;

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
