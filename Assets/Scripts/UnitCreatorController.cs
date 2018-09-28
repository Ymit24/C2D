using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCreatorController : MonoBehaviour {
    public GameObject UnitPanel;
	public List<UnitData> Units;

    public UnitFactory current;

    private int deselect;
    private void Start()
    {
        MouseController.MouseClickDownListeners += OnClick;
        UnitPanel.SetActive(false);
    }
    private string unitname(int type)
    {
		return Units [type].name;
//        switch (type)
//        {
//            case 0:
//                return "Light Soldier";
//            case 1:
//                return "Heavy Soldier";
//            case 2:
//                return "Light Tank";
//            case 3:
//                return "Heavy Tank";
//            default:
//                return "DEFAULT " + type;
//        }
    }

    public void BuildUnit(int ab)
    {
        if (current == null)
            return;
		int cost = Units [current.CanBuild [ab]].cost;
		if (PlayerController.Data (0).money >= cost) {
			PlayerController.Data (0).money -= cost;
		} else {
			return;
		}
		Instantiate(Units[current.CanBuild[ab]].prefab, current.gameObject.transform.position + new Vector3(1, 1), Quaternion.identity);
        UnitPanel.SetActive(false);
    }

    private void OnClick(int button, Vector2 position)
    {
        if (button == 1)
        {
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (!ray)
            {
                UnitPanel.SetActive(false);
                return;
            }

            current = ray.collider.GetComponent<UnitFactory>();
            if (current == null)
                return;
            Button[] ubb = UnitPanel.GetComponentsInChildren<Button>();
            for (int i = 0; i < ubb.Length; i++)
            {
                Text text = ubb[i].GetComponentInChildren<Text>();
                if (text != null) {
                    text.text = unitname(current.CanBuild[i]);
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
