using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour {
    private MapConfiguration Config;
    private void Start()
    {
        Config = Transform.FindObjectOfType<MapConfiguration>();
        GameObject map = Instantiate(Config.mapPrefabs[Config.SelectedMap]);

        // Spawn a command center and units for each team
        Transform playerspawns = findChild(map.transform, Tags.PLAYERSPAWNS);
        int team = 1;
        for (int i = 0; i < playerspawns.childCount; i++)
        {
            if (i >= Config.PlayerCount)
                break; // we don't need to process more spawn points then players
            Transform commandcenterspawn = findChild(playerspawns.GetChild(i), Tags.COMMANDCENTERSPAWN);
            Transform[] unitspawns = findChildren(playerspawns.GetChild(i), Tags.UNITSPAWN);
            if (i == Config.SpawnPoint)
            {
                Vector3 location = commandcenterspawn.position;
                location.z = -10;
                Camera.main.transform.position = location;
            }
            BuildController.PlaceBuilding(BuildingType.CommandCenter, commandcenterspawn.position, (i == Config.SpawnPoint) ? 0 : team);
            for (int j = 0; j < unitspawns.Length; j++)
            {
                // spawn units
                UnitCreatorController.PlaceUnit(UnitType.LightSoldier, unitspawns[j].position, (i == Config.SpawnPoint) ? 0 : team);
            }
            if (i != Config.SpawnPoint)
                team++;
        }
    }

    private Transform findChild(Transform parent, string tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).CompareTag(tag))
            {
                return parent.GetChild(i);
            }
        }
        return parent;
    }

    private Transform[] findChildren(Transform parent, string tag)
    {
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).CompareTag(tag))
            {
                children.Add(parent.GetChild(i));
            }
        }
        return children.ToArray();
    }
}
