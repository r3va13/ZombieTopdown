using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
#region Singleton
    static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (!_instance) _instance = GameObject.Find("Game").GetComponent<GameController>();
            return _instance;
        }
    }
#endregion
    
    void Start()
    {
        PlayerController.Instance.Initialize(CharactersController.Instance.CreateCharacter());
        TheCamera.Instance.EnableFollowing(PlayerController.Instance.PlayerCharacter.Transform);
    }
}
