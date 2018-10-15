using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using C2D.Event;

public class Movement : GameComponent
{
	private float MoveSpeed;
	private Vector3 moveTarget;
	private Rigidbody2D rig2d;
	private Selectability selectability;
	private AIPath ai;
	private EventListener MouseUpdateEvenListener;
	public override void Setup(GameObjectData Config)
	{
		selectability = GetComponent<Selectability>();
		if (selectability == null)
		{
			Debug.LogWarning("Movement Setup: Seletability not found!");
		}

		rig2d = GetComponent<Rigidbody2D>();
		if (rig2d == null)
		{
			Debug.LogWarning("Movement Setup: Rigidbody2D not found!");
		}

		MoveSpeed = ((UnitData)Config).MoveSpeed;
		moveTarget = transform.position;

		MouseUpdateEvenListener = EventSystem.Global.RegisterListener<MouseUpdateEventInfo>(OnMouseUpdate);

		if (Owner.Team != 0)
		{
			ai = gameObject.AddComponent<AIPath>();
			ai.pickNextWaypointDist = 2f;
			ai.rotationIn2D = true;
			ai.gravity = Vector3.zero;
			ai.updateRotation = false;
			ai.constrainInsideGraph = true;
			ai.maxSpeed = ((UnitData)Config).MoveSpeed;
		}
	}
	private void OnDestroy()
	{
		EventSystem.Global.UnregisterListener<MouseUpdateEventInfo>(MouseUpdateEvenListener);
	}
	private void OnMouseUpdate(MouseUpdateEventInfo info)
	{
		if (info.Button != MouseButton.LEFT || info.State != MouseState.DOWN) return;
		if (selectability.IsSelected)
		{
			moveTarget = info.Position;
			moveTarget.z = 0;
		}
	}
	private void Update()
	{
		if (Owner.Team == 0)
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
	}
	public void SetMoveTarget(Vector3 target)
	{
		if (Owner.Team == 0)
		{
			moveTarget = target;
		}
		else
		{
			ai.destination = target;
		}
	}
}