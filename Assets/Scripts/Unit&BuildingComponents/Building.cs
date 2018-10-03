using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData Configuration;
    private Attack AttackComp;

    public void Setup(BuildingData Config)
    {
        Configuration = Config;
        // Attack
        if (Configuration.CanAttack)
        {
            AttackComp = gameObject.AddComponent<Attack>();
            AttackComp.TargetingRange = Configuration.Range;
            AttackComp.Damage = Configuration.Damage;
            AttackComp.AttacksPerSecond = Configuration.AttacksPerSecond;
            AttackComp.Setup();
        }
        //Physics
        Rigidbody2D rig2d = gameObject.AddComponent<Rigidbody2D>();
        rig2d.simulated = true;
        rig2d.bodyType = RigidbodyType2D.Static;
        // add owned component
        gameObject.AddComponent<Owned>();
        // Health
        GameObject hb = new GameObject("Healthbar");
        hb.transform.SetParent(transform);
        hb.tag = Tags.HEALTHBAR;
        hb.transform.localPosition = new Vector2(0, Configuration.HealthbarHeight);

        SpriteRenderer hb_spr = hb.AddComponent<SpriteRenderer>();
        hb_spr.sprite = Resources.Load<Sprite>("Healthbar");
        hb_spr.color = Color.red;
        hb_spr.sortingOrder = 1;

        Health h = gameObject.AddComponent<Health>();
        h.MaxHealth = Configuration.MaxHealth;
        h.MaxSize = Configuration.MaxHealthbarWidth;
        h.Setup();

        // UnitFactory
        if (Configuration.CanBuildUnits)
        {
            UnitFactory factory = gameObject.AddComponent<UnitFactory>();
            factory.CanBuild = Configuration.UnitTypes;
        }
        Begin();
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
            o.Team = team;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag(Tags.HEALTHBAR))
            {
                SpriteRenderer spr = transform.GetChild(i).GetComponent<SpriteRenderer>();
                if (spr != null)
                {
                    spr.color = PlayerController.Data(team).color;
                }
            }
        }
    }

    private void Begin()
    {

    }
}
