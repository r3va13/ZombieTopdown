using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
#region Singleton

    public static bool Initialized;
    static SceneManager _instance;
    public static SceneManager Instance
    {
        get
        {
            if (!_instance) _instance = GameObject.Find("Root").GetComponent<SceneManager>();
            Initialized = true;
            return _instance;
        }
    }
#endregion

    public void LoadGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1, LoadSceneMode.Additive);
        ClientServerController.Instance.Send("game_scene_loaded");
    }
}
