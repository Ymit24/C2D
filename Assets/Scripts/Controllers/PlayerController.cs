using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

public class PlayerController : MonoBehaviour {
	public List<PlayerData> players;
	private static PlayerController instance;

	private Timer powerPlantTimer;

	public int PaymentsPerMinute = 12;

	protected void Awake()
	{
		instance = this;
		powerPlantTimer = new Timer(60 / PaymentsPerMinute);
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
				p.Gold += p.GPM / PaymentsPerMinute;
            }
			powerPlantTimer.Reset ();
		}
	}

    public static void GoldChanged(PlayerData data)
    {
        if (instance.players.IndexOf(data) != 0) return;
		EventSystem.Global.FireEvent(new UIUpdateEventInfo(UiUpdateType.GOLD_TEXT, instance.players[0].Gold));
    }

    public static void GPMChanged(PlayerData data)
    {
        if (instance.players.IndexOf(data) != 0) return;
		EventSystem.Global.FireEvent(new UIUpdateEventInfo(UiUpdateType.GPM_TEXT, instance.players[0].GPM));
    }
}
