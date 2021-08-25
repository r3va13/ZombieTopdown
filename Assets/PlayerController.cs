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

    const float MoveSpeed = 1f;

    
    
    public void Initialize(TheCharacter playerCharacter)
    {
        _playerCharacter = playerCharacter;
    }

    void Update()
    {
        float playerX = 0;
        float playerY = 0;
        
        if (Input.GetKey(KeyCode.D)) playerX += MoveSpeed;
        if (Input.GetKey(KeyCode.A)) playerX -= MoveSpeed;
        if (Input.GetKey(KeyCode.W)) playerY += MoveSpeed;
        if (Input.GetKey(KeyCode.S)) playerY -= MoveSpeed;

        _playerCharacter.SetPosition(new Vector2(playerX, playerY));
        
        _playerCharacter.SetLookPosition(TheCamera.Instance.GetMousePosition());
    }
}
