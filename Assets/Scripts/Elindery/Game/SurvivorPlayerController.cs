using System;
using Elindery.Configs;
using UnityEngine;

namespace Elindery.Game
{
    public class SurvivorPlayerController : MonoBehaviour
    {
        static float _moveSpeed = 10;

        TheSurvivor _playerSurvivor;

        public static event EventHandler<TheSurvivor> OnPlayerSet; 

        public static void Initialize(TheSurvivor playerSurvivor)
        {
            SurvivorPlayerController instance = playerSurvivor.gameObject.AddComponent<SurvivorPlayerController>();

            instance._playerSurvivor = playerSurvivor;
            
            OnPlayerSet?.Invoke(null, instance._playerSurvivor);
        }

        public static void SetSurvivorConfig(string[] args)
        {
            _moveSpeed = Convert.ToSingle(args[1]);
        }

        public static void SetSurvivorConfig(SurvivorConfig config)
        {
            _moveSpeed = config.MoveSpeed;
        }

        void Update()
        {
            if (!GameController.GameStarted) return;
        
            _playerSurvivor.SetLookPosition(CameraController.Instance.GetMousePosition());
        
            float playerX = 0;
            float playerY = 0;
        
            if (Input.GetKey(KeyCode.D)) playerX += _moveSpeed;
            if (Input.GetKey(KeyCode.A)) playerX -= _moveSpeed;
            if (Input.GetKey(KeyCode.W)) playerY += _moveSpeed;
            if (Input.GetKey(KeyCode.S)) playerY -= _moveSpeed;

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
