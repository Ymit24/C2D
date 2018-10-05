using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Unit : MonoBehaviour {
    public UnitData Configuration;
    private Attack AttackComp;
    private Owned owned;
    private AIPath Ai;

	public void Setup (UnitData Config) {
        Configuration = Config;
        // setup unit gameobject from configuration
        // Movement
        MoveSpeed = Configuration.MoveSpeed;
        // Attack
        AttackComp = gameObject.AddComponent<Attack>();
        AttackComp.TargetingRange = Configuration.Range;
        AttackComp.Damage = Configuration.Damage;
        AttackComp.AttacksPerSecond = Configuration.AttacksPerSecond;
        AttackComp.Setup();
        //Collision
        bc2d = gameObject.AddComponent<BoxCollider2D>();
        bc2d.size = Configuration.BoxColliderSize;
        //Physics
        rig2d = gameObject.AddComponent<Rigidbody2D>();
        rig2d.gravityScale = 0;
        rig2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        rig2d.simulated = true;
        rig2d.bodyType = RigidbodyType2D.Dynamic;

        // Selectability
        graphic = new GameObject("SelectedGraphic");
        graphic.transform.SetParent(transform);
        graphic.transform.localPosition = Vector2.zero;
        graphic.tag = Tags.SELECTED_GRAPHIC;

        SpriteRenderer sg_spr = graphic.AddComponent<SpriteRenderer>();
        sg_spr.sprite = Resources.Load<Sprite>("SelectionBox");
        sg_spr.drawMode = SpriteDrawMode.Sliced;
        sg_spr.size = Configuration.SelectedGraphicSize;

        SelectionBox.OnClearSelected += OnClearSelected;
        IsSelected = false;

        // Graphics
        SpriteRenderer spr = gameObject.AddComponent<SpriteRenderer>();
        spr.sprite = Configuration.Graphic;

        // add owned component
        owned = gameObject.AddComponent<Owned>();

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

        Begin();
    }

    public void SetTeam(int team)
    {
        if (owned != null)
        {
            owned.Team = team;
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

        if (team != 0)
        {
            // Add pathfinding components
            Ai = gameObject.AddComponent<AIPath>();
            Ai.pickNextWaypointDist = 2f;
            Ai.rotationIn2D = true;
            Ai.gravity = Vector3.zero;
            Ai.updateRotation = false;
            Ai.constrainInsideGraph = true;
            Ai.maxSpeed = Configuration.MoveSpeed;
            //rig2d.constraints = RigidbodyConstraints2D.None;
        }
    }

    private Vector3 moveTarget;

    private float MoveSpeed = 3f;

    private bool _selected;
    private bool IsSelected
    {
        get
        {
            return _selected;
        }
        set
        {
            _selected = value;
            if (graphic != null)
            {
                graphic.SetActive(value);
            }
        }
    }

    private int deselect; // this is so we don't deselect before another script can handle selection on the same frame
                          // e.g. soldier needing to set move target when we click but we unselect first.

    private BoxCollider2D bc2d;
    private Rigidbody2D rig2d;
    private GameObject graphic;

    void Begin()
    {
        // MOVEMENT
        moveTarget = transform.position;
        MouseController.MouseClickDownListeners += OnMouseClickDown;
    }

    public void OnMouseClickDown(int button, Vector2 position)
    {
        if (button != 0) return;

        if (IsSelected)
        {
            moveTarget = MouseController.calculateMousePosition();
            moveTarget.z = 0;
        }
    }

    public void SetMoveTarget(Vector3 target)
    {
        if (owned.Team == 0)
        {
            moveTarget = target;
        }
        else
        {
            Ai.destination = target;
        }
    }

    void Update()
    {
        // Movement
        if (owned.Team == 0)
        {
            if (Vector3.Distance(transform.position, moveTarget) > 0.75f)
            {
                Vector3 move = (moveTarget - transform.position).normalized * MoveSpeed * Time.deltaTime;
                rig2d.MovePosition(transform.position + move);
            }
            else
            {
                moveTarget = transform.position;
                rig2d.velocity = Vector2.zero;
            }
        }

        // Selectability
        if (deselect == 1)
        {
            deselect++;
        }
        else if (deselect == 2)
        {
            deselect = 0;
            IsSelected = false;
        }
    }

    public void OnClearSelected()
    {
        deselect = 1;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!bc2d.IsTouching(col))
        {
            return;
        }
        if (col.CompareTag(Tags.SELECTION_BOX))
        {
            if (owned.Team != 0)
                return;
            IsSelected = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(Tags.SELECTION_BOX))
        {
            IsSelected = false;
        }
    }
}
