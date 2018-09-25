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
            Vector3 scale = Healthbar.localScale;
            scale.x = Percentage * MaxSize;
            Healthbar.localScale = scale;

            this._value = value;
        }
    }
    public float MaxSize;
    public float Max;
    public float Percentage
    {
        get
        {
            return Value / Max;
        }
    }

    private Transform Healthbar;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Healthbar"))
            {
                Healthbar = transform.GetChild(i);
            }
        }
        _value = Max;
    }
}
