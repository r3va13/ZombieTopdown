using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/Weapon")]
public class WeaponConfig : ScriptableObject
{
   public int Damage;
   public float FireRate;
   public int LoadAmmo;
   public int MaxAmmo;
   public float ReloadTime;
}

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

      _loadAmmo = _loadAmmoMax;
      _totalAmmo = _holdAmmoMax;
   }
   
   int _loadAmmo;
   int _totalAmmo;

   public void TryShoot(Action onSuccsessShoot)
   {
      switch (_state)
      {
         case State.Ready:
            if (_loadAmmo <= 0) return;
            
            _loadAmmo--;
            _totalAmmo--;
            
            onSuccsessShoot.Invoke();
            
            FireRate();
            if (_loadAmmo <= 0 && _totalAmmo > 0) Reload();
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
      _loadAmmo = _totalAmmo >= _loadAmmoMax ? _loadAmmoMax : _totalAmmo;
      _state = State.Ready;
   }
}
