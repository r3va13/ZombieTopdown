using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
#region Singleton
    static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (!_instance) _instance = GameObject.Find("GameUI").GetComponent<GameController>();
            return _instance;
        }
    }
#endregion

    public static bool ServerOk;
    
    TMP_Text _messageLbl;

    public static bool GameStarted;
    DateTime _waitPlayersTime;
    DateTime _gameStartTime;
    
    void Start()
    {
        _messageLbl = transform.Find("Message").GetComponent<TMP_Text>();
        
        //Запуск просто сцены игры
        if (!SceneManager.Initialized)
        {
            ServerOk = false;
            PlayerController.Instance.Initialize(CharactersController.Instance.CreateCharacter("DebugName", new Vector2(0, 0)));
            TheCamera.Instance.EnableFollowing(PlayerController.Instance.PlayerCharacter.Transform);
            GameStarted = true;
        }
        //Запуск через сервер
        else
        {
            ServerOk = true;
            enabled = false;
            ClientServerController.Instance.Send("game_scene_loaded");
        }
    }

    public void WaitPlayers(string[] args)
    {
        _waitPlayersTime = Convert.ToDateTime(args[1]);
        _gameStartTime = Convert.ToDateTime(args[2]);
        enabled = true;
    }
    
    public void CreatePlayer(string[] args)
    {
        TheCharacter character = CharactersController.Instance.CreateCharacter(args[1], Utils.GetVector2FromString(args[2]));

        if (args[1] != Storage.Instance.ClientID) return;
        
        PlayerController.Instance.Initialize(character);
        TheCamera.Instance.EnableFollowing(PlayerController.Instance.PlayerCharacter.Transform);
    }

    public void GameStart(string[] args)
    {
        _gameStartTime = Convert.ToDateTime(args[1]);
        _waitPlayersTime = new DateTime();
    }

    void FixedUpdate()
    {
        if (!GameStarted)
        {
            if (DateTime.UtcNow < _waitPlayersTime)
            {
                TimeSpan ts = _waitPlayersTime - DateTime.UtcNow;
                _messageLbl.text = "Ожидание игроков " + ts.ToString("ss");
                return;
            }

            if (DateTime.UtcNow < _gameStartTime)
            {
                TimeSpan ts = _gameStartTime - DateTime.UtcNow;
                _messageLbl.text = "До начала игры " + ts.ToString("ss");
                return;
            }

            _messageLbl.text = "";
            GameStarted = true;
        }
    }
}
