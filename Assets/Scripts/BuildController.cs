using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour {

    private int _buildType; // what kind of building we're building (e.g. CommandCenter, Barracks, etc...)
    private int _buildState; // 0 -- Off, 1 -- Selecting type, 2 -- Ghost, 3 -- place & reset to 0

	public List<BuildingData> Buildings;
    public GameObject GhostBuilding;

    public GameObject BuildPanel;

    private void Start()
    {
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
					Destroy (GhostBuilding);
				}
				GhostBuilding = Instantiate(Buildings[_buildType].prefab);
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
                Vector2 pos = MouseController.calculateMousePosition();
                GhostBuilding.transform.position = pos;
                if (Input.GetMouseButtonDown(0))
                { /*  CONFIRM BUILD AND PLACE BUILDING  */
                    // somehow check to see if this is a valid location
                    PlaceBuildingIfValid();
                    
                }
                break;
        }
    }

    private void PlaceBuildingIfValid()
    {
		BoxCollider2D bc2d = GhostBuilding.GetComponent<BoxCollider2D> ();
		if (bc2d != null) {
			bc2d.enabled = true;
			if (!Requirements.CanFitBuildingAt (bc2d, GhostBuilding.transform.position, Buildings [_buildType]))
				return;
			bc2d.enabled = false;
		}

        // Do some kind of testing for building requirements,
        // e.g. PowerPlant requires an unused (by this owner) crystal to be close and
        // a friendly unit to be close
        // ALL buildings (besides PP) require a friendly building to be near
        Collider2D[] cols = Physics2D.OverlapCircleAll(GhostBuilding.transform.position, 2);
        bool isNearFriendlyBuilding = false;
		bool isNearFriendlyUnit = false;
		bool isNearCrystal = false;
		bool nearCrystalHasAFreeSpot = false;
		Crystal crystal = null;
        for (int i = 0; i < cols.Length; i++)
        {
            Owned o = cols[i].GetComponent<Owned>();
            Building b = cols[i].GetComponent<Building>();
			Unit u = cols[i].GetComponent<Unit>();
			if (!isNearCrystal && !nearCrystalHasAFreeSpot) {
				crystal = cols[i].GetComponent<Crystal>();
				if (crystal != null && !crystal.TeamsWhoHaveAPowerPlantHere.Contains (0))
					nearCrystalHasAFreeSpot = true;
			}
			if (cols[i].CompareTag("Crystal")) isNearCrystal = true;
			if (u != null && o != null && o.Team == 0) { isNearFriendlyUnit = true; }
            if (o != null && o.Team == 0 && b != null) // Hardcode 0 to be friendly, will be fixed soon
            {
                isNearFriendlyBuilding = true;
            }
        }
        Building GB = GhostBuilding.GetComponent<Building>();
        bool isCC = GB != null && GB.Type == BuildingType.CommandCenter;
		bool isPP = GB != null && GB.Type == BuildingType.PowerPlant;
        if (!isCC)
        {
			if (isPP && (!isNearFriendlyUnit || !isNearCrystal || !nearCrystalHasAFreeSpot)) {
				// look for a friendly unit and crystall
				return;
			}

			if (isNearFriendlyBuilding == false && isPP == false) return; // Unless we are building a power plant
        }
		if (PlayerController.Data (0).money <= Buildings [_buildType].cost)
		{
			return;
		}
		else
		{
			PlayerController.Data (0).money -= Buildings [_buildType].cost;
			if (isPP & isNearCrystal && nearCrystalHasAFreeSpot) {
				if (crystal != null)
					crystal.TeamsWhoHaveAPowerPlantHere.Add (0);
				PlayerController.Data (0).number_of_power_plants++;
			}
		}
        Ghost g = GhostBuilding.GetComponent<Ghost>();
        if (g != null)
        {
            Destroy(g);
        }
        GhostBuilding = null;
        updateBuildState(0);
    }
}
