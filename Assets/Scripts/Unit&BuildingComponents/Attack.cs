using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    [HideInInspector] public float TargetingRange;
    [HideInInspector] public float Damage;
    [HideInInspector] public float AttacksPerSecond;

    [HideInInspector] public Health Target;

    private Timer attack_timer;

    public void Setup()
    {
        CircleCollider2D c2d = gameObject.AddComponent<CircleCollider2D>();
        c2d.isTrigger = true;
        c2d.radius = TargetingRange;

        attack_timer = new Timer(1f / AttacksPerSecond);
    }

    public void SetCircleCollider(bool enable)
    {
        GetComponent<CircleCollider2D>().enabled = enable;
    }

    private void Update()
    {
        if (Target == null)
        {
            attack_timer.Reset();
        }

        if (attack_timer.tick(Time.deltaTime))
        {

            attack_timer.Reset();

            Target.Value -= Damage;
            if (Target.Value <= 0)
            {
                attack_timer.Reset();
                Destroy(Target.gameObject);
                Target = null;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D col2d)
    {
        if (Target != null)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) <= TargetingRange)
                return;
        } // at this point either our target isn't set or is out of range
        if (!(col2d as BoxCollider2D))
        {
            return;
        } // we only target the box collider not other ranges
        Health health = col2d.GetComponent<Health>();
        if (health != null)
        {
            Owned them = col2d.GetComponent<Owned>();
            Owned me = GetComponent<Owned>();
            if (them != null && me != null)
            {
                if (them.Team != me.Team)
                {
                    Target = health;
                }
            }
            else
            {
                Target = health;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D col2d)
    {
        if (!(col2d as BoxCollider2D))
        {
            return;
        }
        Health health = col2d.GetComponent<Health>();
        if (health != null)
        {
            if (health.Equals(Target))
            {
                Target = null;
            }
        }
    }
}
