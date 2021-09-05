using System;
using CodeMonkey.Utils;
using Elindery.Configs;
using Elindery.Internals;
using UnityEngine;

namespace Elindery.Game
{
    public class TheSurvivor : ServerControlledUnit
    {
        [SerializeField] LayerMask ShootRaycastLayers;
        public Weapon Weapon { get; private set; }

        Transform _bulletStartPoint, _bulletDirectionPoint;
        Animator _gunAnimator, _bodyAnimator, _feetAnimator;

        public Action OnWeaponChange;

        public void Initialize(int hp)
        {
            base.Initialize();
        
            _bodyAnimator = Holder.Find("Body").GetComponent<Animator>();
            _gunAnimator = _bodyAnimator.transform.Find("Gun").GetComponent<Animator>();
            _feetAnimator = Holder.Find("Feet").GetComponent<Animator>();

            _bulletStartPoint = _gunAnimator.transform.Find("BulletStartPoint");
            _bulletDirectionPoint = _gunAnimator.transform.Find("BulletDirectionPoint");
        
            Health = new Health(hp, 0);
        }

        public void SetWeaponFromServer(string configArgs)
        {
            string[] args = configArgs.Split('_');
        
            Weapon = new Weapon(Convert.ToInt32(args[0]),
                Convert.ToSingle(args[1]),
                Convert.ToInt32(args[2]),
                Convert.ToInt32(args[3]),
                Convert.ToSingle(args[4]));
            
            OnWeaponChange?.Invoke();
        }

        public void SetWeaponLocal(WeaponConfig weaponConfig)
        {
            Weapon = new Weapon(weaponConfig.Damage,
                weaponConfig.FireRate,
                weaponConfig.LoadAmmo,
                weaponConfig.MaxAmmo,
                weaponConfig.ReloadTime);
            
            OnWeaponChange?.Invoke();
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
    
        public void ClearPredictMovement()
        {
            NewPosition = transform.position;
        }

        public void SetPlayerPosition(Vector3 position)
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
    
        public void PlayerShoot()
        {
            Weapon.TryShoot(OnSuccsessShoot);

            void OnSuccsessShoot()
            {
                Vector3 endPoint = _bulletDirectionPoint.position;

                if (!GameController.ServerOk)
                {
                    RaycastHit2D hit = Physics2D.Linecast(_bulletStartPoint.position, endPoint, ShootRaycastLayers);
                    if (hit)
                    {
                        endPoint = hit.point;
                        TheEnemy enemy = hit.transform.GetComponent<TheEnemy>();
                        if (enemy) enemy.DoDamage(Weapon.Damage);
                    }

                    AnimateShoot(endPoint);
                }
                else
                {
                    Client.Client.Send("player_shoot|" + ClientID + "|" + endPoint.x + "_" + endPoint.y);
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

            if (WalkTurnOffTime > 0) return;
        
            _feetAnimator.SetBool("Run", false);
            _bodyAnimator.SetBool("Run", false);
            _feetAnimator.SetBool("Strafe", false);
        }
    }
}
