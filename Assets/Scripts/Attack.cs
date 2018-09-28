using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Attack : MonoBehaviour {
    public float Range;
    public float Damage;
    public float AttacksPerSecond;

    public Health Target;

    private Timer attack_timer;

    private void Start()
    {
        attack_timer = new Timer(1f/AttacksPerSecond);
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
            if (Vector3.Distance(transform.position, Target.transform.position) <= Range)
                return;
        }
        if (!(col2d as BoxCollider2D))
        {
            return;
        }
        Health health = col2d.GetComponent<Health>();
        if (health != null)
        {
            Owned cbo = col2d.GetComponent<Owned>();
            Owned me = GetComponent<Owned>();
            if (cbo != null && me != null)
            {
                if (cbo.Team != me.Team)
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
