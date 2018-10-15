using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using C2D.Event;

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
		EventSystem.Global.RegisterListener<UIUpdateEventInfo>(OnUiUpdate);
    }
	private void OnUiUpdate(UIUpdateEventInfo info)
	{
		switch (info.Type)
		{
			case UiUpdateType.SOLDIER_TEXT:
				SetText(SoldierText, info.new_value, 10, "Soldiers"); // TODO: Don't hardcode this, add it somewhere
				break;
			case UiUpdateType.TANK_TEXT:
				SetText(TankText, info.new_value, 10, "Tanks"); // TODO: Don't hardcode this, add it somewhere
				break;
			case UiUpdateType.BUILDING_TEXT:
				SetText(BuildingText, info.new_value, 10, "Buildings"); // TODO: Don't hardcode this, add it somewhere
				break;
			case UiUpdateType.GOLD_TEXT:
				GoldText.text = info.new_value + " Gold";
				break;
			case UiUpdateType.GPM_TEXT:
				GPMText.text = "+" + info.new_value + " GPM";
				break;
				
		}
	}
    private static void SetText(Text text, int current, int max, string identifier)
    {
        text.text = current + "/" + max + " " + identifier;
    }
}
