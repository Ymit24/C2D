using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitBuildButton : MonoBehaviour {
    public void SetText(string text)
    {
        GetComponentInChildren<Text>().text = text;
    }
}
