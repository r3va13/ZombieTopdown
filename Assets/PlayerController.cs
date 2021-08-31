using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
#region Singleton
    static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            if (!_instance) _instance = GameObject.Find("Game").GetComponent<PlayerController>();
            return _instance;
        }
    }
#endregion
    
    TheCharacter _playerCharacter;
    public TheCharacter PlayerCharacter => _playerCharacter;

    const float MoveSpeed = 5f;

    
    
    public void Initialize(TheCharacter playerCharacter)
    {
        _playerCharacter = playerCharacter;
    }

    void Update()
    {
        if (!GameController.GameStarted) return;
        
        _playerCharacter.SetLookPosition(TheCamera.Instance.GetMousePosition());
        
        float playerX = 0;
        float playerY = 0;
        
        if (Input.GetKey(KeyCode.D)) playerX += MoveSpeed;
        if (Input.GetKey(KeyCode.A)) playerX -= MoveSpeed;
        if (Input.GetKey(KeyCode.W)) playerY += MoveSpeed;
        if (Input.GetKey(KeyCode.S)) playerY -= MoveSpeed;

        if (playerX != 0 || playerY != 0) _playerCharacter.SetPosition(new Vector3(playerX, playerY));
        
        if (Input.GetMouseButton(0)) _playerCharacter.Shoot();
        
        SendStateToServer();
    }

    float _sendAwait = 0.05f;
    void SendStateToServer()
    {
        if (!GameController.ServerOk) return;
        
        if (_sendAwait > 0) _sendAwait -= Time.deltaTime;
        else
        {
            _sendAwait = 0.1f;
            
            Vector2 newPosition = _playerCharacter.GetLastFramePosition(out bool havePositionChange);
        float newRotation = _playerCharacter.GetLastFrameRotation(out bool haveRotationChange);
        if (havePositionChange || haveRotationChange) ClientServerController.Instance.Send("player_move|" 
                                                                                           + _playerCharacter.ClientID + "|" +  
                                                                                           newPosition.x + "_" + newPosition.y + "|" + 
                                                                                           newRotation);
        }
    }
}
