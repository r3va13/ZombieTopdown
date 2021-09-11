using System;
using Elindery.Configs;
using UnityEngine;

namespace Elindery.Game
{
    public class SurvivorPlayerController : MonoBehaviour
    {
        static float _moveSpeed = 10;
        static float _moveSpeedOnShoot = 6;

        public static event EventHandler<TheSurvivor> OnPlayerSet; 

        public static void Initialize(TheSurvivor playerSurvivor)
        {
            SurvivorPlayerController instance = playerSurvivor.gameObject.AddComponent<SurvivorPlayerController>();

            instance._playerSurvivor = playerSurvivor;
            instance._playerSurvivor.Health.OnDie += instance.OnPlayerDied;
            
            OnPlayerSet?.Invoke(null, instance._playerSurvivor);
        }

        public static void SetSurvivorConfig(string[] args)
        {
            _moveSpeed = Convert.ToSingle(args[1]);
            _moveSpeedOnShoot = Convert.ToSingle(args[2]);
        }

        public static void SetSurvivorConfig(SurvivorConfig config)
        {
            _moveSpeed = config.MoveSpeed;
        }
        
        
        
        
        TheSurvivor _playerSurvivor;
        bool _playerIsDead;

        void OnPlayerDied()
        {
            _playerIsDead = true;
        }

        void Update()
        {
            if (!GameController.GameStarted) return;
            if (_playerIsDead) return;
        
            if (!_playerSurvivor.IsShootingState && Input.GetMouseButton(0)) _playerSurvivor.SetLookPosition(CameraController.Instance.GetMousePosition(), true);
            if (_playerSurvivor.IsShootingState) _playerSurvivor.SetLookPosition(CameraController.Instance.GetMousePosition(), false);
        
            float playerX = 0;
            float playerY = 0;

            float speed = _playerSurvivor.IsShootingState ? _moveSpeedOnShoot : _moveSpeed;
            
            if (Input.GetKey(KeyCode.D)) playerX += speed;
            if (Input.GetKey(KeyCode.A)) playerX -= speed;
            if (Input.GetKey(KeyCode.W)) playerY += speed;
            if (Input.GetKey(KeyCode.S)) playerY -= speed;

            if (playerX != 0 || playerY != 0) _playerSurvivor.SetPlayerPosition(new Vector3(playerX, playerY));
            else _playerSurvivor.ClearPredictMovement();
        
            if (Input.GetMouseButton(0)) _playerSurvivor.PlayerShoot();
        
            SendStateToServer();
        }

        float _sendTimeout = 0.1f;
        void SendStateToServer()
        {
            if (!GameController.ServerOk) return;
        
            if (_sendTimeout > 0) _sendTimeout -= Time.deltaTime;
            else
            {
                _sendTimeout = 0.1f;
            
                Vector2 newPosition = _playerSurvivor.GetLastFramePosition(out bool havePositionChange);
                float newRotation = _playerSurvivor.GetLastFrameRotation(out bool haveRotationChange);
                if (havePositionChange || haveRotationChange) 
                    Client.Client.Send("player_move|" + _playerSurvivor.ClientID + "|" + newPosition.x + "_" + newPosition.y + "|" + newRotation);
            }
        }
    }
}
