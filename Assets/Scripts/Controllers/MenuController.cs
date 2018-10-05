using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour {
    public MapConfiguration Config;
    public Text MapText, PlayerText;
    public Dropdown PlayerCount, Spawnpoint;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Config = Transform.FindObjectOfType<MapConfiguration>();
        if (Config == null)
        {
            Debug.LogWarning("MenuController -- can't find Config");
        }
        if (scene.buildIndex == 1) // config
        {
            MapText.text = "Map Name: " + (Config.SelectedMap == 0 ? "Simple" : "Island");
            PlayerText.text ="Max Players: " + (Config.maxPlayers[Config.SelectedMap]);
            PlayerCount.options.Clear();
            List<string> options = new List<string>();
            for (int i = 0; i < Config.maxPlayers[Config.SelectedMap]; i++)
            {
                options.Add((i+1).ToString());
            }
            PlayerCount.AddOptions(options);
            Spawnpoint.options.Clear();
            options.Clear();
            for (int i = 0; i < Config.spawnPoints[Config.SelectedMap]; i++)
            {
                options.Add((i+1).ToString());
            }
            Spawnpoint.AddOptions(options);
        }
        if (scene.buildIndex == 2) // Gameplay
        {
            Destroy(gameObject);
        }
    }
    public void OnPlayerCountChanged()
    {
        Config.PlayerCount = PlayerCount.value+1;
    }
    public void OnSpawnPointChanged()
    {
        Config.SpawnPoint = Spawnpoint.value+1;
    }
    public void SelectMap(int index)
    {
        if (Config != null)
        {
            Config.SelectedMap = index;
        }
    }
    public void SwitchScenes(int newScene)
    {
        if (newScene == 0)
        {
            Destroy(Config.gameObject);
        }
        SceneManager.LoadScene(newScene);
    }

}
