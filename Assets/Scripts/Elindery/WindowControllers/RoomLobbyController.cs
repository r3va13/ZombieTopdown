using System;
using Elindery.Internals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elindery.WindowControllers
{
    public class RoomLobbyController : MonoBehaviour
    {
#region Singleton
        static RoomLobbyController _instance;
        public static RoomLobbyController Instance
        {
            get
            {
                if (_instance) return _instance;
            
                _instance = GameObject.Find("Root").transform.Find("RoomLobbyController").GetComponent<RoomLobbyController>();
                _instance.GetWhatYouNeed();
            
                return _instance;
            }
        }
#endregion

        TMP_Text _survivorsLbl, _zombiesLbl, _countdownLbl;
        Button _exitBtn;
    
        void GetWhatYouNeed()
        {
            _survivorsLbl = transform.Find("Survivors").Find("Text (TMP)").GetComponent<TMP_Text>();
            _zombiesLbl = transform.Find("Zombies").Find("Text (TMP)").GetComponent<TMP_Text>();
            _countdownLbl = transform.Find("TextCountdown").GetComponent<TMP_Text>();
            _exitBtn = transform.Find("ButtonExit").GetComponent<Button>();
            _exitBtn.onClick.AddListener(OnExitBtn);
        }

        public void Show()
        {
            enabled = false;
            gameObject.SetActive(true);
            _survivorsLbl.text = "Выжившие:";
            _zombiesLbl.text = "Зомби:";
            _countdownLbl.text = "Ожидание игроков";
            _exitBtn.gameObject.SetActive(true);
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }

        void OnExitBtn()
        {
            Hide();
            Client.Client.Send("left_room|" + Client.Client.RoomID + "|" + Storage.ClientID);
            LogInController.Instance.Show();
        }

        public void RefreshRoomUsers(string[] args)
        {
            string surv = "Выжившие:<br>";
            string zomb = "Зомби:<br>";
        
            for (int i = 1; i < args.Length; i++)
            {
                if (args[i].StartsWith("s_")) surv += "<br>" + args[i].Remove(0, 2);
                else  zomb += "<br>" + args[i].Remove(0, 2);
            }

            _survivorsLbl.text = surv;
            _zombiesLbl.text = zomb;
            _countdownLbl.text = "Ожидание игроков";
        }

        DateTime _gameStartTime;
        bool _startGame;
        public void StartCountdown(string[] args)
        {
            _exitBtn.gameObject.SetActive(false);
            _gameStartTime = Convert.ToDateTime(args[1]);
            _startGame = true;
            enabled = true;
        }

        void FixedUpdate()
        {
            if (!_startGame) return;
        
            TimeSpan ts = _gameStartTime - DateTime.UtcNow;
            _countdownLbl.text = "Игра начнется через " + ts.ToString("ss");
            if (ts.TotalSeconds > 0) return;
        
            _startGame = false;
            Hide();
            SceneManager.LoadGameScene();
        }
    }
}
