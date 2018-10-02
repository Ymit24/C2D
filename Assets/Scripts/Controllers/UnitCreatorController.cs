using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCreatorController : MonoBehaviour {
    public GameObject UnitPanel;
	public List<UnitData> Units;

    public UnitFactory current;

    public GameObject UnitHolder;

    private int deselect;
    private void Start()
    {
        MouseController.MouseClickDownListeners += OnClick;
        UnitPanel.SetActive(false);
    }

    public void BuildUnit(int ab)
    {
        if (current == null)
            return;
		int cost = Units [(int)current.CanBuild [ab]].Cost;
		if (PlayerController.Data (0).money >= cost) {
			PlayerController.Data (0).money -= cost;
		} else {
			return;
		}
        GameObject newUnit = new GameObject("AwesomeUnit");
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
        u.Setup(Units[(int)current.CanBuild[ab]]);
        //GameObject g = Instantiate(Units[current.CanBuild[ab]].prefab, current.gameObject.transform.position + new Vector3(1, 1), Quaternion.identity);
        u.SetTeam(0);
        UnitPanel.SetActive(false);
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
            for (int i = 0; i < ubb.Length; i++)
            {
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
