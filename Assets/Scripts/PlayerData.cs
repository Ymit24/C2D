using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject {
	public Color color;
    public int gold;

    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
            PlayerController.GoldChanged(this);
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
            PlayerController.GPMChanged(this);
        }
    }

    public int GPM
    {
        get 
        {
            return Number_of_power_plants * 10 + 25; // 25 is a base GPM
        }
    }
}
