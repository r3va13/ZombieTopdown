using System;
using System.Collections.Generic;
using Elindery.Internals;
using UnityEngine;

namespace Elindery.Game
{
    public class SurvivorsController : MonoBehaviour
    {
#region Singleton
        static SurvivorsController _instance;
        public static SurvivorsController Instance
        {
            get
            {
                if (!_instance) _instance = GameObject.Find("Game").transform.Find("Survivors").GetComponent<SurvivorsController>();
                return _instance;
            }
        }
#endregion

        [SerializeField] TheSurvivor SurvivorPrefab;

        readonly Dictionary<string, TheSurvivor> _survivors = new Dictionary<string, TheSurvivor>();
    
        public TheSurvivor CreateSurvivor(string clientID, Vector2 position, int hp)
        {
            TheSurvivor created = Instantiate(SurvivorPrefab, transform);
            created.Initialize(hp);
            created.InitializePositionFromServer(position);
            _survivors.Add(clientID, created);
            created.ClientID = clientID;
            return created;
        }
    
        public void SetWeapon(string[] args)
        {
            _survivors[args[1]].SetWeaponFromServer(args[2]);
        }

        public void UserStates(string[] args)
        {
            for (int i = 1; i < args.Length; i++)
            {
                UserMove(args[i]);
            }
        }

        public void UserShoot(string[] args)
        {
            _survivors[args[1]].ShootFromServer(args[2]);
        }

        void UserMove(string userLine)
        {
            string[] args = userLine.Split('_');
        
            if (!_survivors.ContainsKey(args[0])) return;
            if (Storage.ClientID == args[0]) return;

            float posX = Convert.ToSingle(args[1]);
            float posY = Convert.ToSingle(args[2]);
            float rotation = Convert.ToSingle(args[3]);
            _survivors[args[0]].SetServerLookAngle(rotation);
            _survivors[args[0]].SetServerPosition(new Vector3(posX, posY, 0));
        }
    }
}
