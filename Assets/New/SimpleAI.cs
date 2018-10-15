using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour {
    public int Team;
    public bool HasTankFactory;
    public int Strength;

    public GameObject CommandCenter;

    private Timer actionTimer;

    public int State = 0;
    public int Substate1 = 0;
    public int Substate2 = 0;

    public bool MovingToCrystal = false;
    public Crystal Target;

    public Building Barracks;

    protected void Start()
    {
        actionTimer = new Timer(2);
    }

    protected void Update()
    {
        if (actionTimer.tick(Time.deltaTime))
        {
            actionTimer.Reset();

            switch (State)
            {
                case 0: // Move units to nearest crystal that is available
                    if (MovingToCrystal == false)
                    {
                        Target = FindNearestCrystal();
                        MoveAllUnitsTo(Target.transform.position);
                        MovingToCrystal = true;
                    }
                    else
                    {
                        if (AnyUnitInRange(Target.transform.position))
                        {
                            MovingToCrystal = false;
                            State++;
                        }
                    }
                    break;
                case 1: // Build a power plant there, if substate < 2 state=0 else state=2
                    if (PlayerController.Data(Team).Gold >= 100) // TODO: Don't hardcode this cost for power plant
                    {
                        PlaceUntilWorked(BuildingType.PowerPlant, Target.transform.position, 1.5f);
                        Target.TeamsWhoHaveAPowerPlantHere.Add(Team);
                        Target = null;
                        PlayerController.Data(Team).Gold -= 100; // TODO: Same here, don't hardcode
                        Substate1++;
                        if (Substate1 < 4)
                            State = 0;
                        else
                            State++;
                    }
                    break;
                case 2: // build barracks and move units to command center
                    if (Barracks != null) State++;
                    if (PlayerController.Data(Team).Gold >= 150) // TODO: STOP HARDCODING
                    {
                        PlaceUntilWorked(BuildingType.Barracks, CommandCenter.transform.position, 3);
                        PlayerController.Data(Team).Gold -= 100; // TODO: Same here, don't hardcode
                        MoveAllUnitsTo(CommandCenter.transform.position);
                        State ++;
                    }
                    break;
                case 3: // build up to 5 total units
                    if (MapController.UnitsForTeam(Team).Length >= 5) { State++; break; }
                    if (PlayerController.Data(Team).Gold >= 50) // TODO: DONT HARDCODE
                    {
                        UnitCreatorController.PlaceUnit(UnitType.LightSoldier, Barracks.transform.position + SpawnCircle(1.5f), Team);
                        PlayerController.Data(Team).Gold -= 50;
                    }
                    break;
                case 4: // repeat states 0,1 until substate > 5
                    break;
                case 5: // attack nearest enemy powerplant, if strength/unitcount > 5 repeat, else state=3
                    break;
            }

//            Unit[] units = MapController.UnitsForTeam(Team);
//            for (int i = 0; i < units.Length; i++)
//            {
//                Crystal c = Utils.CrystalInRange(units[i].transform.position);
//                if (c != null && c.TeamsWhoHaveAPowerPlantHere.Contains(Team) == false)
//                {
//                    BuildController.PlaceBuilding(BuildingType.PowerPlant, units[i].transform.position, Team);
//                    c.TeamsWhoHaveAPowerPlantHere.Add(Team);
//                }
//            }
//
//            // Find nearest available crystal
//            GameObject map = MapController.Map;
//            Transform crystalHolder = Utils.findChild(map.transform, Tags.CRYSTALS);
//            if (crystalHolder != map.transform)
//            {
//                Transform[] crystals = Utils.findChildren(crystalHolder, Tags.CRYSTAL);
//                Transform[] orderedCrystals = Utils.ClosestOrdered(crystals, CommandCenter.transform.position);
//                for (int i = 0; i < orderedCrystals.Length; i++)
//                {
//                    Crystal c = orderedCrystals[i].GetComponent<Crystal>();
//                    if (c.TeamsWhoHaveAPowerPlantHere.Contains(Team))
//                        continue;
//                    MoveAllUnitsTo(orderedCrystals[i].position);
//                    break;
//                }
//            }
            // if there is one, move all units to it
            // once there build a powerplant whenver the ai can afford it
            // repeat
        }
    }

    private void PlaceUntilWorked(BuildingType type, Vector3 origin, float range)
    {
        bool worked = false;
        int cycles = 500;
        while (worked == false && cycles > 0)
        {
            cycles--;
            Vector3 p = Random.onUnitSphere * range;
            p.z = 0;
            Barracks = BuildController.PlaceBuilding(type, origin + p, Team, out worked);
        }
    }

    private Vector3 SpawnCircle(float range)
    {
        Vector3 p = Random.onUnitSphere;
        p.z = 0;
        return p * range;
    }

    private Crystal FindNearestCrystal()
    {
        GameObject map = MapController.Map;
        Transform crystalHolder = Utils.findChild(map.transform, TAGS.Crystals);
        Crystal closest = null;
        if (crystalHolder != map.transform)
        {
            Transform[] crystals = Utils.findChildren(crystalHolder, TAGS.Crystal);
            float distance = Mathf.Infinity;
            for (int i = 0; i < crystals.Length; i++)
            {
                float d = Vector3.Distance(CommandCenter.transform.position, crystals[i].position);
                if (d < distance && crystals[i].GetComponent<Crystal>().TeamsWhoHaveAPowerPlantHere.Contains(Team) == false)
                {
                    distance = d;
                    closest = crystals[i].GetComponent<Crystal>();
                }
            }
        }
        return closest;
    }

    private bool AnyUnitInRange(Vector3 point)
    {
        Unit[] units = MapController.UnitsForTeam(Team);
        for (int i = 0; i < units.Length; i++)
        {
            if (Vector3.Distance(units[i].transform.position, point) <= 2) // TODO: don't hardcode range
            {
                return true;
            }
        }
        return false;
    }

    private void MoveAllUnitsTo(Vector3 point)
    {
        Unit[] units = MapController.UnitsForTeam(Team);
        for (int i = 0; i < units.Length; i++)
        {
			Movement move = units[i].GetComponent<Movement>();
			if (move != null)
			{
				move.SetMoveTarget(point);
			}
        }
    }
}
