using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

[RequireComponent(typeof(BoxCollider2D))]
public class Selectability : GameComponent {

    private GameObject graphic;
	private BoxCollider2D bc2d;

    private bool _selected;
	public bool IsSelected
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

	private EventListener SelectCheckListener;

	public override void Setup(GameObjectData Config)
	{
		bc2d = GetComponent<BoxCollider2D>();
		if (bc2d == null)
		{
			Debug.LogWarning("Selectability Setup: BoxCollider2D not found");
		}

		graphic = new GameObject("SelectedGraphic");
		graphic.transform.SetParent(transform);
		graphic.transform.localPosition = Vector2.zero;
		graphic.tag = TAGS.SelectedGraphic.ToString();

		SpriteRenderer sg_spr = graphic.AddComponent<SpriteRenderer>();
		sg_spr.sprite = Resources.Load<Sprite>("SelectionBox");
		sg_spr.drawMode = SpriteDrawMode.Sliced;
		sg_spr.size = ((UnitData)Config).SelectedGraphicSize;
		IsSelected = false;

		SelectCheckListener = EventSystem.Global.RegisterListener<SelectEventInfo>(OnSelectCheck);
	}

	private void OnDestroy()
	{
		EventSystem.Global.UnregisterListener<SelectEventInfo>(SelectCheckListener);
	}

	private void OnSelectCheck(SelectEventInfo info)
	{
		if (Owner.Team != 0) return;
		IsSelected = info.Bounds.Intersects(bc2d.bounds);
	}
}
