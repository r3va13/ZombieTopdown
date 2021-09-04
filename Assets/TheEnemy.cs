using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEnemy : ServerControlledUnit
{
    public ZombieConfig Config;

    TheCharacter _target;
    CircleCollider2D _collider;
    Vector3 _myPosition;
    Vector3 _targetPosition;
    float _targetDistance;

    Animator _feetAnimator;
    Animator _bodyHolderAnimator;
    Animator _bodyAnimator;

    int _stunDamage = 0;
    float _attackCooldownTimer = 0;
    float _stunTimer = 0;
    float _stunCooldownTimer = 0;

    bool _initialized = false;
    public override void Initialize()
    {
        base.Initialize();

        _collider = GetComponent<CircleCollider2D>();
        _feetAnimator = Holder.Find("Feet").GetComponent<Animator>();
        _bodyHolderAnimator = Holder.Find("BodyHolder").GetComponent<Animator>();
        _bodyAnimator = _bodyHolderAnimator.transform.Find("Body").GetComponent<Animator>();

        Health.HP = Config.HP;
        Health.MaxHP = Config.HP;
        Health.OnDamageEvent += OnDamage;

        _initialized = true;
    }

    protected override void FixedUpdate()
    {
        if (!_initialized) return;
        
        if (WalkTurnOffTime <= 0) _feetAnimator.SetBool("Walk", false);
        if (_attackCooldownTimer > 0) _attackCooldownTimer -= Time.deltaTime;
        if (_stunTimer > 0) _stunTimer -= Time.deltaTime;
        if (_stunCooldownTimer > 0) _stunCooldownTimer -= Time.deltaTime;
        
        base.FixedUpdate();

        if (GameController.ServerOk) return;
        
        NoServerFixedUpdate();
    }

    public override void SetServerPosition(Vector3 position)
    {
        base.SetServerPosition(position);
        
        _feetAnimator.SetBool("Walk", true);
    }

    public void SetStatus(string status)
    {
        switch (status)
        {
            case "dead":
                OnDie();
                break;
            case "stun":
                _bodyAnimator.SetTrigger("Damage");
                break;
        }
    }

    void OnDie()
    {
        _collider.enabled = false;
        _feetAnimator.gameObject.SetActive(false);
        _bodyHolderAnimator.enabled = false;
        _bodyAnimator.SetTrigger("Die");
        _bodyAnimator.transform.localPosition = new Vector3(0, 0, 0.001f);
        enabled = false;
    }

#region NoServer
    void NoServerFixedUpdate()
    {
        if (!_target) return;
        
        _myPosition = Transform.position;
        _targetPosition = _target.Transform.position;
        _targetDistance = Vector3.Distance(_myPosition, _targetPosition);
        
        
        FollowTarget();
        WatchAttacks();
    }
    

    public void SetTarget(TheCharacter targetCharacter)
    {
        _target = targetCharacter;
    }
    
    void WatchAttacks()
    {
        if (_stunTimer > 0) return;
        if (_attackCooldownTimer > 0) return;
        if (_targetDistance > Config.AttackDistance) return;
        
        _bodyAnimator.SetTrigger("Attack");
        _attackCooldownTimer = Config.AttackCooldown;
    }

    void FollowTarget()
    {
        if (_stunTimer > 0) return;
        if (_targetDistance < 2f) return;
        
        Vector3 aimDirection = (_targetPosition - _myPosition).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //Holder.eulerAngles = new Vector3(0, 0, angle);
        
        Rigidbody2D.MovePosition(Vector3.MoveTowards(_myPosition, _targetPosition, Config.WalkSpeed * Time.deltaTime));
        Rigidbody2D.MoveRotation(angle);
        
        _feetAnimator.SetBool("Walk", true);
        WalkTurnOffTime = 0.1f;
    }
    
    void OnDamage(int damage)
    {
        if (Health.HP > 0)
        {
            _stunDamage += damage;
            if (_stunDamage >= Config.StunDamage)
            {
                _stunDamage = 0;
                _stunTimer = 1f;
                _stunCooldownTimer = Config.StunCooldown;
                WalkTurnOffTime = 0;
                _bodyAnimator.SetTrigger("Damage");
            }
        }
        else
        {
            OnDie();
        }
    }
#endregion
    
   

    
}
