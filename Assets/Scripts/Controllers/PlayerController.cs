using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;
using System;

public class PlayerData
{
	public int Team;
	public float gold;
	public int power_plants;
	public int gpm
	{
		get
		{
			return power_plants * 10 + 25;
		}
	}
}

public class PlayerController : MonoBehaviour {
	
	public List<PlayerData> players;
	public Color[] PlayerColors;
	public int PaymentsPerMinute = 12;
	public int UIUpdatsPerSecond = 3;

    private static PlayerController instance;

	private Timer powerPlantTimer;
	private Timer uiUpdateTimer;

	protected void Awake()
	{
		instance = this;
		powerPlantTimer = new Timer(60 / PaymentsPerMinute);
		uiUpdateTimer = new Timer (1 / UIUpdatsPerSecond);
	}

    public static Color GetPlayerColor(int player_index)
    {
        AssertValidPlayerIndex(player_index);
        AssertValidInstanceReference();
        return instance.PlayerColors[player_index];
    }

	public static PlayerData GetPlayer(int player_index)
	{
		AssertValidPlayerIndex(player_index);
		AssertValidInstanceReference();
		return instance.players [player_index];
	}

    public static void Setup(int playerCount)
    {
        AssertValidInstanceReference();
        instance.players = new List<PlayerData>();
        for (int i = 0; i < playerCount; i++)
        {
            instance.players.Add(new PlayerData());
        }
    }

	private void Update()
	{
		if (powerPlantTimer.tick (Time.deltaTime))
		{
			powerPlantTimer.Reset ();
			for (int i = 0; i < players.Count; i++)
			{
				players[i].gold += players[i].gpm / PaymentsPerMinute;
            }
		}
		if (uiUpdateTimer.tick (Time.deltaTime))
		{
			uiUpdateTimer.Reset ();
			EventSystem.Global.FireEvent(
				new UIUpdateEventInfo(
					UiUpdateType.GOLD_TEXT, (int) players[0].gold
				)
			);
			EventSystem.Global.FireEvent(
				new UIUpdateEventInfo(
					UiUpdateType.GPM_TEXT, players[0].gpm
				)
			);
		}
	}

    private static void AssertValidPlayerIndex(int index)
    {
        if (index < 0 || index >= instance.players.Count)
        {
            Debug.LogError("Assert FAILED: " + index + " is not a valid player index!!");
        }
    }

    private static void AssertValidInstanceReference()
    {
        if (instance == null)
        {
            Debug.LogError("Assert FAILED: instance is null!");
        }
    }
}
