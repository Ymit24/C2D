using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
    private static MapController _instance;
    public static List<BuildingType>[] BuildingsPerPlayer;
    public static List<UnitType>[] UnitsPerPlayer;

    public int PlayerCount; // TODO: implement a map configuration scriptable object with this in it

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("MapController already exists!");
            Destroy(this);
        }
    }
    private void Start()
    {
        BuildingsPerPlayer = new List<BuildingType>[PlayerCount];
        UnitsPerPlayer = new List<UnitType>[PlayerCount];

        for (int i = 0; i < PlayerCount; i++)
        {
            BuildingsPerPlayer[i] = new List<BuildingType>();
            UnitsPerPlayer[i] = new List<UnitType>();
        }
    }

    public static int SoldierCount(int team)
    {
        if (team < 0 || team >= _instance.PlayerCount)
            return -1;
        int count = 0;
        foreach (UnitType type in UnitsPerPlayer[team])
        {
            if (type == UnitType.LightSoldier || type == UnitType.HeavySoldier)
                count++;
        }
        return count;
    }

    public static int TankCount(int team)
    {
        if (team < 0 || team >= _instance.PlayerCount)
            return -1;
        int count = 0;
        foreach (UnitType type in UnitsPerPlayer[team])
        {
            if (type == UnitType.LightTank || type == UnitType.HeavyTank)
                count++;
        }
        return count;
    }
}
