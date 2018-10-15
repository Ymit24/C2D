using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { LightSoldier = 0, HeavySoldier = 1, LightTank = 2, HeavyTank  = 3}

[CreateAssetMenu]
public class UnitData : GameObjectData
{
	[Header("Movement")]
	public float MoveSpeed;
    [Header("Creation")]
    public UnitType Type;
	[Header("Settings")]
	public Vector2 SelectedGraphicSize;

    public static string NameFromType(UnitType type)
    {
        switch (type)
        {
            case UnitType.LightSoldier:
                return "Light Soldier";
            case UnitType.HeavySoldier:
                return "Heavy Soldier";
            case UnitType.LightTank:
                return "Light Tank";
            case UnitType.HeavyTank:
                return "Heavy Tank";
            default:
                return "No Unit Name";
        }
    }

    public static bool isSoldier(UnitType type)
    {
        return type == UnitType.LightSoldier || type == UnitType.HeavySoldier;
    }

    public static bool isTank(UnitType type)
    {
        return type == UnitType.LightTank|| type == UnitType.HeavyTank;
    }
}
