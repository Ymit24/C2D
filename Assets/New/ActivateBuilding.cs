using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBuilding : MonoBehaviour {
    public int t;
	void Start () {
        Building b = GetComponent<Building>();
        b.StartGhostMode(b.Configuration);
        b.EndGhostMode();
        b.SetTeam(t);
        GetComponent<BoxCollider2D>().enabled = true;
        Destroy(this);
	}
}
