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
				p.Gold += p.Number_of_power_plants * 10;
            }
			powerPlantTimer.Reset ();
		}
	}

    public static void GoldChanged(PlayerData data)
    {
        if (instance.players.IndexOf(data) != 0) return;
        UIController.OnGoldCountChanged(instance.players[0].Gold);
    }

    public static void GPMChanged(PlayerData data)
    {
        if (instance.players.IndexOf(data) != 0) return;
        UIController.OnGPMCountChanged(instance.players[0].GPM);
    }
}
