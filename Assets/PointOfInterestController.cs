using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestController : MonoBehaviour
{
#region Singleton
    static PointOfInterestController _instance;
    public static PointOfInterestController Instance
    {
        get
        {
            if (!_instance) _instance = GameObject.Find("GameUI").GetComponent<PointOfInterestController>();
            return _instance;
        }
    }
#endregion

    [SerializeField] PoiMarker PoiMarkerPrefab;

    readonly List<PoiMarker> _createdPoi = new List<PoiMarker>();

    public static float ScreenHeight { get; private set; }
    public static float ScreenWidth { get; private set; }
    
    void Start()
    {
        ScreenHeight = Screen.height;
        ScreenWidth = Screen.width;
    }

    public void CreatePoi()
    {
        
    }
}
