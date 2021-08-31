using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiesController : MonoBehaviour
{
#region Singleton
    static ZombiesController _instance;
    public static ZombiesController Instance
    {
        get
        {
            if (!_instance) _instance = GameObject.Find("Game").transform.Find("Zombies").GetComponent<ZombiesController>();
            return _instance;
        }
    }
#endregion

    public TheEnemy EasyZombiePrefab;
    
    Dictionary<int, TheEnemy> _createdEnemys = new Dictionary<int, TheEnemy>();

    public void OnLocalGameStart()
    {
        int zombieCount = 100;

        for (int i = 0; i < zombieCount; i++)
        {
            int randX = Random.Range(5, 195);
            int randY = Random.Range(-95, 95);
            int randRotation = Random.Range(0, 359);

            TheEnemy created = Instantiate(EasyZombiePrefab.gameObject, transform).GetComponent<TheEnemy>();
            created.Initialize();
            Transform createdTransform = created.transform;
            createdTransform.localPosition = new Vector3(randX, randY, 0);
            createdTransform.eulerAngles = new Vector3(0, 0,  randRotation);
            _createdEnemys.Add(i, created);
        }
    }
}
