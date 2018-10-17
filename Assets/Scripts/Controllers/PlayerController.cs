using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;
using System;

public class PlayerData
{
    public float gold
    {
        get;
        private set;
    }
    public int power_plants;
    public int gpm
    {
        get
        {
            return power_plants * 10 + 25;
        }
    }

    public void SetGold(float amount)
    {
        gold = amount;
    }
}

public class PlayerController : MonoBehaviour {
	public List<PlayerData> players;

    public Color[] PlayerColors;

    private static PlayerController instance;

	private Timer powerPlantTimer;

	public int PaymentsPerMinute = 12;

	protected void Awake()
	{
		instance = this;
		powerPlantTimer = new Timer(60 / PaymentsPerMinute);
	}

    public static Color GetPlayerColor(int player_index)
    {
        AssertValidPlayerIndex(player_index);
        AssertValidInstanceReference();
        return instance.PlayerColors[player_index];
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

    public static void SetGold(int player_index, float goldamount)
    {
        AssertValidPlayerIndex(player_index);
        AssertValidInstanceReference();
        instance.players[player_index].SetGold(goldamount);
        if (player_index == 0)
        {
            EventSystem.Global.FireEvent(
                new UIUpdateEventInfo(
                    UiUpdateType.GOLD_TEXT, (int) goldamount
                )
            );
        }
    }

    public static void AddGold(int player_index, float goldincrement)
    {
        SetGold(player_index, GetPlayer(player_index).gold + goldincrement);
    }

    public static void SetPowerPlantCount(int player_index, int powerplantcount)
    {
        AssertValidPlayerIndex(player_index);
        AssertValidInstanceReference();
        PlayerData player = instance.players[player_index];
        player.power_plants = powerplantcount;
        instance.players[player_index] = player;
        if (player_index == 0)
        {
            EventSystem.Global.FireEvent(
                new UIUpdateEventInfo(
                    UiUpdateType.GPM_TEXT, powerplantcount
                )
            );
        }
    }

	public static PlayerData GetPlayer(int player_index)
    {
        AssertValidPlayerIndex(player_index);
        AssertValidInstanceReference();
		return instance.players [player_index];
	}

	private void Update()
	{
		if (powerPlantTimer.tick (Time.deltaTime))
		{
			for (int i = 0; i < players.Count; i++)
			{
                AddGold(i, players[i].gpm / PaymentsPerMinute);
            }
			powerPlantTimer.Reset ();
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
