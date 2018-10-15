using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

public class Collisions : EventComponent {
    private void OnCollisionEnter2D(Collision2D collision)
    {
		localEventSystem.FireEvent(new OnCollisionEventInfo(collision, CollisionState.ENTER));
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
		localEventSystem.FireEvent(new OnCollisionEventInfo(collision, CollisionState.STAY));
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        localEventSystem.FireEvent(new OnCollisionEventInfo(collider, CollisionState.ENTER));
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
		localEventSystem.FireEvent(new OnCollisionEventInfo(collider, CollisionState.STAY));
    }
}