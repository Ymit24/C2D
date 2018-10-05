using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
    private static MapController _instance;
    public static List<BuildingType>[] BuildingsPerPlayer;
    public static List<UnitType>[] UnitsPerPlayer;

    public static GameObject Map;
    public static AstarPath Pathfinder;
    public GameObject UnitHolder;
    public GameObject BuildingHolder;

    private MapConfiguration Config;
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
        Config = Transform.FindObjectOfType<MapConfiguration>();
    }
    private void Start()
    {
        BuildingsPerPlayer = new List<BuildingType>[Config.PlayerCount];
        UnitsPerPlayer = new List<UnitType>[Config.PlayerCount];

        for (int i = 0; i < Config.PlayerCount; i++)
        {
            BuildingsPerPlayer[i] = new List<BuildingType>();
            UnitsPerPlayer[i] = new List<UnitType>();
        }
    }

    public static int SoldierCount(int team)
    {
        if (team < 0 || team >= _instance.Config.PlayerCount)
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
        if (team < 0 || team >= _instance.Config.PlayerCount)
            return -1;
        int count = 0;
        foreach (UnitType type in UnitsPerPlayer[team])
        {
            if (type == UnitType.LightTank || type == UnitType.HeavyTank)
                count++;
        }
        return count;
    }

    public static Building[] AllBuildings()
    {
        List<Building> buildings = new List<Building>();
        for (int i = 0; i < _instance.BuildingHolder.transform.childCount; i++)
        {
            buildings.Add(_instance.BuildingHolder.transform.GetChild(i).GetComponent<Building>());
        }
        return buildings.ToArray();
    }

    public static Unit[] UnitsForTeam(int team)
    {
        List<Unit> units = new List<Unit>();
        for (int i = 0; i < _instance.UnitHolder.transform.childCount; i++)
        {
            int iteam = _instance.UnitHolder.transform.GetChild(i).GetComponent<Owned>().Team;
            if (iteam == team)
            {
                units.Add(_instance.UnitHolder.transform.GetChild(i).GetComponent<Unit>());
            }
        }
        return units.ToArray();
    }
}
