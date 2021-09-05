using Elindery.Configs;
using UnityEngine;

namespace Elindery.Game
{
    public class TheEnemy : ServerControlledUnit
    {
        ZombieConfig _zombieConfig;

        TheSurvivor _target;
        CircleCollider2D _collider;
        Vector3 _myPosition;
        Vector3 _targetPosition;
        float _targetDistance;

        Animator _feetAnimator;
        Animator _bodyHolderAnimator;
        Animator _bodyAnimator;

        float _attackCooldownTimer = 0;

        bool _initialized = false;
        public void Initialize(ZombieConfig zombieConfig)
        {
            base.Initialize();

            _collider = GetComponent<CircleCollider2D>();
            _feetAnimator = Holder.Find("Feet").GetComponent<Animator>();
            _bodyHolderAnimator = Holder.Find("BodyHolder").GetComponent<Animator>();
            _bodyAnimator = _bodyHolderAnimator.transform.Find("Body").GetComponent<Animator>();

            _zombieConfig = zombieConfig;
        
            Health = new Health(zombieConfig.HP, zombieConfig.StunDamage);
            Health.OnDie += OnDie;
            Health.OnStun += OnStun;

            _initialized = true;
        }

        protected override void FixedUpdate()
        {
            if (!_initialized) return;
        
            if (WalkTurnOffTime <= 0) _feetAnimator.SetBool("Walk", false);
            if (_attackCooldownTimer > 0) _attackCooldownTimer -= Time.deltaTime;
        
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
                    OnStun();
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

        void OnStun()
        {
            WalkTurnOffTime = 0;
            _bodyAnimator.SetTrigger("Damage");
        }

#region NoServer
        void NoServerFixedUpdate()
        {
            if (!_target) return;
            if (Health.Condition == Health.ConditionState.Stun) return;
        
            _myPosition = Transform.position;
            _targetPosition = _target.Transform.position;
            _targetDistance = Vector3.Distance(_myPosition, _targetPosition);
        
        
            FollowTarget();
            WatchAttacks();
        }

        public void SetTarget(TheSurvivor targetSurvivor)
        {
            _target = targetSurvivor;
        }
    
        void WatchAttacks()
        {
            if (_attackCooldownTimer > 0) return;
            if (_targetDistance > _zombieConfig.AttackDistance) return;
        
            _bodyAnimator.SetTrigger("Attack");
            _attackCooldownTimer = _zombieConfig.AttackCooldown;
        }

        void FollowTarget()
        {
            if (_targetDistance < 2f) return;
        
            Vector3 aimDirection = (_targetPosition - _myPosition).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
            Rigidbody2D.MovePosition(Vector3.MoveTowards(_myPosition, _targetPosition, _zombieConfig.WalkSpeed * Time.deltaTime));
            Rigidbody2D.MoveRotation(angle);
        
            _feetAnimator.SetBool("Walk", true);
            WalkTurnOffTime = 0.1f;
        }

        public void DoDamage(int damage)
        {
            Health.DoDamage(damage);
        }
#endregion
    
   

    
    }
}
