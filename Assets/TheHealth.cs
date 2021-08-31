using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheHealth : MonoBehaviour
{
    public delegate void OnDamage(int damage);
    public event OnDamage OnDamageEvent;
    protected virtual void CallOnDamageEvent(int damage)
    {
        OnDamageEvent?.Invoke(damage);
    }
    
    public enum TeamSide
    {
        Survivor,
        Zombie
    }

    public TeamSide Side;

    public int MaxHP;
    public int HP;

    public void DoDamage(int damage)
    {
        HP -= damage;
        CallOnDamageEvent(damage);
    }
}
