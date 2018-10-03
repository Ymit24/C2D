using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public List<PlayerData> players;
	private static PlayerController instance;

	private Timer powerPlantTimer;

	protected void Awake()
	{
		instance = this;
		powerPlantTimer = new Timer(10);
	}

	public static PlayerData Data(int index)
	{
		if (instance == null)
			return null;
		return instance.players [index];
	}

	protected void Update()
	{
		if (powerPlantTimer.tick (Time.deltaTime))
		{
			foreach (PlayerData p in players)
			{
				p.Money += p.Number_of_power_plants * 10;
            }
			powerPlantTimer.Reset ();
		}
	}
}
