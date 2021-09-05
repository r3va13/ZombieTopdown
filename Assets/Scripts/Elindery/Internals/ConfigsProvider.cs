
using Elindery.Configs;
using UnityEngine;

namespace Elindery.Internals
{
    public static class ConfigsProvider
    {
        public static SurvivorConfig[] SurvivorConfigs;
        public static WeaponConfig[] WeaponConfigs;
        public static ZombieConfig[] ZombieConfigs;
        
        public static void Initialize()
        {
            SurvivorConfigs = Resources.LoadAll<SurvivorConfig>("Configs/Survivors");
            WeaponConfigs = Resources.LoadAll<WeaponConfig>("Configs/Weapons");
            ZombieConfigs = Resources.LoadAll<ZombieConfig>("Configs/Zombies");
        }
    }
}
