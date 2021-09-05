using UnityEngine;

namespace Elindery.Configs
{
   [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/Weapon")]
   public class WeaponConfig : ScriptableObject
   {
      public int Damage = 5;
      public float FireRate = 0.25f;
      public int LoadAmmo = 30;
      public int MaxAmmo = 150;
      public float ReloadTime = 2;
   }
}


