using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class TheCharacter : ServerControlledUnit
{
    public LayerMask RaycastLayers;
    Weapon _weapon;
    
    Transform _bulletStartPoint, _bulletDirectionPoint;
    Animator _gunAnimator, _bodyAnimator, _feetAnimator;
    
    public override void Initialize()
    {
        base.Initialize();
        
        _bodyAnimator = Holder.Find("Body").GetComponent<Animator>();
        _gunAnimator = _bodyAnimator.transform.Find("Gun").GetComponent<Animator>();
        _feetAnimator = Holder.Find("Feet").GetComponent<Animator>();

        _bulletStartPoint = _gunAnimator.transform.Find("BulletStartPoint");
        _bulletDirectionPoint = _gunAnimator.transform.Find("BulletDirectionPoint");
    }

    public void SetWeapon(string configArgs)
    {
        string[] args = configArgs.Split('_');
        
        _weapon = new Weapon(Convert.ToInt32(args[0]),
            Convert.ToSingle(args[1]),
            Convert.ToInt32(args[2]),
            Convert.ToInt32(args[3]),
            Convert.ToSingle(args[4]));
    }

    public void ShootFromServer(string directionString)
    {
        string[] args = directionString.Split('_');

        AnimateShoot(new Vector2(Convert.ToSingle(args[0]), Convert.ToSingle(args[1])));
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
        Vector3 movePosition = Transform.position + (position * Time.fixedDeltaTime);
        Rigidbody2D.MovePosition(movePosition);
        
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

    public void ClearPredictMoving()
    {
        NewPosition = transform.position;
    }

    public override void SetServerPosition(Vector3 position)
    {
        Vector3 aimDirection = (position - Holder.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        base.SetServerPosition(position);
        
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
        Rigidbody2D.MoveRotation(angle);

        NewRotation = angle;
    }
    
    public void ShootLocal()
    {
        _weapon.TryShoot(OnSuccsessShoot);

        void OnSuccsessShoot()
        {
            Vector3 endPoint = _bulletDirectionPoint.position;

            if (!GameController.ServerOk)
            {
                RaycastHit2D hit = Physics2D.Linecast(_bulletStartPoint.position, endPoint, RaycastLayers);
                if (hit)
                {
                    endPoint = hit.point;
                    TheHealth health = hit.transform.GetComponent<TheHealth>();
                    if (health) health.DoDamage(_weapon.Damage);
                    //Debug.DrawLine(_bulletStartPoint.position, endPoint, Color.black, 0.25f);
                }

                AnimateShoot(endPoint);
            }
            else
            {
                ClientServerController.Instance.Send("player_shoot|" + ClientID + "|" + endPoint.x + "_" + endPoint.y);
            }
        }
    }

    void AnimateShoot(Vector2 endPoint)
    {
        Utils.CreateBulletTracer(_bulletStartPoint.position, endPoint);
        UtilsClass.ShakeCamera(0.05f, 0.2f);
        _gunAnimator.SetTrigger("Shoot");
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
    }
}
