using System.Collections;
using System.Collections.Generic;
using Elindery.Game;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
#region Singleton
    static GameUIController _instance;
    public static GameUIController Instance
    {
        get
        {
            if (!_instance) _instance = GameObject.Find("GameUI").GetComponent<GameUIController>();
            return _instance;
        }
    }
#endregion
    
    [SerializeField] ThePotraitUI PortraitPrefab;

    TMP_Text _messageLbl;
    TMP_Text _ammoLbl;
    Transform _portraitsSurvivorsRoot;
    GameObject _survivorUI;

    TheSurvivor _playerSurvivor;

    void Awake()
    {
        _messageLbl = transform.Find("Message").GetComponent<TMP_Text>();
        _portraitsSurvivorsRoot = transform.Find("Portraits").Find("Left");
        _survivorUI = transform.Find("SurvivorUI").gameObject;
        _ammoLbl = _survivorUI.transform.Find("AmmoLbl").GetComponent<TMP_Text>();
        
        GameController.OnMessage += OnMessage;
        SurvivorPlayerController.OnPlayerSet += OnPlayerSurvivorSet;
    }

    void OnMessage(object sender, string message)
    {
        _messageLbl.text = message;
    }

    public void OnSurvivorCreated(TheSurvivor survivor)
    {
        ThePotraitUI potraitUI = Instantiate(PortraitPrefab.gameObject, _portraitsSurvivorsRoot).GetComponent<ThePotraitUI>();
        potraitUI.Initialize(survivor);
    }

    void OnPlayerSurvivorSet(object sender, TheSurvivor playerSurvivor)
    {
        _playerSurvivor = playerSurvivor;
        playerSurvivor.OnWeaponChange += OnPlayerWeaponChange;
    }

    void OnPlayerWeaponChange()
    {
        _playerSurvivor.Weapon.OnAmmoChanged += OnAmmoChanged;
        OnAmmoChanged();
    }

    void OnAmmoChanged()
    {
        _ammoLbl.text = _playerSurvivor.Weapon.LoadAmmo + "/" + _playerSurvivor.Weapon.HoldAmmo;
    }

    void OnPlayerDamage()
    {
        
    }
}
