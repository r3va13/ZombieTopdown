using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Elindery.Game
{
    public class Weapon
    {
        public int Damage { get; private set; }
        readonly float _fireRate;
        readonly int _loadAmmoMax;
        readonly int _holdAmmoMax;
        readonly float _reloadTime;
   
        enum State
        {
            Ready,
            FireRateTimeout,
            Reloading
        }

        State _state = State.Ready;
   
        public Weapon(int damage, float fireRate, int loadAmmoMax, int holdAmmoMax, float reloadTime)
        {
            Damage = damage;
            _fireRate = fireRate;
            _loadAmmoMax = loadAmmoMax;
            _holdAmmoMax = holdAmmoMax;
            _reloadTime = reloadTime;

            LoadAmmo = _loadAmmoMax;
            HoldAmmo = _holdAmmoMax;
        }

        public int LoadAmmo { get; private set; }
        public int HoldAmmo { get; private set; }
        
        public Action OnAmmoChanged;

        public void TryShoot(Action onSuccsessShoot)
        {
            switch (_state)
            {
                case State.Ready:
                    if (LoadAmmo <= 0) return;
            
                    LoadAmmo--;
            
                    OnAmmoChanged?.Invoke();
                    onSuccsessShoot.Invoke();
            
                    FireRate();
                    if (LoadAmmo <= 0 && HoldAmmo > 0) Reload();
                    break;
            }
        }

        async void FireRate()
        {
            _state = State.FireRateTimeout;
            await Task.Delay(Mathf.CeilToInt(_fireRate * 1000));
            if (_state == State.FireRateTimeout) _state = State.Ready;
        }

        async void Reload()
        {
            _state = State.Reloading;
            await Task.Delay(Mathf.CeilToInt(_reloadTime * 1000));
            if (HoldAmmo >= _loadAmmoMax)
            {
                LoadAmmo = _loadAmmoMax;
                HoldAmmo -= _loadAmmoMax;
            }
            else
            {
                LoadAmmo = HoldAmmo;
                HoldAmmo = 0;
            }
            
            OnAmmoChanged?.Invoke();
            _state = State.Ready;
        }
    }
}
