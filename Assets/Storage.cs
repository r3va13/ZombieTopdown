using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
#region Singleton
    static Storage _instance;
    public static Storage Instance
    {
        get
        {
            if (_instance) return _instance;
            
            _instance = GameObject.Find("Root").GetComponent<Storage>();
            
            return _instance;
        }
    }
#endregion
    
    public string ClientID;
}
