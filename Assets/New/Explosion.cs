using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public float Radius = 1f;
    public float TotalExplosionTime = 0.25f;
    private float CurrentElapsedTime = 0.0f;

	void Update () {
        CurrentElapsedTime += Time.deltaTime;
        if (transform.localScale.x <= Radius)
        {
            Vector3 size = transform.localScale;
            size.x = Radius * (CurrentElapsedTime / TotalExplosionTime);
            size.y = size.x;
            size.z = 1;
            transform.localScale = size;
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
