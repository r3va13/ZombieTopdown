using System;
using System.Collections;
using System.Collections.Generic;
using Elindery.Game;
using UnityEngine;

public class ItemsController : MonoBehaviour
{
#region Singleton
    static ItemsController _instance;
    public static ItemsController Instance
    {
        get
        {
            if (!_instance) _instance = GameController.Instance.transform.Find("ItemsController").GetComponent<ItemsController>();
            return _instance;
        }
    }
#endregion

    readonly Dictionary<string, TheItem> _createdItems = new Dictionary<string, TheItem>();

    [SerializeField] TheItem ItemPrefab;

    public void CreateItem(string[] args)
    {
        TheItem created = Instantiate(ItemPrefab, transform);
        created.ID = args[1];
        created.Type = (TheItem.ItemType)Enum.ToObject(typeof(TheItem.ItemType), Convert.ToInt32(args[2]));
        string[] positionArgs = args[3].Split('_');
        created.transform.position = new Vector3(Convert.ToSingle(positionArgs[0]), Convert.ToSingle(positionArgs[1]));
        _createdItems.Add(created.ID, created);
    }

    public void TakeItem(string[] args)
    {
        _createdItems[args[1]].TakeItem();
    }
}
