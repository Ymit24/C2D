using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float PanSpeed;
    public float ShiftMultiplier = 1.5f;
    public float ZoomSpeed;

    private Vector2 lastPos;

    void Start()
    {
        MouseController.ScrollListener += OnScroll;
    }

    void Update()
    {
        Vector2 vel = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            vel.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            vel.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            vel.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            vel.x += 1;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            vel *= ShiftMultiplier;
        }
        transform.Translate(vel * PanSpeed * Time.deltaTime);
    }
    
    void OnScroll(float delta)
    {
        Zoom(delta);
    }

    void Zoom(float delta)
    {
        float v = delta;
        Camera.main.orthographicSize += v * ZoomSpeed * Time.deltaTime;

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 2, 15);
    }
}
