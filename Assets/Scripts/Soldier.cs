using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Selectability))]
[RequireComponent(typeof(Rigidbody2D))]
public class Soldier : MonoBehaviour {

    public Vector3 moveTarget;

    Selectability selectability;

    // stats
    public float MoveSpeed = 3f;

    void Start()
    {
        moveTarget = transform.position;

        selectability = GetComponent<Selectability>();

        MouseController.MouseClickDownListeners += OnMouseClickDown;
    }

    public void OnMouseClickDown(int button, Vector2 position)
    {
        if (button != 0) return;

        if (selectability.IsSelected)
        {
            moveTarget = MouseController.calculateMousePosition();
            moveTarget.z = 0;
        }
    }

	void Update () {
        if (Vector3.Distance(transform.position, moveTarget) > 0.5f)
        {
            Vector3 move = (moveTarget - transform.position).normalized * MoveSpeed * Time.deltaTime;
            //transform.Translate(move);
            Rigidbody2D rig2d = GetComponent<Rigidbody2D>();
            rig2d.MovePosition(transform.position + move);
        }
	}
}
