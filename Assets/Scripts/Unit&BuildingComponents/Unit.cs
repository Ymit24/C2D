using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

public class Unit : MonoBehaviour {
    public UnitData Configuration;
    private Combat CombatComp;
    private Owned owned;

	public void Setup (UnitData Config) {
        Configuration = Config;
        // EventSystem
        gameObject.AddComponent<EventSystem>();

		// add owned component
		owned = gameObject.AddComponent<Owned>();

		// Movement
		Movement move = gameObject.AddComponent<Movement>();

        // Combat
        CombatComp = gameObject.AddComponent<Combat>();

        gameObject.AddComponent<ProjectileSpawner>();
        
		//Collision
        BoxCollider2D bc2d = gameObject.AddComponent<BoxCollider2D>();
        bc2d.size = Configuration.BoxColliderSize;

        gameObject.AddComponent<Collisions>();

        //Physics
        Rigidbody2D rig2d = gameObject.AddComponent<Rigidbody2D>();
        rig2d.gravityScale = 0;
        rig2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        rig2d.simulated = true;
        rig2d.bodyType = RigidbodyType2D.Dynamic;

		// Selectability
		Selectability s = gameObject.AddComponent<Selectability>();

        // Graphics
        SpriteRenderer spr = gameObject.AddComponent<SpriteRenderer>();
        spr.sprite = Configuration.Graphic;

        // Health
        Health h = gameObject.AddComponent<Health>();
    }

    public void SetTeam(int team)
    {
        if (owned != null)
        {
			owned.SetTeam(team);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag(TAGS.Healthbar.ToString()))
            {
                SpriteRenderer spr = transform.GetChild(i).GetComponent<SpriteRenderer>();
                if (spr != null)
                {
                    spr.color = PlayerController.Data(team).color;
                }
            }
        }
		// Setup all components
		GetComponent<Movement>().Setup(Configuration);
		CombatComp.Setup(Configuration);
		GetComponent<Selectability>().Setup(Configuration);
		GetComponent<Health>().Setup(Configuration);
    }
}
