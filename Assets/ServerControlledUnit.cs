using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class ServerControlledUnit : MonoBehaviour
{
    public string ClientID { get; set; }
    public Transform Transform => _transform;
    public Rigidbody2D Rigidbody2D;

    Transform _transform;
    protected Transform Holder;
    
    protected float WalkTurnOffTime;
    float _serverMoveDelayTime = 100; //0.1 сек. для плавной отрисовки движения и поворота с момента получения команды с сервера

    public TheHealth Health { get; private set; }

    //Server
    protected Vector2 OldPosition;
    protected Vector2 NewPosition;
    protected float OldRotation;
    protected float NewRotation;

    public virtual void Initialize()
    {
        _transform = transform;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Health = GetComponent<TheHealth>();
        Holder = _transform.Find("Holder");
        
        OldPosition = NewPosition = _transform.position;
        OldRotation = NewRotation = Holder.eulerAngles.z;

        _serverMoveDelayTime = 100;
    }
    
    public virtual void InitializePositionFromServer(Vector3 position)
    {
        Transform.position = position;
        OldPosition = NewPosition = _transform.position;
    }

    public virtual void InitializeLookDirectionFromServer(float angle)
    {
        Rigidbody2D.MoveRotation(angle);
        OldRotation = NewRotation = angle;
    }
    
    public virtual void SetServerPosition(Vector3 position)
    {
        OldPosition = Rigidbody2D.position;
        NewPosition = position;
        _serverMoveDelayTime = 0f;
        WalkTurnOffTime = 0.1f;
    }

    public void SetServerLookAngle(float angle)
    {
        OldRotation = Rigidbody2D.rotation;
        NewRotation = angle;
    }

    protected virtual void FixedUpdate()
    {
        PlayMoveAndRotationFromServer();

        if (WalkTurnOffTime > 0) WalkTurnOffTime -= Time.deltaTime;
    }

    void PlayMoveAndRotationFromServer()
    {
        if (_serverMoveDelayTime < 0 || _serverMoveDelayTime > 1) return;
        
        Rigidbody2D.MovePosition(Vector3.Lerp(OldPosition, NewPosition, _serverMoveDelayTime));
        float angle = Mathf.LerpAngle(OldRotation , NewRotation, _serverMoveDelayTime);
        Rigidbody2D.MoveRotation(angle);
        _serverMoveDelayTime += Time.deltaTime * 10;
    }
}
