using System;
using Elindery.Configs;
using Elindery.Internals;
using UnityEngine;

namespace Elindery.Game
{
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

        public static bool ServerOk;
    
        public static bool GameStarted;
        DateTime _waitPlayersTime;
        DateTime _gameStartTime;

        public static event EventHandler<string> OnMessage; 
    
        void Start()
        {
            //Запуск просто сцены игры
            if (!SceneManager.Initialized)
            {
                ServerOk = false;
                LoadController.OnStart();
                TheSurvivor playerSurvivor = SurvivorsController.Instance.CreateSurvivor("DebugName", new Vector2(0, 0), ConfigsProvider.SurvivorConfigs[0].HP);
                playerSurvivor.SetWeaponLocal(ConfigsProvider.WeaponConfigs[0]);
                GameUIController.Instance.OnSurvivorCreated(playerSurvivor);
                SetUpPlayerSurvivor(playerSurvivor);
                SurvivorPlayerController.SetSurvivorConfig(ConfigsProvider.SurvivorConfigs[0]);
                EnemiesController.Instance.OnLocalGameStart();
                GameStarted = true;
            }
            //Запуск через сервер
            else
            {
                ServerOk = true;
                enabled = false;
                Client.Client.Send("game_scene_loaded");
            }
        }

        public void WaitPlayers(string[] args)
        {
            _waitPlayersTime = Convert.ToDateTime(args[1]);
            _gameStartTime = Convert.ToDateTime(args[2]);
            enabled = true;
        }
    
        public void CreateSurvivor(string[] args)
        {
            TheSurvivor survivor = SurvivorsController.Instance.CreateSurvivor(args[1], Utils.GetVector2FromString(args[2]), Convert.ToInt32(args[3]));
            GameUIController.Instance.OnSurvivorCreated(survivor);
            
            if (args[1] != Storage.ClientID) return;
        
            SetUpPlayerSurvivor(survivor);
        }

        void SetUpPlayerSurvivor(TheSurvivor playerSurvivor)
        {
            SurvivorPlayerController.Initialize(playerSurvivor);
            CameraController.Instance.EnableFollowing(playerSurvivor.Transform);
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
                    if (OnMessage != null)
                    {
                        TimeSpan ts = _waitPlayersTime - DateTime.UtcNow;
                        string message = "Ожидание игроков " + ts.ToString("ss");
                        OnMessage?.Invoke(this, message);
                    }
                
                    return;
                }

                if (DateTime.UtcNow < _gameStartTime)
                {
                    if (OnMessage != null)
                    {
                        TimeSpan ts = _gameStartTime - DateTime.UtcNow;
                        string message = "До начала игры " + ts.ToString("ss");
                        OnMessage?.Invoke(this, message);
                    }
                
                    return;
                }

                OnMessage?.Invoke(this, "");
                GameStarted = true;
            }
        }
    }
}
