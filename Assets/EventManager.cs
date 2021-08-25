using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void Login();

    public static event Login LoginEvent;

    public static void OnLoginEvent()
    {
        LoginEvent?.Invoke();
    }
}
