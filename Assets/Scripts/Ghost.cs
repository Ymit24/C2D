using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
    GameObject healthbar;

    void Awake() {
        var hb = GetComponentInChildren<Healthbar>();
        if (hb != null)
        {
            healthbar = hb.gameObject;
        }

        SetComponents(false);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0.2f;
            sr.color = c;
        }
	}

    void OnDestroy()
    {
        SetComponents(true);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 1;
            sr.color = c;
        }
    }

    void SetComponents(bool active)
    {
        Health h = GetComponent<Health>();
        Attack a = GetComponent<Attack>();
        Selectability s = GetComponent<Selectability>();
        BoxCollider2D bc2d = GetComponent<BoxCollider2D>();
        CircleCollider2D cc2d = GetComponent<CircleCollider2D>();

        if (h != null)
        {
            h.enabled = active;
        }
        if (a != null)
        {
            a.enabled = active;
        }
        if (s != null)
        {
            s.enabled = active;
        }
        if (bc2d != null)
        {
            bc2d.enabled = active;
        }
        if (cc2d != null)
        {
            cc2d.enabled = active;
        }

        if (healthbar != null)
        {
            healthbar.SetActive(active);
        }
    }
}
