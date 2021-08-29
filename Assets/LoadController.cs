using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadController : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
        if (RoomController.Instance) ;
        LogInController.Instance.Show();
    }
}
