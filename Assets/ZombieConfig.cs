using UnityEngine;

[CreateAssetMenu(fileName = "ZombieConfig", menuName = "Configs/Zombie")]
public class ZombieConfig : ScriptableObject
{
    public int HP = 20;
    public float WalkSpeed = 5f;
    public float AttackCooldown = 1.5f;
    public int AttackDamage = 3;
    public float AttackDistance = 2f;
    public int StunDamage = 12;
    public float StunCooldown = 3f;
}
