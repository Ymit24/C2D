using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour {
    private MapConfiguration Config;
    private void Start()
    {
        Config = Transform.FindObjectOfType<MapConfiguration>();
        GameObject map = Instantiate(Config.mapPrefabs[Config.SelectedMap]);
        MapController.Pathfinder = Utils.findChild(map.transform, Tags.ASTAR).GetComponent<AstarPath>();
        MapController.Map = map;
        // Spawn a command center and units for each team
        Transform playerspawns = Utils.findChild(map.transform, Tags.PLAYERSPAWNS);
        int team = 1;

//        GameObject go = new GameObject("SimpleAI");
//        SimpleAI ai = go.AddComponent<SimpleAI>();
//        ai.Team = 1;
        for (int i = 0; i < playerspawns.childCount; i++)
        {
            if (i >= Config.PlayerCount)
                break; // we don't need to process more spawn points then players
            Transform commandcenterspawn = Utils.findChild(playerspawns.GetChild(i), Tags.COMMANDCENTERSPAWN);
            Transform[] unitspawns = Utils.findChildren(playerspawns.GetChild(i), Tags.UNITSPAWN);
            if (i == Config.SpawnPoint)
            {
                Vector3 location = commandcenterspawn.position;
                location.z = -10;
                Camera.main.transform.position = location;
            }
            PlayerController.Data(i).Gold = 1000;
            Building building = BuildController.PlaceBuilding(BuildingType.CommandCenter, commandcenterspawn.position, (i == Config.SpawnPoint) ? 0 : team);
            if (i != Config.SpawnPoint)
            {
                GameObject go = new GameObject("SimpleAI - " + i);
                SimpleAI ai = go.AddComponent<SimpleAI>();
                ai.Team = i;
                ai.CommandCenter = building.gameObject;
            }
            for (int j = 0; j < unitspawns.Length; j++)
            {
                // spawn units
                UnitCreatorController.PlaceUnit(UnitType.LightSoldier, unitspawns[j].position, (i == Config.SpawnPoint) ? 0 : team);
            }
            PlayerController.Data(i).Gold = 300;
            PlayerController.Data(i).Number_of_power_plants = 0;
            if (i != Config.SpawnPoint)
                team++;
        }
    }
}
