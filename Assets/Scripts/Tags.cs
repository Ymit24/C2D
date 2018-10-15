using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TAGS
{
    SelectionBox, SelectedGraphic, Unplaceable,
    Crystal, Healthbar, PlayerSpawns, UnitSpawn,
    CommandCenterSpawn, Crystals, AStar, Projectile
}

public class Tags {
    public static bool Compare(Transform t, TAGS tag)
    {
        return t.CompareTag(tag.ToString());
    }
}
