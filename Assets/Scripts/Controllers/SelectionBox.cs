using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

public class SelectionBox : MonoBehaviour {

    public GameObject graphic;
    bool displaying;

    private SpriteRenderer gr;

    public static event Action OnClearSelected;

    Vector3 start;

    private void Start()
    {
        gr = graphic.GetComponent<SpriteRenderer>();

		EventSystem.Global.RegisterListener<MouseUpdateEventInfo>(OnMouseUpdate);
    }

	private void OnMouseUpdate(MouseUpdateEventInfo info)
	{
		if (info.State == MouseState.UP && info.Button == MouseButton.LEFT)
		{
			// Calculate who is in the selection box
			// and clear the rest
			EventSystem.Global.FireEvent(new SelectEventInfo(calculateBounds(MouseController.CalculateMousePosition())));
			start = Vector2.zero;
			displaying = false;
		}
		if (info.State == MouseState.DOWN && info.Button == MouseButton.LEFT)
		{
			start = info.Position;
			displaying = true;
		}
	}

	private Bounds calculateBounds(Vector2 current)
	{
		float sx = Mathf.Min(current.x, start.x);
		float ex = Mathf.Max(current.x, start.x);
		float sy = Mathf.Min(current.y, start.y);
		float ey = Mathf.Max(current.y, start.y);
		Bounds b = new Bounds();
		b.SetMinMax(new Vector2(sx, sy), new Vector2(ex, ey));
		return b;
	}

	private void Update () {
        Vector3 current = MouseController.CalculateMousePosition();
        Vector3 center = (current + start) / 2;

        float sx = Mathf.Min(current.x, start.x);
        float ex = Mathf.Max(current.x, start.x);
        float sy = Mathf.Min(current.y, start.y);
        float ey = Mathf.Max(current.y, start.y);

        Vector3 half = new Vector3(ex-sx, ey-sy, 0.5f);

        graphic.transform.position = center;

        gr.size = half;

        graphic.SetActive(displaying);
    }
}
