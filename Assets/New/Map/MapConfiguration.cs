using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConfiguration : MonoBehaviour {
    public GameObject[] mapPrefabs;
    public int[] maxPlayers;
    public int[] spawnPoints;

    public int SelectedMap;
    public int PlayerCount;
    public int SpawnPoint;

	protected void Awake() {
        DontDestroyOnLoad(gameObject);
	}
}
