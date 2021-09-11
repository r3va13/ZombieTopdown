using System;
using System.Collections;
using System.Collections.Generic;
using Elindery.Game;
using UnityEngine;

public class ContainerController : MonoBehaviour
{
#region Singleton
    static ContainerController _instance;
    public static ContainerController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameController.Instance.transform.Find("ContainerController").GetComponent<ContainerController>();
                _instance.Initialize();
            }
            return _instance;
        }
    }
#endregion

    readonly Dictionary<string, TheContainer> _createdContainers = new Dictionary<string, TheContainer>();
    
    void Initialize()
    {
        TheContainer[] containers = GetComponentsInChildren<TheContainer>();
        foreach (TheContainer container in containers) _createdContainers.Add(container.ID, container);
    }

    public void SetContainers(string[] args)
    {
        for (int i = 1; i < args.Length; i++)
        {
            string[] split = args[i].Split('_');
            _createdContainers[split[0]].Initialize(new Vector2(Convert.ToSingle(split[1]), Convert.ToSingle(split[2])));
        }
    }

    public void SetContainerStates(string[] args)
    {
        for (int i = 1; i < args.Length; i++)
        {
            string[] split = args[i].Split('_');
            _createdContainers[split[0]].SetOpenState(Convert.ToSingle(split[1]));
        }
    }

    public void SetContainerOpened(string[] args)
    {
        _createdContainers[args[1]].SetOpened();
    }
}
