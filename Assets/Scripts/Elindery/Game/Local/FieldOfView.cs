using System;
using UnityEngine;

namespace Elindery.Game.Local
{
    public class FieldOfView : MonoBehaviour
    {
        TheEnemy _enemy;

        void Start()
        {
            if (GameController.ServerOk) gameObject.SetActive(false);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!_enemy) _enemy = transform.parent.GetComponent<TheEnemy>();

            if (other.CompareTag("Survivor")) _enemy.SetTarget(other.GetComponent<TheSurvivor>());
        }
    }
}
