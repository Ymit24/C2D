using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

public class Combat : GameComponent {
	public float Range;
    public float Damage;
    public float AttacksPerSecond;
	public bool CombatEnabled;

    public GameObject WhatToFire;

    public Combat Target;

    private Timer attack_timer;

	public override void Setup(GameObjectData Config)
    {
		CombatEnabled = Config.CombatEnabled;
		Range = Config.Range;
		Damage = Config.Damage;
		AttacksPerSecond = Config.AttacksPerSecond;
		WhatToFire = Config.Projectile;

		localEventSystem.RegisterListener<OnCollisionEventInfo>(OnCollision);

        MapController.AddCombatant(this);
        attack_timer = new Timer(1f / AttacksPerSecond);
    }

    private void OnDestroy()
    {
        MapController.RemoveCombatant(this);
    }

	private int GetTeam() { return Owner.Team; }

    public void OnCollision(OnCollisionEventInfo info)
    {
        if (Tags.Compare(info.Collider.transform, TAGS.Projectile))
        {
            Projectile p = info.Collider.GetComponent<Projectile>();
            if (p.Team != GetTeam())
            {
                localEventSystem.FireEvent(new TakeDamageEventInfo(p.Damage));
                Destroy(p.gameObject);
            }
        }
    }

    private bool InRange(Combat combatant)
    {
        return Vector3.Distance(combatant.transform.position, transform.position) <= Range;
    }

    private void FindTarget()
    {
        Combat[] combatants = MapController.GetCombatants();
        for (int i = 0; i < combatants.Length; i++)
        {
            if (combatants[i].GetTeam() != GetTeam() && InRange(combatants[i]))
            {
                Target = combatants[i];
                return;
            }
        }
    }

    private void Update()
    {
		if (CombatEnabled == false) return;
        if (Target == null || !InRange(Target))
        {
            FindTarget();
        }
        else
        {
            if (attack_timer.tick(Time.deltaTime))
            {
                attack_timer.Reset();

				localEventSystem.FireEvent(new FireProjectilEventInfo(GetTeam(), Range, transform.position, Target.transform.position, WhatToFire));
            }
        }
    }
}
