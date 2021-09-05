using System;
using System.Threading.Tasks;

namespace Elindery.Game
{
    public class Health
    {
        public int Hp { get; private set; }
        int _stunPoints;
        public int MaxHp { get; }
        readonly int _stunLimit;
    
        public enum ConditionState
        {
            Good,
            Dead,
            Stun
        }
    
        public ConditionState Condition = ConditionState.Good;
    
        public Action OnDie;
        public Action OnStun;
        public event EventHandler<int> OnDamage;
    
        public Health(int maxHp, int stunLimit)
        {
            Hp = MaxHp = maxHp;
            _stunLimit = stunLimit;
        }
    
        public void DoDamage(int damage)
        {
            Hp -= damage;
            _stunPoints += damage;
            OnDamage?.Invoke(this, damage);

            if (Hp <= 0)
            {
                Condition = ConditionState.Dead;
                OnDie?.Invoke();
            }
            else ProduceStun();
        }
    
        async void ProduceStun()
        {
            if (_stunLimit == 0) return;
            if (_stunPoints < _stunLimit) return; 
            _stunPoints -= _stunLimit;
            Condition = ConditionState.Stun;
            OnStun?.Invoke();
            await Task.Delay(1000);
            Condition = ConditionState.Good;
        }
    }
}
