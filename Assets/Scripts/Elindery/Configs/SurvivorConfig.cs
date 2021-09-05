using UnityEngine;

namespace Elindery.Configs
{
    [CreateAssetMenu(fileName = "SurvivorConfig", menuName = "Configs/Survivor")]
    public class SurvivorConfig : ScriptableObject
    {
        // ReSharper disable once InconsistentNaming
        public int HP = 30;
        public float MoveSpeed = 10f;
    }
}
