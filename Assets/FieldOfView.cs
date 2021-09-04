using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    TheEnemy _enemy;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameController.ServerOk)
        {
            gameObject.SetActive(false);
            return;
        }
        //Debug.Log(other.name);
        
        if (!_enemy) _enemy = transform.parent.GetComponent<TheEnemy>();

        if (other.CompareTag("Survivor")) _enemy.SetTarget(other.GetComponent<TheCharacter>());
    }
}
