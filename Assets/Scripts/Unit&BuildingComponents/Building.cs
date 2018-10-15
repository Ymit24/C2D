using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

public class Building : MonoBehaviour
{
    public BuildingData Configuration;
    private Combat AttackComp;

    public void Setup(BuildingData Config)
    {
        Configuration = Config;
		gameObject.AddComponent<EventSystem>();
        // Attack
        AttackComp = gameObject.AddComponent<Combat>();
		gameObject.AddComponent<ProjectileSpawner>();
        //Physics
        Rigidbody2D rig2d = gameObject.AddComponent<Rigidbody2D>();
        rig2d.simulated = true;
		rig2d.bodyType = RigidbodyType2D.Kinematic;

		// Collisions
		gameObject.AddComponent<Collisions>();

        // add owned component
        gameObject.AddComponent<Owned>();

        // Health
        Health h = gameObject.AddComponent<Health>();

		AttackComp.Setup(Config);
		h.Setup(Config);

        // UnitFactory
        if (Configuration.CanBuildUnits)
        {
            UnitFactory factory = gameObject.AddComponent<UnitFactory>();
            factory.CanBuild = Configuration.UnitTypes;
        }
    }

    private bool isInGhostMode = false;
    public void StartGhostMode(BuildingData Config)
    {
        if (isInGhostMode) return;
        isInGhostMode = true;
        Configuration = Config;
        foreach (Component comp in gameObject.GetComponents<Component>())
        {
            if (!(comp is Transform) && (!comp is Building))
            {
                Destroy(comp);
            }
        }
        // Graphics
        SpriteRenderer spr = gameObject.AddComponent<SpriteRenderer>();
        spr.sprite = Configuration.Graphic;
        //Collision
        BoxCollider2D bc2d = gameObject.AddComponent<BoxCollider2D>();
        bc2d.size = Configuration.BoxColliderSize;
        bc2d.enabled = false;
    }

    public void EndGhostMode()
    {
        if (!isInGhostMode) return;
        isInGhostMode = false;
        Setup(Configuration);
    }

    public void SetTeam(int team)
    {
        Owned o = GetComponent<Owned>();
        if (o != null)
        {
			o.SetTeam(team);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (Tags.Compare(transform.GetChild(i), TAGS.Healthbar))
            {
                SpriteRenderer spr = transform.GetChild(i).GetComponent<SpriteRenderer>();
                if (spr != null)
                {
                    spr.color = PlayerController.Data(team).color;
                }
            }
        }
    }
}
