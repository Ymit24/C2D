using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType { CommandCenter, Barracks, TankFactory, PowerPlant}

[RequireComponent(typeof(Owned))]
public class Building : MonoBehaviour {
    public BuildingType Type;
}
