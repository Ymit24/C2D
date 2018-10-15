using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

public class BuildController : MonoBehaviour {

    private int _buildType; // what kind of building we're building (e.g. CommandCenter, Barracks, etc...)
    private int _buildState; // 0 -- Off, 1 -- Selecting type, 2 -- Ghost, 3 -- place & reset to 0

	public List<BuildingData> Buildings;
    public GameObject GhostBuilding;

    public GameObject BuildPanel;
    public GameObject BuildingHolder;
    private static BuildController _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        updateBuildState(0);
    }

    public void SetBuildType(int type)
    {
        _buildType = type;

        updateBuildState(2);
    }

    private void updateBuildState(int state)
    {
        _buildState = state;
        switch (_buildState)
        {
            case 0:
                BuildPanel.SetActive(false);
                break;
            case 1:
                BuildPanel.SetActive(true);
                break;
            case 2:
                if (GhostBuilding != null)
                {
                    Destroy(GhostBuilding);
                }
                GhostBuilding = new GameObject(BuildingData.NameFromType(Buildings[_buildType].Type));
                GhostBuilding.transform.SetParent(BuildingHolder.transform);
                GhostBuilding.tag = TAGS.Unplaceable.ToString();
                GhostBuilding.AddComponent<Building>().StartGhostMode(Buildings[_buildType]);
	                break;
        }
    }

    private void Update()
    {
        switch (_buildState)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.X))
                {
                    updateBuildState(1);
                }
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    updateBuildState(0);
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Z) && GhostBuilding != null)
                {
                    Destroy(GhostBuilding);
                    updateBuildState(0);
                    break;
                }
                Vector2 pos = MouseController.CalculateMousePosition();
                GhostBuilding.transform.position = pos;
                if (Input.GetMouseButtonDown(0))
                { /*  CONFIRM BUILD AND PLACE BUILDING  */
                    // somehow check to see if this is a valid location
                    if (PlaceBuildingIfValid(GhostBuilding, 0))
                    {
                        GhostBuilding = null;
                    }
                }
                break;
        }
    }

    public static Building PlaceBuilding(BuildingType type, Vector3 location, int owner)
    {
        bool _;
        return PlaceBuilding(type, location, owner, out _);
    }

    public static Building PlaceBuilding(BuildingType type, Vector3 location, int owner, out bool success)
    {
        GameObject building = new GameObject(BuildingData.NameFromType(type));
        building.transform.SetParent(_instance.BuildingHolder.transform);
        building.tag = TAGS.Unplaceable.ToString();
        building.layer = LayerMask.NameToLayer(TAGS.Unplaceable.ToString());
        building.AddComponent<Building>().StartGhostMode(_instance.Buildings[(int)type]);
        building.transform.position = location;
        success = _instance.PlaceBuildingIfValid(building, owner);
        if (success)
        {
            MapController.Pathfinder.Scan();
        }
        else
        {
            Destroy(building);
            return null;
        }
        return building.GetComponent<Building>();
    }

    private bool PlaceBuildingIfValid(GameObject building, int team)
    {
		BoxCollider2D bc2d = building.GetComponent<BoxCollider2D> ();
		if (bc2d != null) {
			bc2d.enabled = true;
			if (!Requirements.CanFitBuildingAt (bc2d, building.transform.position, Buildings [_buildType]))
				return false;
			bc2d.enabled = false;
		}

        // Do some kind of testing for building requirements,
        // e.g. PowerPlant requires an unused (by this owner) crystal to be close and
        // a friendly unit to be close
        // ALL buildings (besides PP) require a friendly building to be near
        Collider2D[] cols = Physics2D.OverlapCircleAll(building.transform.position, 1.5f);
        bool isNearFriendlyBuilding = false;
		bool isNearFriendlyUnit = false;
		bool isNearCrystal = false;
		bool nearCrystalHasAFreeSpot = false;
		Crystal crystal = null;
        for (int i = 0; i < cols.Length; i++)
        {
            if (!cols[i] is BoxCollider2D)
                continue; // we dont want to collide with an attack range, because that has no physical presence
            Owned o = cols[i].GetComponent<Owned>();
            Building b = cols[i].GetComponent<Building>();
			Unit u = cols[i].GetComponent<Unit>();
			if (!isNearCrystal && !nearCrystalHasAFreeSpot) {
				crystal = cols[i].GetComponent<Crystal>();
				if (crystal != null && !crystal.TeamsWhoHaveAPowerPlantHere.Contains (team))
					nearCrystalHasAFreeSpot = true;
			}
            if (Tags.Compare(cols[i].transform, TAGS.Crystal)) isNearCrystal = true;
			if (u != null && o != null && o.Team == team) { isNearFriendlyUnit = true; }
            if (o != null && o.Team == team && b != null)
            {
                isNearFriendlyBuilding = true;
            }
        }
        Building GB = building.GetComponent<Building>();
        bool isCC = GB != null && GB.Configuration.Type == BuildingType.CommandCenter;
		bool isPP = GB != null && GB.Configuration.Type == BuildingType.PowerPlant;
        if (!isCC)
        {
			if (isPP && (!isNearFriendlyUnit || !isNearCrystal || !nearCrystalHasAFreeSpot)) {
				// look for a friendly unit and crystall
				return false;
			}

			if (isNearFriendlyBuilding == false && isPP == false) return false; // Unless we are building a power plant
        }
		if (PlayerController.Data (team).Gold < Buildings [_buildType].Cost)
		{
			return false;
		}
		else
		{
			PlayerController.Data (team).Gold -= Buildings [_buildType].Cost;
			if (isPP & isNearCrystal && nearCrystalHasAFreeSpot) {
				if (crystal != null)
					crystal.TeamsWhoHaveAPowerPlantHere.Add (team);
				PlayerController.Data (team).Number_of_power_plants++;
			}
        }
        bc2d.enabled = true;
        Building ghostbuilding = building.GetComponent<Building>();
        ghostbuilding.EndGhostMode();
        ghostbuilding.SetTeam(team);
        updateBuildState(0);
        MapController.BuildingsPerPlayer[team].Add(ghostbuilding.Configuration.Type);
        if (team == 0)
        {
			EventSystem.Global.FireEvent(new UIUpdateEventInfo(UiUpdateType.BUILDING_TEXT, MapController.BuildingsPerPlayer[team].Count));
        }
        return true;
    }
}
