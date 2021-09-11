using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheContainer : MonoBehaviour
{
    public string ID;

    float _openTime;

    GameObject _timerBar;
    Transform _fill;
    SpriteRenderer _renderer;
    
    public void Initialize(Vector2 position)
    {
        _timerBar = transform.Find("TimerBar").gameObject;
        _fill = _timerBar.transform.Find("Fill");
        _renderer = GetComponent<SpriteRenderer>();
        _openTime = 0;
        transform.position = position;
    }

    public void SetOpenState(float time)
    {
        _openTime = time;
        _timerBar.SetActive(_openTime > 0);
        _fill.localScale = new Vector3(_openTime, 1, 1);
    }

    public void SetOpened()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        _renderer.color = new Color(1, 1, 1, 0.25f);
        SetOpenState(0);
    }
}
