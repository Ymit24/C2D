using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using C2D.Event;

public class UnitCreatorController : MonoBehaviour {
    public GameObject UnitPanel;
	public List<UnitData> Units;

    public UnitFactory current;

    public GameObject[] Projectiles;

    public GameObject UnitHolder;
    private static UnitCreatorController _instance;
    private int deselect;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    private void Start()
    {
		EventSystem.Global.RegisterListener<MouseUpdateEventInfo>(OnClick);
        UnitPanel.SetActive(false);
    }
	public static Unit PlaceUnit(UnitType type, Vector3 location, int owner, int projectile_index = 0)
    {
        GameObject newUnit = new GameObject(UnitData.NameFromType(type));
        newUnit.transform.SetParent(_instance.UnitHolder.transform);
        newUnit.transform.position = location;
        Unit u = newUnit.AddComponent<Unit>();
        u.Setup(_instance.Units[(int)type]);
        u.SetTeam(owner);
        u.GetComponent<Combat>().WhatToFire = _instance.Projectiles[projectile_index];
        MapController.UnitsPerPlayer[owner].Add(type);
		return u;
    }
    public void BuildUnit(int index)
    {
        if (current == null)
            return;
        if (index >= current.CanBuild.Count)
            return;
		int cost = Units [(int)current.CanBuild [index]].Cost;
        PlayerData data = PlayerController.GetPlayer(0);
        if (data.gold >= cost)
        {
			data.gold -= cost;
        }
        else
        {
            return;
        }
		Unit u = PlaceUnit(current.CanBuild[index], current.transform.position + new Vector3(1, 1), 0);

        UnitPanel.SetActive(false);
        UnitType type = u.Configuration.Type;
		if (UnitData.isSoldier(type))
			EventSystem.Global.FireEvent(new UIUpdateEventInfo(UiUpdateType.SOLDIER_TEXT, MapController.SoldierCount(0)));
        if (UnitData.isTank(type))
			EventSystem.Global.FireEvent(new UIUpdateEventInfo(UiUpdateType.TANK_TEXT, MapController.TankCount(0)));
    }

	private void OnClick(MouseUpdateEventInfo info)
    {
		if (info.Button == MouseButton.RIGHT)
        {
            current = null;
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (hits.Length < 1)
            {
                UnitPanel.SetActive(false);
                return;
            }
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider is BoxCollider2D && hits[i].collider.GetComponent<UnitFactory>() != null)
                {
                    current = hits[i].collider.GetComponent<UnitFactory>();
                }
            }
            if (current == null)
                return;
            Button[] ubb = UnitPanel.GetComponentsInChildren<Button>();
            if (ubb.Length > current.CanBuild.Count)
            {
                //TODO: remove extra buttons
            }
            else if (ubb.Length < current.CanBuild.Count)
            {
                //TODO: add missing buttons
            }
            for (int i = 0; i < current.CanBuild.Count; i++)
            {
                if (i == ubb.Length)
                    break; // more units then buttons, FIXME: this should be removed once prior two TODOs are implemented.
                Text text = ubb[i].GetComponentInChildren<Text>();
                if (text != null) {
                    text.text = UnitData.NameFromType(current.CanBuild[i]);
                }
            }

            UnitPanel.SetActive(true);
            RectTransform rt = UnitPanel.GetComponent<RectTransform>();
            rt.position = Input.mousePosition + new Vector3( rt.rect.width / 2,- rt.rect.height / 2 );
        }
		if (info.Button == MouseButton.LEFT)
        {
            //deselect = 1;
        }
    }

    private const int frames_for_deselect = 20;
    protected void Update()
    {
        if (deselect < frames_for_deselect && deselect != 0)
        {
            deselect ++;
        } else if (deselect == frames_for_deselect)
        {
            deselect = 0;
            UnitPanel.SetActive(false);
        }
    }

    public static int GetUnitCost(UnitType type)
    {
        for (int i = 0; i < _instance.Units.Count; i++)
        {
            if (_instance.Units[i].Type == type)
            {
                return _instance.Units[i].Cost;
            }
        }
        Debug.LogError("Couldn't find Unit with type of " + type + "!");
        return -1;
    }
}
