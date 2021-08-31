using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class ServerControlledUnit : MonoBehaviour
{
    public string ClientID { get; set; }
    public Transform Transform => _transform;

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
        Health = GetComponent<TheHealth>();
        Holder = _transform.Find("Holder");
        
        OldPosition = NewPosition = _transform.position;
        OldRotation = NewRotation = Holder.eulerAngles.z;

        _serverMoveDelayTime = 100;
    }
    
    public virtual void SetPositionFromServer(Vector3 position)
    {
        OldPosition = _transform.position;
        NewPosition = position;
        _serverMoveDelayTime = 0f;
        WalkTurnOffTime = 0.1f;
    }
    
    public void SetServerLookPosition(float angle)
    {
        OldRotation = Holder.eulerAngles.z;
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
        
        _transform.position = Vector3.Lerp(OldPosition, NewPosition, _serverMoveDelayTime);
        float angle = Mathf.LerpAngle(OldRotation , NewRotation, _serverMoveDelayTime);
        Holder.eulerAngles = new Vector3(0, 0, angle);
        _serverMoveDelayTime += Time.deltaTime * 10;
    }
}
