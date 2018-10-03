using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject {
	public Color color;
    public int money;

    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            UIController.OnGoldCountChanged(value);
        }
    }

    public int number_of_power_plants;

    public int Number_of_power_plants
    {
        get
        {
            return number_of_power_plants;
        }
        set
        {
            number_of_power_plants = value;
            UIController.OnGPMCountChanged(number_of_power_plants*10);
        }
    }
}
