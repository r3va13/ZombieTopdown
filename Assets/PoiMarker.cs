using System;
using System.Collections;
using System.Collections.Generic;
using Elindery.Game;
using TMPro;
using UnityEngine;

public class PoiMarker : MonoBehaviour
{
    public Transform PointOfInterest;
    TMP_Text _distanceLbl;
    [SerializeField] TheSurvivor _survivor;

    Camera _camera;
    
    void Start()
    {
        _camera = Camera.main;
        _distanceLbl = transform.Find("Text").GetComponent<TMP_Text>();
    }

    void FixedUpdate()
    {
        if (!_survivor) return;

        Vector3 poiPosition = PointOfInterest.position;
        
        float distance = Vector3.Distance(_survivor.Transform.position, poiPosition);
        _distanceLbl.text = Mathf.CeilToInt(distance).ToString();

        Vector3 position = _camera.WorldToScreenPoint(poiPosition);
        
        float x = position.x < 0 ? 0 : position.x;
        if (x > PointOfInterestController.ScreenWidth) x = PointOfInterestController.ScreenWidth;
        float y = position.y < 0 ? 0 : position.y;
        if (y > PointOfInterestController.ScreenHeight) y = PointOfInterestController.ScreenHeight;
        
        transform.position = new Vector3(x, y);
    }
}
