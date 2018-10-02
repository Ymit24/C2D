using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour {

    public GameObject graphic;
    bool displaying;

    SpriteRenderer gr;
    BoxCollider2D bc;

    public static event Action OnClearSelected;

    Vector3 start;

    void Start()
    {
        gr = graphic.GetComponent<SpriteRenderer>();
        bc = graphic.GetComponent<BoxCollider2D>();

        MouseController.MouseClickDownListeners += OnMouseClickDown;
        MouseController.MouseClickUpListeners += OnMouseClickUp;
    }

    public void OnMouseClickDown(int button, Vector2 position)
    {
        if (button != 0) return;
        if (OnClearSelected != null)
        {
            OnClearSelected();
        }

        start = position;
        displaying = true;
    }

    public void OnMouseClickUp(int button, Vector2 position)
    {
        if (button != 0) return;
        displaying = false;
        start = Vector3.zero;
    }

	void Update () {
        Vector3 current = MouseController.calculateMousePosition();
        Vector3 center = (current + start) / 2;

        float sx = Mathf.Min(current.x, start.x);
        float ex = Mathf.Max(current.x, start.x);
        float sy = Mathf.Min(current.y, start.y);
        float ey = Mathf.Max(current.y, start.y);

        Vector3 half = new Vector3(ex-sx, ey-sy, 0.5f);

        graphic.transform.position = center;

        gr.size = half;
        bc.size = half;

        graphic.SetActive(displaying);
    }
}
