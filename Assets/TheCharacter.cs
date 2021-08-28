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
    Animator _gunAnimator;
    Animator _bodyAnimator;
    Animator _feetAnimator;
    float _runTurnOffTime;

    public void Initialize()
    {
        _transform = transform;
        _holder = transform.Find("Holder");
        
        _bodyAnimator = _holder.Find("Body").GetComponent<Animator>();
        _gunAnimator = _bodyAnimator.transform.Find("Gun").GetComponent<Animator>();
        _feetAnimator = _holder.Find("Feet").GetComponent<Animator>();
    }

    public void SetPosition(Vector3 position)
    {
        transform.Translate(position * Time.fixedDeltaTime);
        _feetAnimator.SetBool("Run", true);
        _bodyAnimator.SetBool("Run", true);
        _runTurnOffTime = 0.1f;
        
        Vector3 aimDirection = ((transform.position + position) - _holder.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //_holder.eulerAngles = new Vector3(0, 0, angle);
        //Debug.Log(NormalizeAngle(angle));

        float normalizedMoveAngle = NormalizeAngle(angle);
        float normalizedLookAngle = NormalizeAngle(_holder.eulerAngles.z);
        float diff = Math.Abs(normalizedMoveAngle - normalizedLookAngle);
        if ((diff >= 70 && diff <= 110) || (diff <= 290 && diff >= 250)) _feetAnimator.SetBool("Strafe", true);
        else _feetAnimator.SetBool("Strafe", false);
        
        Debug.Log(normalizedMoveAngle + "|" + normalizedLookAngle + "|" + diff);
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
    }

    public void Shoot()
    {
        _gunAnimator.SetTrigger("Shoot");
        UtilsClass.ShakeCamera(0.05f, 0.2f);
    }

    void FixedUpdate()
    {
        if (_runTurnOffTime > 0) _runTurnOffTime -= Time.deltaTime;
        else
        {
            _feetAnimator.SetBool("Run", false);
            _bodyAnimator.SetBool("Run", false);
            _feetAnimator.SetBool("Strafe", false);
        }
    }
}
