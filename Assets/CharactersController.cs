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
    
    List<TheCharacter> _characters = new List<TheCharacter>();
    
    public TheCharacter CreateCharacter()
    {
        TheCharacter created = Instantiate(CharacterPrefab, transform);
        created.Initialize();
        _characters.Add(created);
        return created;
    }
}
