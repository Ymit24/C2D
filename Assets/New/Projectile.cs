using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public Vector3 Target;
    private Vector3 Direction;
    public float Speed = 6f;
    public float Damage = 1f;
	public float Distance = 2f;
    public int Team; // To avoid friendly fire
	private float traveled = 0f;

    private void Start()
    {
        Direction = (Target - transform.position).normalized;
    }
	private void Update()
	{
		// Basic projectile moves toward target in straight line.
		// If it hits a collider, the unit or building should take
		// damage and destroy the projectile. If it hits a wall/boundry
		// it should destroy itself
		Vector3 oldPos = transform.position;
		transform.position += Direction * Speed * Time.deltaTime;
		traveled += Vector3.Distance(transform.position, oldPos);
		if (traveled >= Distance)
		{
			Destroy(gameObject);
		}
	}
    public void OnCollisionEnter2D(Collision2D col)
    {
        // This might cause projectiles to destroy themselves before units/buildings
        // can respond and take damage.
        if (Tags.Compare(col.transform, TAGS.Unplaceable))
        {
            Destroy(gameObject);
        }
    }
}
