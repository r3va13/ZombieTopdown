using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
#region Singleton
    static LobbyController _instance;
    public static LobbyController Instance
    {
        get
        {
            if (_instance) return _instance;
            
            _instance = GameObject.Find("Root").transform.Find("LobbyController").GetComponent<LobbyController>();
            _instance.GetWhatYouNeed();
            
            return _instance;
        }
    }
#endregion

    TMP_Text _survivorsLbl, _zombiesLbl;
    
    void GetWhatYouNeed()
    {
        _survivorsLbl = transform.Find("Survivors").Find("Text (TMP)").GetComponent<TMP_Text>();
        _zombiesLbl = transform.Find("Zombies").Find("Text (TMP)").GetComponent<TMP_Text>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _survivorsLbl.text = "Выжившие:";
        _zombiesLbl.text = "Зомби:";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void RefreshUsers(string[] args)
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
    }
}
