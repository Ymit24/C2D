using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

public class ProjectileSpawner : EventComponent {
    protected void Start()
    {
        localEventSystem.RegisterListener<FireProjectilEventInfo>(OnFireProjectile);
    }

    private void OnFireProjectile(FireProjectilEventInfo info)
    {
        GameObject projgo = Instantiate(info.Prefab);

        Vector3 random_circle = Random.onUnitSphere;
        random_circle.z = 0;

        projgo.transform.position = info.Source;

        Projectile projectile = projgo.GetComponent<Projectile>();
        projectile.Target = info.Target;
        projectile.Team = info.Team;
		projectile.Distance = info.Distance;
    }
}
