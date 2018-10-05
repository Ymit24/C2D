using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {
    private static Fade _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        if (EnableOnFadein != null)
        {
            EnableOnFadein.SetActive(false);
        }
        GetComponent<UnityEngine.UI.Image>().enabled = true;
    }
    public Animator anim;
    public GameObject EnableOnFadein;
    public void OnAnimationFadeInFinish()
    {
        if (EnableOnFadein != null)
        {
            EnableOnFadein.SetActive(true);
        }
    }
    private int scene;
    public static void LoadScene(int newScene)
    {
        _instance.scene = newScene;
        _instance.anim.SetTrigger("StartFadeOut");
    }

    public void OnAnimationTriggerFade()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
