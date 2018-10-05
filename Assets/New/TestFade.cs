using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFade : MonoBehaviour {
	void Update () {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Starting fade");
            Fade.LoadScene(1);
        }
	}
}
