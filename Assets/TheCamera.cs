using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCamera : MonoBehaviour
{
#region Singleton
    static TheCamera _instance;
    public static TheCamera Instance
    {
        get
        {
            if (!_instance) _instance = GameObject.Find("Game Camera").GetComponent<TheCamera>();
            return _instance;
        }
    }
#endregion
    
    Transform _followingTarget;
    bool _following;
    Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    public void EnableFollowing(Transform target)
    {
        _followingTarget = target;
        _following = true;
    }

    public Vector2 GetMousePosition()
    {
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update()
    {
        if (_following) transform.localPosition = new Vector3(_followingTarget.localPosition.x, _followingTarget.localPosition.y, -10);
    }
}
