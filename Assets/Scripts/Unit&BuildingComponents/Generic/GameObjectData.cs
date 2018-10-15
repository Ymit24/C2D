using System;
using UnityEngine;

[CreateAssetMenu]
public class GameObjectData : ScriptableObject
{
	[Header("Creation")]
	public string Name;
	public int Cost;
	public Sprite Graphic;

	[Header("Settings")]
	public float HealthbarHeight;
	public Vector2 BoxColliderSize;

	[Header("Health")]
	public float MaxHealth;
	public float MaxHealthbarWidth;

	[Header("Combat")]
	public bool CombatEnabled;
	public float Range;
	public float Damage;
	public float AttacksPerSecond;
	public GameObject Projectile;
}
