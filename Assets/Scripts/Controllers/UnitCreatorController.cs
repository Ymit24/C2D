using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCreatorController : MonoBehaviour {
    public GameObject UnitPanel;
	public List<UnitData> Units;

    public UnitFactory current;

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
        MouseController.MouseClickDownListeners += OnClick;
        UnitPanel.SetActive(false);
    }
    public static void PlaceUnit(UnitType type, Vector3 location, int owner)
    {
        GameObject newUnit = new GameObject(UnitData.NameFromType(type));
        newUnit.transform.SetParent(_instance.UnitHolder.transform);
        newUnit.transform.position = location;
        Unit u = newUnit.AddComponent<Unit>();
        u.Setup(_instance.Units[(int)type]);
        u.SetTeam(owner);
        MapController.UnitsPerPlayer[owner].Add(type);
    }
    public void BuildUnit(int index)
    {
        if (current == null)
            return;
        if (index >= current.CanBuild.Count)
            return;
		int cost = Units [(int)current.CanBuild [index]].Cost;
		if (PlayerController.Data (0).Gold >= cost) {
			PlayerController.Data (0).Gold -= cost;
		} else {
			return;
		}
        GameObject newUnit = new GameObject(UnitData.NameFromType(current.CanBuild[index]));
        newUnit.transform.SetParent(UnitHolder.transform);
        if (current != null)
        {
            newUnit.transform.position = current.transform.position + new Vector3(1, 1);
        }
        else
        {
            Debug.LogWarning("BuildUnit -- Current is null?!");
        }

        Unit u = newUnit.AddComponent<Unit>();
        u.Setup(Units[(int)current.CanBuild[index]]);
        //GameObject g = Instantiate(Units[current.CanBuild[ab]].prefab, current.gameObject.transform.position + new Vector3(1, 1), Quaternion.identity);
        u.SetTeam(0);
        UnitPanel.SetActive(false);
        UnitType type = u.Configuration.Type;
        MapController.UnitsPerPlayer[0].Add(type);
        if (UnitData.isSoldier(type))
            UIController.OnSoldierCountChanged(MapController.SoldierCount(0));
        if (UnitData.isTank(type))
            UIController.OnTankCountChanged(MapController.TankCount(0));
    }

    private void OnClick(int button, Vector2 position)
    {
        if (button == 1)
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
        if (button == 0)
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
}
