using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
    public delegate void OnMouseClickDown(int button, Vector2 position);
    public delegate void OnMouseClickUp(int button, Vector2 position);
    public delegate void WhileMouseClickDown(int button, Vector2 position);
    public delegate void WhileMouseClickUp(int button, Vector2 position);
    public delegate void OnScroll(float delta);

    public static event OnMouseClickDown MouseClickDownListeners;
    public static event OnMouseClickUp MouseClickUpListeners;
    public static event WhileMouseClickDown WhileMouseClickDownListeners;
    public static event WhileMouseClickUp WhileMouseClickUpListeners;
    public static event OnScroll ScrollListener;
    
    void MouseClick(int button)
    {
        if (Input.GetMouseButtonDown(button))
        {
            if (MouseClickDownListeners != null)
            {
                MouseClickDownListeners.Invoke(button, calculateMousePosition());
            }
        }
        if (Input.GetMouseButtonUp(button))
        {
            if (MouseClickUpListeners != null)
            {
                MouseClickUpListeners.Invoke(button, calculateMousePosition());
            }
        }
        if (Input.GetMouseButton(button))
        {
            if (WhileMouseClickDownListeners != null)
            {
                WhileMouseClickDownListeners.Invoke(button, calculateMousePosition());
            }
        }
    }

    void Update () {

        MouseClick(0);
        MouseClick(1);
        MouseClick(2);

        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0.2f)
        {
            ScrollListener(-Input.mouseScrollDelta.y);
        }
    }

    public static Vector3 calculateMousePosition()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0; // just incase we got a weird Z value (like -10 or something)
        return p;
    }
}
