using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

public class MouseController : MonoBehaviour {
	private void MouseClick(int button)
	{
	    if (Input.GetMouseButtonDown(button))
		{
			EventSystem.Global.FireEvent(new MouseUpdateEventInfo((MouseButton)button, MouseState.DOWN, CalculateMousePosition()));
	    }
	    if (Input.GetMouseButtonUp(button))
	    {
			EventSystem.Global.FireEvent(new MouseUpdateEventInfo((MouseButton)button, MouseState.UP, CalculateMousePosition()));
	    }
	    if (Input.GetMouseButton(button))
	    {
			EventSystem.Global.FireEvent(new MouseUpdateEventInfo((MouseButton)button, MouseState.HELD, CalculateMousePosition()));
	    }
	}

	private void Update () {

	    MouseClick(0);
	    MouseClick(1);
	    MouseClick(2);

	    if (Mathf.Abs(Input.mouseScrollDelta.y) > 0.2f)
	    {
			EventSystem.Global.FireEvent(new MouseScrollEventInfo(-Input.mouseScrollDelta.y));
	    }
	}

    public static Vector3 CalculateMousePosition()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0; // just incase we got a weird Z value (like -10 or something)
        return p;
    }
}
