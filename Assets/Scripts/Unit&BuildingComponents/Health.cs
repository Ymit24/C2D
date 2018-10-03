using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    private float _value;
    public float Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            Vector3 scale = Healthbar.localScale;
            scale.x = MaxSize * Percentage;
            scale.y = 0.2f;
            Healthbar.localScale = scale;
        }
    }
    public float MaxSize;
    public float MaxHealth;
    public float Percentage
    {
        get
        {
            return Value / MaxHealth;
        }
    }

    private Transform Healthbar;
    public void Setup()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag(Tags.HEALTHBAR))
            {
                Healthbar = transform.GetChild(i);
            }
        }
        Value = MaxHealth;
    }
}
