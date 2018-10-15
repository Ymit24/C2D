using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Requirements : MonoBehaviour {
	public static bool CanFitBuildingAt(BoxCollider2D bc2d, Vector3 location, BuildingData type)
	{
		BoxCollider2D[] colliders = new BoxCollider2D[64];
		int ColliderCount = bc2d.OverlapCollider(new ContactFilter2D() { }, colliders);
		if (ColliderCount != 0)
		{
			for (int i = 0; i < ColliderCount; i++)
			{
                if (!colliders[i] is BoxCollider2D)
                    continue; // we dont want to collide with an attack range, because that has no physical presence
				if (colliders[i].gameObject == bc2d.gameObject) continue; // just incase we collide with our own colliders somehow
				// if the ghost building is colliding with something
				// like a wall or another building then bail.
                if (Tags.Compare(colliders[i].transform, TAGS.Unplaceable)) return false;
			}
		}
		return true;
	}
	public static bool CanBuildUnit(int money, UnitData type)
	{
		return money >= type.Cost;
	}
}
