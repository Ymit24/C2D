using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace C2D.Event
{
    public class EventInfo {}

    public class TakeDamageEventInfo : EventInfo
    {
        public float Damage;
        public TakeDamageEventInfo(float damage)
        {
            Damage = damage;
        }
    }

	public enum CollisionState { ENTER, STAY, EXIT }
    public class OnCollisionEventInfo : EventInfo
    {
        public Collider2D Collider;
		public CollisionState State;

		public OnCollisionEventInfo(Collision2D collision, CollisionState state)
        {
            this.Collider = collision.collider;
			this.State = state;
        }

        public OnCollisionEventInfo(Collider2D collider, CollisionState state)
        {
            this.Collider = collider;
			this.State = state;
        }
    }

    public class FireProjectilEventInfo : EventInfo
    {

        public int Team;
		public float Distance;
        public Vector3 Source;
        public Vector3 Target;
        public GameObject Prefab;

        public FireProjectilEventInfo(int team, float distance, Vector3 source, Vector3 target, GameObject prefab)
        {
            this.Team = team;
			this.Distance = distance;
            this.Source = source;
            this.Target = target;
            this.Prefab = prefab;
        }
    }

	public enum UiUpdateType { SOLDIER_TEXT, TANK_TEXT, BUILDING_TEXT, GOLD_TEXT, GPM_TEXT }
	public class UIUpdateEventInfo : EventInfo
	{
		public UiUpdateType Type;
		public int new_value;
		public UIUpdateEventInfo(UiUpdateType type, int value)
		{
			Type = type;
			new_value = value;
		}
	}

	public enum MouseButton { LEFT, RIGHT, MIDDLE }
	public enum MouseState { DOWN, HELD, UP }
	public class MouseUpdateEventInfo : EventInfo
	{
		public MouseButton Button;
		public MouseState State;
		public Vector2 Position;
		public MouseUpdateEventInfo(MouseButton button, MouseState state, Vector2 position)
		{
			this.Button = button;
			this.State = state;
			this.Position = position;
		}
	}
	public class MouseScrollEventInfo : EventInfo
	{
		public float DeltaY;
		public MouseScrollEventInfo(float deltaY)
		{
			this.DeltaY = deltaY;
		}
	}
	public class SelectEventInfo : EventInfo
	{
		public Bounds Bounds;
		public SelectEventInfo(Bounds bounds)
		{
			this.Bounds = bounds;
		}
	}
}