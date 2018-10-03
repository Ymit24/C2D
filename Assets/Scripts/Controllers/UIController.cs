using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text SoldierText, TankText, BuildingText, GoldText, GPMText;
    private static UIController _instance;
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("UIController already exists!");
            Destroy(this);
        }
    }
    public static void OnSoldierCountChanged(int value)
    {
        SetText(_instance.SoldierText, value, 10, "Soldiers"); // TODO: Don't hardcode this, add it somewhere
    }
    public static void OnTankCountChanged(int value)
    {
        SetText(_instance.TankText, value, 10, "Tanks"); // TODO: Don't hardcode this, add it somewhere
    }
    public static void OnBuildingCountChanged(int value)
    {
        SetText(_instance.BuildingText, value, 10, "Buildings"); // TODO: Don't hardcode this, add it somewhere
    }
    public static void OnGoldCountChanged(int value)
    {
        _instance.GoldText.text = value + " Gold";
    }
    public static void OnGPMCountChanged(int value)
    {
        _instance.GPMText.text = "+" + value + " GPM";
    }
    private static void SetText(Text text, int current, int max, string identifier)
    {
        text.text = current + "/" + max + " " + identifier;
    }
}
