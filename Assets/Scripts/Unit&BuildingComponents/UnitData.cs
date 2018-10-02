using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { LightSoldier = 0, HeavySoldier = 1, LightTank = 2, HeavyTank  = 3}

[CreateAssetMenu]
public class UnitData : ScriptableObject
{
    [Header("Creation")]
    public string UnitName;
    public int Cost;
    public Sprite Graphic;

    [Header("Settings")]
    public float HealthbarHeight;
    public Vector2 BoxColliderSize;
    public Vector2 SelectedGraphicSize;

    [Header("Health")]
    public float MaxHealth;
    public float MaxHealthbarWidth;

    [Header("Movement")]
    public float MoveSpeed;

    [Header("Attack")]
    public float Range;
    public float Damage;
    public float AttacksPerSecond;


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
}
