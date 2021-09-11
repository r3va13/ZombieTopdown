using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheItem : MonoBehaviour
{
    public string ID;

    public enum ItemType
    {
        None,
        Ammo,
        Healing,
        Revive,
        Money
    }

    public ItemType Type;
    
    public void TakeItem()
    {
        gameObject.SetActive(false);
    }
}
