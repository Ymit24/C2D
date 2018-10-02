using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Selectability : MonoBehaviour {

    //public bool _selected;
    //public bool IsSelected
    //{
    //    get
    //    {
    //        return _selected;
    //    }
    //    private set
    //    {
    //        _selected = value;
    //        if (graphic != null)
    //        {
    //            graphic.SetActive(value);
    //        }
    //    }
    //}

    //private int deselect; // this is so we don't deselect before another script can handle selection on the same frame
    //                       // e.g. soldier needing to set move target when we click but we unselect first.

    //private BoxCollider2D bc2d;
    //private GameObject graphic;

    //void Awake()
    //{
    //    bc2d = GetComponent<BoxCollider2D>();
    //    graphic = transform.Find("SelectedGraphic").gameObject;
    //    SelectionBox.OnClearSelected += OnClearSelected;

    //    IsSelected = false;
    //}

    //protected void Update()
    //{
    //    if (deselect == 1)
    //    {
    //        deselect++;
    //    }
    //    else if (deselect == 2)
    //    {
    //        deselect = 0;
    //        IsSelected = false;
    //    }
    //}

    //public void OnClearSelected()
    //{
    //    deselect = 1;
    //    //IsSelected = false;
    //}

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (!bc2d.IsTouching(col))
    //    {
    //        return;
    //    }
    //    if (col.CompareTag("SelectionBox"))
    //    {
    //        IsSelected = true;
    //    }
    //}

    //void OnTriggerExit2D(Collider2D col)
    //{
    //    if (col.CompareTag("SelectionBox"))
    //    {
    //        IsSelected = false;
    //    }
    //}
}
