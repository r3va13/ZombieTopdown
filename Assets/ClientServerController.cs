using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientServerController : MonoBehaviour
{
#region Singleton
    static ClientServerController _instance;
    public static ClientServerController Instance
    {
        get
        {
            if (!_instance) _instance = GameObject.Find("Root").GetComponent<ClientServerController>();
            return _instance;
        }
    }
#endregion
    
    WebSocket _webSocket;

    const string IP = "192.168.0.183"; //Дом
    //const string IP = "18.221.164.4"; //Амазон
    string _roomID = "/Lobby";
    public string RoomID => _roomID;

    readonly List<string> _sendList = new List<string>();

    Coroutine _currentConnection;
    
    public void Connect()
    {
        _currentConnection = StartCoroutine(ClientCoroutine());
    }

    void ConnectToRoomID(string id)
    {
        _roomID = id;
        _webSocket.Close();
        _webSocket.m_IsConnected = false;
        //Connect();
    }

    public void Send(string message)
    {
        _sendList.Add(message);
    }

    IEnumerator SendCor()
    {
        //yield return new WaitForSecondsRealtime(0.05f);
        
        while (true)
        {
            if (_sendList.Count > 0)
            { 
                _webSocket.SendString(_sendList[0]);

                if (_webSocket.m_IsConnected) _sendList.RemoveAt(0);
            }
            else yield return null;
        }
    }

    int _connectAttempt = 1;
    IEnumerator ClientCoroutine()
    {
        LogInController.Instance.ChangeLoginText("Попытка соединения " + _connectAttempt);
        
        _webSocket = new WebSocket(new Uri("ws://" + IP + ":4649" + _roomID));
        Coroutine connectCor = StartCoroutine(_webSocket.Connect());
        
        float timeOut = 0;
        while (!_webSocket.m_IsConnected && timeOut < 5)
        {
            timeOut += 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        
        Coroutine sendCor = null;
        if (!_webSocket.m_IsConnected)
        {
            _connectAttempt++;
            StopCoroutine(connectCor);
            LogInController.Instance.ChangeLoginText("Не подключено");
        }
        else
        {
            sendCor = StartCoroutine(SendCor());
            LogInController.Instance.ChangeLoginText("Подключено");
            _connectAttempt = 0;
        }

        while (_webSocket.m_IsConnected)
        {
            string reply = _webSocket.RecvString();
            if (reply != null)
            {
                string recieved = reply.ToString();
                OnRecieve(recieved);
            }

            if (_webSocket.Error != null)
            {
                _webSocket.m_IsConnected = false;
                break;
            }

            yield return null;
        }

        if (sendCor != null) StopCoroutine(sendCor);
        LogInController.Instance.ChangeLoginText("Не подключено");
        
        Connect();
    }

    void OnRecieve(string message)
    {
        Debug.Log(message);
        
        string[] args = message.Split('|');
        
        switch (args[0])
        {
            case "connected_to_lobby":
                if (Storage.Instance.ClientID == "") _webSocket.SendString("create_new_player");
                EventManager.OnLoginEvent();
                break;
            case "id":
                Storage.Instance.ClientID = args[1];
                break;
            case "join_room":
                RoomController.Instance.Hide();
                RoomLobbyController.Instance.Show();
                ConnectToRoomID(args[1]);
                break;
            case "join_lobby":
                ConnectToRoomID("/Lobby");
                break;
            case "connected_to_room":
                _webSocket.SendString("join_room|" + _roomID + "|" + Storage.Instance.ClientID);
                break;
            case "refresh_room":
                RoomLobbyController.Instance.RefreshRoomUsers(args);
                break;
            case "join_game":
                RoomLobbyController.Instance.StartCountdown(args);
                break;
            case "create_player":
                GameController.Instance.CreatePlayer(args);
                break;
            case "wait_players":
                GameController.Instance.WaitPlayers(args);
                break;
            case "game_start":
                GameController.Instance.GameStart(args);
                break;
            case "user_states":
                CharactersController.Instance.UserStates(args);
                break;
        }
        
    }
}
