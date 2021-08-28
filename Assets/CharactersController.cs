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
    
    public TheCharacter CharacterPrefab;
    
    Dictionary<string, TheCharacter> _characters = new Dictionary<string, TheCharacter>();
    
    public TheCharacter CreateCharacter(string clientID, Vector2 position)
    {
        TheCharacter created = Instantiate(CharacterPrefab, transform);
        created.Initialize();
        created.transform.localPosition = position;
        _characters.Add(clientID, created);
        return created;
    }
}
