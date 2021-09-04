using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersController : MonoBehaviour
{
#region Singleton
    static CharactersController _instance;
    public static CharactersController Instance
    {
        get
        {
            if (!_instance) _instance = GameObject.Find("Game").transform.Find("Characters").GetComponent<CharactersController>();
            return _instance;
        }
    }
#endregion

    public Material BulletTracerMaterial;
    public TheCharacter CharacterPrefab;
    
    Dictionary<string, TheCharacter> _characters = new Dictionary<string, TheCharacter>();
    
    public TheCharacter CreateCharacter(string clientID, Vector2 position)
    {
        TheCharacter created = Instantiate(CharacterPrefab, transform);
        created.Initialize();
        created.InitializePositionFromServer(position);
        _characters.Add(clientID, created);
        created.ClientID = clientID;
        return created;
    }
    
    public void SetWeapon(string[] args)
    {
        _characters[args[1]].SetWeapon(args[2]);
    }

    public void UserStates(string[] args)
    {
        for (int i = 1; i < args.Length; i++)
        {
            UserMove(args[i]);
        }
    }

    public void UserShoot(string[] args)
    {
        _characters[args[1]].ShootFromServer(args[2]);
    }

    void UserMove(string userLine)
    {
        string[] args = userLine.Split('_');
        
        if (!_characters.ContainsKey(args[0])) return;
        if (Storage.Instance.ClientID == args[0]) return;

        float posX = Convert.ToSingle(args[1]);
        float posY = Convert.ToSingle(args[2]);
        float rotation = Convert.ToSingle(args[3]);
        _characters[args[0]].SetServerLookAngle(rotation);
        _characters[args[0]].SetServerPosition(new Vector3(posX, posY, 0));
    }

    public static event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Vector3 BulletStartPosition;
        public Vector3 BulletDirection;
    }
}
