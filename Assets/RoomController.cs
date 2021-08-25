using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviour
{
#region Singleton
    static RoomController _instance;
    public static RoomController Instance
    {
        get
        {
            if (_instance) return _instance;
            
            _instance = GameObject.Find("Root").transform.Find("RoomController").GetComponent<RoomController>();
            _instance.GetWhatYouNeed();
            
            return _instance;
        }
    }
#endregion

    Button _survivorBtn;
    Button _zombieBtn;

    void GetWhatYouNeed()
    {
        _survivorBtn = transform.Find("ButtonSurvivor").GetComponent<Button>();
        _zombieBtn = transform.Find("ButtonZombie").GetComponent<Button>();
        _survivorBtn.onClick.AddListener(FindGameSurvivor);
        _zombieBtn.onClick.AddListener(FindGameZombie);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    void FindGameSurvivor()
    {
        ClientServerController.Instance.Send("find_game|survivor|" + Storage.Instance.ClientID);
    }
    
    void FindGameZombie()
    {
        ClientServerController.Instance.Send("find_game|zombie|" + Storage.Instance.ClientID);
    }
}
