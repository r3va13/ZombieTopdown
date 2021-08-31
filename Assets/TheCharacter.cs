using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class TheCharacter : ServerControlledUnit
{
    public LayerMask RaycastLayers;
    public WeaponConfig WeaponConfig;
    
    Transform _bulletStartPoint, _bulletDirectionPoint;
    Animator _gunAnimator, _bodyAnimator, _feetAnimator;
    
    float _weaponFireRateTimer;
    float _weaponReloadTimer;
    int _weaponAmmo;
    
    public override void Initialize()
    {
        base.Initialize();
        
        _bodyAnimator = Holder.Find("Body").GetComponent<Animator>();
        _gunAnimator = _bodyAnimator.transform.Find("Gun").GetComponent<Animator>();
        _feetAnimator = Holder.Find("Feet").GetComponent<Animator>();

        _bulletStartPoint = _gunAnimator.transform.Find("BulletStartPoint");
        _bulletDirectionPoint = _gunAnimator.transform.Find("BulletDirectionPoint");

        _weaponAmmo = WeaponConfig.LoadAmmo;
    }

    public Vector2 GetLastFramePosition(out bool haveChange)
    {
        haveChange = OldPosition != NewPosition;
        OldPosition = NewPosition;
        return NewPosition;
    }

    public float GetLastFrameRotation(out bool haveChange)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        haveChange = OldRotation != NewRotation;
        OldRotation = NewRotation;
        return NewRotation;
    }

    public void SetPosition(Vector3 position)
    {
        Transform.Translate(position * Time.fixedDeltaTime);
        
        NewPosition = transform.position + (position * 0.1f);
        
        _feetAnimator.SetBool("Run", true);
        _bodyAnimator.SetBool("Run", true);
        
        Vector3 aimDirection = ((Transform.position + position) - Holder.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        float normalizedMoveAngle = Utils.NormalizeAngle(angle);
        float normalizedLookAngle = Utils.NormalizeAngle(Holder.eulerAngles.z);
        float diff = Math.Abs(normalizedMoveAngle - normalizedLookAngle);
        if ((diff >= 70 && diff <= 110) || (diff <= 290 && diff >= 250)) _feetAnimator.SetBool("Strafe", true);
        else _feetAnimator.SetBool("Strafe", false);
    }

    public override void SetPositionFromServer(Vector3 position)
    {
        Vector3 aimDirection = (position - Holder.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        base.SetPositionFromServer(position);
        
        _feetAnimator.SetBool("Run", true);
        _bodyAnimator.SetBool("Run", true);
        
        float normalizedMoveAngle = Utils.NormalizeAngle(angle);
        float normalizedLookAngle = Utils.NormalizeAngle(Holder.eulerAngles.z);
        float diff = Math.Abs(normalizedMoveAngle - normalizedLookAngle);
        if ((diff >= 70 && diff <= 110) || (diff <= 290 && diff >= 250)) _feetAnimator.SetBool("Strafe", true);
        else _feetAnimator.SetBool("Strafe", false);
    }

    public void SetLookPosition(Vector3 position)
    {
        Vector3 aimDirection = (position - Holder.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        Holder.eulerAngles = new Vector3(0, 0, angle);

        NewRotation = angle;
    }
    
    public void Shoot()
    {
        if (_weaponFireRateTimer > 0) return;
        if (_weaponReloadTimer > 0) return;
        if (_weaponAmmo <= 0) return;
        
        Vector3 endPoint = _bulletDirectionPoint.position;

        RaycastHit2D hit = Physics2D.Linecast(_bulletStartPoint.position, endPoint, RaycastLayers);
        if (hit)
        {
            endPoint = hit.point;
            TheHealth health = hit.transform.GetComponent<TheHealth>();
            if (health) health.DoDamage(WeaponConfig.Damage);
            Debug.DrawLine(_bulletStartPoint.position, endPoint, Color.black, 0.25f);
        }

        Utils.CreateBulletTracer(_bulletStartPoint.position, endPoint);
        
        _gunAnimator.SetTrigger("Shoot");
        UtilsClass.ShakeCamera(0.05f, 0.2f);

        _weaponFireRateTimer = WeaponConfig.FireRate;
        _weaponAmmo--;

        if (_weaponAmmo <= 0) _weaponReloadTimer = WeaponConfig.ReloadTime;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (WalkTurnOffTime <= 0)
        {
            _feetAnimator.SetBool("Run", false);
            _bodyAnimator.SetBool("Run", false);
            _feetAnimator.SetBool("Strafe", false);
        }

        if (_weaponFireRateTimer >= 0) _weaponFireRateTimer -= Time.deltaTime;
        if (_weaponReloadTimer > 0) _weaponReloadTimer -= Time.deltaTime;
        if (_weaponReloadTimer < 0)
        {
            _weaponAmmo = WeaponConfig.LoadAmmo;
            _weaponReloadTimer = 0;
        }
    }
}
