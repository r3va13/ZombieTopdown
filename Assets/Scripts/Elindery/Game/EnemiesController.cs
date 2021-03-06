using System;
using System.Collections.Generic;
using Elindery.Internals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Elindery.Game
{
    public class EnemiesController : MonoBehaviour
    {
#region Singleton
        static EnemiesController _instance;
        public static EnemiesController Instance
        {
            get
            {
                if (!_instance) _instance = GameObject.Find("Game").transform.Find("Zombies").GetComponent<EnemiesController>();
                return _instance;
            }
        }
#endregion

        [SerializeField] TheEnemy EasyZombiePrefab;
    
        readonly Dictionary<string, TheEnemy> _createdEnemys = new Dictionary<string, TheEnemy>();
    
        bool _initialized;

        TheEnemy CreateEnemy(string id, float posX, float posY, float lookAngle)
        {
            TheEnemy created = Instantiate(EasyZombiePrefab.gameObject, transform).GetComponent<TheEnemy>();
            created.Initialize(ConfigsProvider.ZombieConfigs[0]);
            created.InitializePositionFromServer(new Vector3(posX, posY));
            created.InitializeLookDirectionFromServer(lookAngle);
            _createdEnemys.Add(id, created);
            return created;
        }

        void SetEnemyState(string id, float posX, float posY, float lookAngle)
        {
            _createdEnemys[id].SetServerPosition(new Vector3(posX, posY));
            _createdEnemys[id].SetServerLookAngle(lookAngle);
        }

        public void SetZombieStates(string[] args)
        {
            for (int i = 1; i < args.Length; i++)
            {
                string[] zArgs = args[i].Split('_');

                string id = zArgs[0];
                float posX = Convert.ToSingle(zArgs[1]);
                float posY = Convert.ToSingle(zArgs[2]);
                float lookAngle = Convert.ToSingle(zArgs[3]);

                if (!_initialized) CreateEnemy(id, posX, posY, lookAngle);
                else SetEnemyState(id, posX, posY, lookAngle);
            }
        
            _initialized = true;
        }

        public void SetZombieStatus(string[] args)
        {
            _createdEnemys[args[1]].SetStatus(args[2]);
        }

#region Local

        public void OnLocalGameStart()
        {
            int zombieCount = 100;

            for (int i = 0; i < zombieCount; i++)
            {
                int randX = Random.Range(5, 195);
                int randY = Random.Range(-95, 95);
                int randRotation = Random.Range(0, 359);

                CreateEnemy(i.ToString(), randX, randY, randRotation);
            }
        }

#endregion
    
    }
}
