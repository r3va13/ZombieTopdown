using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TheGame : MonoBehaviour
{
    public string RoomID;

    Button _enterBtn;
    TMP_Text _hostLbl;

    public void Initialize(string id)
    {
        RoomID = id;

        _hostLbl = transform.Find("Host").GetComponent<TMP_Text>();
        _enterBtn = transform.Find("ButtonEnter").GetComponent<Button>();

        _hostLbl.text = "Игрок " + RoomID;
        _enterBtn.onClick.AddListener(OnEnterBtn);
    }

    void OnEnterBtn()
    {
        
    }
}
