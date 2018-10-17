using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

public class Health : GameComponent {

    [SerializeField] private float _value;
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
	public override void Setup(GameObjectData Config)
    {
		localEventSystem.RegisterListener<TakeDamageEventInfo>(OnTakeDamage);

		MaxHealth = Config.MaxHealth;
		MaxSize = Config.MaxHealthbarWidth;

		GameObject hb = new GameObject("Healthbar");
		hb.transform.SetParent(transform);
		hb.tag = TAGS.Healthbar.ToString();
		hb.transform.localPosition = new Vector2(0, Config.HealthbarHeight);

		SpriteRenderer hb_spr = hb.AddComponent<SpriteRenderer>();
		hb_spr.sprite = Resources.Load<Sprite>("Healthbar");
		hb_spr.color = PlayerController.GetPlayerColor(Owner.Team);
		hb_spr.sortingOrder = 1;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (Tags.Compare(transform.GetChild(i), TAGS.Healthbar))
            {
                Healthbar = transform.GetChild(i);
            }
        }
        Value = MaxHealth;
    }

    public void OnTakeDamage(TakeDamageEventInfo info)
    {
        Value -= info.Damage;
        if (Value <= 0)
        {
            Destroy(gameObject);
        }
    }
}
