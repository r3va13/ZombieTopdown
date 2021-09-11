using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elindery.Client
{
    public class Client : MonoBehaviour
    {
        const string IP = "localhost";
        //const string IP = "192.168.0.183"; //Дом
        //const string IP = "18.221.164.4"; //Амазон

        static Client Instance => GameObject.Find("Root").GetComponent<Client>();

        static WebSocket _webSocket;
        static readonly List<string> _sendList = new List<string>();


        public static string RoomID { get; private set; } = "/Lobby";
        public static event EventHandler<string> OnRecieve;
        public static event EventHandler<string> OnDebugMessage;

        void Start()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new TaskManager();
        }

        public static void Connect()
        {
            Instance.StartCoroutine(Instance.ClientCoroutine());
        }

        public static void ConnectToRoomID(string id)
        {
            RoomID = id;
            _webSocket.Close();
            _webSocket.m_IsConnected = false;
        }

        public static void Send(string message)
        {
            _sendList.Add(message);
        }

        IEnumerator SendCor()
        {
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
            OnDebugMessage?.Invoke(this, "Попытка соединения " + _connectAttempt);

            _webSocket = new WebSocket(new Uri("ws://" + IP + ":4649" + RoomID));
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
                OnDebugMessage?.Invoke(this, "Не подключено");
            }
            else
            {
                sendCor = StartCoroutine(SendCor());
                OnDebugMessage?.Invoke(this, "Подключено");
                _connectAttempt = 0;
            }

            while (_webSocket.m_IsConnected)
            {
                string reply = _webSocket.RecvString();
                if (reply != null)
                {
                    string recieved = reply.ToString();
                    OnRecieve?.Invoke(this, recieved);
                }

                if (_webSocket.Error != null)
                {
                    _webSocket.m_IsConnected = false;
                    break;
                }

                yield return null;
            }

            if (sendCor != null) StopCoroutine(sendCor);
            OnDebugMessage?.Invoke(this, "Не подключено");

            Connect();
        }
    }
}

