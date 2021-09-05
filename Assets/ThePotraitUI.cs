using System.Collections;
using System.Collections.Generic;
using Elindery.Game;
using UnityEngine;
using UnityEngine.UI;

public class ThePotraitUI : MonoBehaviour
{
    RectTransform _rectTransform;
    Image _healthFill;

    TheSurvivor _survivor;

    public void Initialize(TheSurvivor survivor)
    {
        _rectTransform = GetComponent<RectTransform>();
        _healthFill = transform.Find("Bar").Find("Fill").GetComponent<Image>();
        
        _survivor = survivor;
        _survivor.Health.OnDamage += OnDamage;
        
        ShowCurrentHealth();

        CameraController.OnFollowedChanged += OnCameraFollowingChange;
    }

    void ShowCurrentHealth()
    {
        _healthFill.fillAmount = (float) _survivor.Health.Hp / _survivor.Health.MaxHp;
    }

    void OnDamage(object sender, int damage)
    {
        ShowCurrentHealth();
    }

    void OnCameraFollowingChange(object sender, Transform followTarget)
    {
        float height = Screen.height * 0.22f;
        float windth = height * 0.8f;
        _rectTransform.sizeDelta = followTarget == _survivor.Transform ? new Vector2(windth, height) : new Vector2(windth * 0.6f, height * 0.6f);
    }
}
