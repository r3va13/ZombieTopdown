using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadController : MonoBehaviour
{
    void Start()
    {
        if (RoomController.Instance) ;
        LogInController.Instance.Show();
    }
}
