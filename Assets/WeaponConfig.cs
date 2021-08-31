using System.Collections;
using System.Collections.Generic;
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
