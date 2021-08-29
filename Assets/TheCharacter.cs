using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class TheCharacter : MonoBehaviour
{
    public string ClientID;
    
    Transform _transform;
    public Transform Transform => _transform;
    
    Transform _holder;
    Transform _bulletStartPoint, _bulletDirectionPoint;
    Animator _gunAnimator;
    Animator _bodyAnimator;
    Animator _feetAnimator;
    float _runTurnOffTime;
    float _serverMoveTime;
    
    //Server
    Vector2 _oldPosition;
    Vector2 _newPosition;
    float _oldRotation;
    float _newRotation;

    public void Initialize()
    {
        _transform = transform;
        _holder = transform.Find("Holder");
        
        _bodyAnimator = _holder.Find("Body").GetComponent<Animator>();
        _gunAnimator = _bodyAnimator.transform.Find("Gun").GetComponent<Animator>();
        _feetAnimator = _holder.Find("Feet").GetComponent<Animator>();

        _bulletStartPoint = _gunAnimator.transform.Find("BulletStartPoint");
        _bulletDirectionPoint = _gunAnimator.transform.Find("BulletDirectionPoint");

        _oldPosition = _newPosition = _transform.position;
        _oldRotation = _newRotation = _holder.eulerAngles.z;

        _serverMoveTime = 100;
    }

    public Vector2 GetLastFramePosition(out bool haveChange)
    {
        haveChange = _oldPosition != _newPosition;
        _oldPosition = _newPosition;
        return _newPosition;
    }

    public float GetLastFrameRotation(out bool haveChange)
    {
        haveChange = _oldRotation != _newRotation;
        _oldRotation = _newRotation;
        return _newRotation;
    }

    public void SetPosition(Vector3 position)
    {
        _transform.Translate(position * Time.fixedDeltaTime);
        //_transform.position = transform.position + (position * 0.1f);
        
        _newPosition = transform.position + (position * 0.1f);
        
        _feetAnimator.SetBool("Run", true);
        _bodyAnimator.SetBool("Run", true);
        _runTurnOffTime = 0.1f;
        
        Vector3 aimDirection = ((_transform.position + position) - _holder.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        float normalizedMoveAngle = NormalizeAngle(angle);
        float normalizedLookAngle = NormalizeAngle(_holder.eulerAngles.z);
        float diff = Math.Abs(normalizedMoveAngle - normalizedLookAngle);
        if ((diff >= 70 && diff <= 110) || (diff <= 290 && diff >= 250)) _feetAnimator.SetBool("Strafe", true);
        else _feetAnimator.SetBool("Strafe", false);
    }

    public void SetPositionFromServer(Vector3 position)
    {
        Vector3 aimDirection = (position - _holder.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        _oldPosition = _transform.position;
        _newPosition = position;
        _serverMoveTime = 0f;
        
        _feetAnimator.SetBool("Run", true);
        _bodyAnimator.SetBool("Run", true);
        _runTurnOffTime = 0.1f;
        
        float normalizedMoveAngle = NormalizeAngle(angle);
        float normalizedLookAngle = NormalizeAngle(_holder.eulerAngles.z);
        float diff = Math.Abs(normalizedMoveAngle - normalizedLookAngle);
        if ((diff >= 70 && diff <= 110) || (diff <= 290 && diff >= 250)) _feetAnimator.SetBool("Strafe", true);
        else _feetAnimator.SetBool("Strafe", false);
    }

    float NormalizeAngle(float angle)
    {
        if (angle >= 0) return angle;
        else return 180 + (180 + angle);
    }

    public void SetLookPosition(Vector3 position)
    {
        Vector3 aimDirection = (position - _holder.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        _holder.eulerAngles = new Vector3(0, 0, angle);

        _newRotation = angle;
    }
    
    public void SetServerLookPosition(float angle)
    {
        _oldRotation = _holder.eulerAngles.z;
        _newRotation = angle;
    }

    public void Shoot()
    {
        Vector3 endPoint = _bulletDirectionPoint.position;
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(_bulletStartPoint.position, endPoint, 50f);

        foreach (RaycastHit2D raycastHit2D in hits)
        {
            //if (!Blockers.Contains(raycastHit2D.collider.gameObject)) continue;
                    
            Debug.Log("Hit " + raycastHit2D.collider.name);
            endPoint = raycastHit2D.point;
            break;
        }
        
        Debug.DrawLine(_bulletStartPoint.position, endPoint, Color.green, 0.1f);
        
        _gunAnimator.SetTrigger("Shoot");
        UtilsClass.ShakeCamera(0.05f, 0.2f);
    }

    void FixedUpdate()
    {
        if (_serverMoveTime >= 0 && _serverMoveTime <= 1)
        {
            _transform.position = Vector3.Lerp(_oldPosition, _newPosition, _serverMoveTime);
            float angle = Mathf.LerpAngle(_oldRotation , _newRotation, _serverMoveTime);
            _holder.eulerAngles = new Vector3(0, 0, angle);
            //transform.eulerAngles = new Vector3(0, angle, 0);
            //_holder.rotation = Quaternion.Lerp(new Quaternion(0, 0, _oldRotation, 0), new Quaternion(0, 0, _newRotation, 0), _serverMoveTime);
            //_holder.eulerAngles = Vector3.Lerp(new Vector3(0,0, _oldRotation), new Vector3(0,0, _newRotation), _serverMoveTime);
            _serverMoveTime += Time.deltaTime * 10;
        }

        if (_runTurnOffTime > 0) _runTurnOffTime -= Time.deltaTime;
        else
        {
            _feetAnimator.SetBool("Run", false);
            _bodyAnimator.SetBool("Run", false);
            _feetAnimator.SetBool("Strafe", false);
        }
    }
}
