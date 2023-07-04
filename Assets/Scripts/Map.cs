using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Maps.Unity;
using Microsoft.Geospatial;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// using Microsoft.MixedReality.Toolkit;

/// <summary>
/// The Class switches the map display to the desired location while it is running.
/// </summary>

public class Map : MonoBehaviour
{
    [SerializeField]
    MapRenderer _mapRenderer;
    public static JToken entity;
    public float Lat;
    public float Lng; 
    // Start is called before the first frame update
    private void Awake() {
        Lat = (float)entity["MekomNefilaLat"];
        Lng = (float)entity["MekomNefilaLng"];
        print(entity);
    }
    void Start()
    {
       
        _mapRenderer = GameObject.Find("Map").GetComponent<MapRenderer>();
       Nefila();
    
    }
    //Ecerest
   
    //Mt.Fuji
    public void Nefila()
    {
        // 31.776940839643327, 35.23509881132895
        MapChanger( Lat,Lng,14.5f);
    }
    //Elbrus
    
    public void MapChanger(float lat ,float lon,float zoom)
    {
        _mapRenderer.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(lat, lon), zoom));
    }
}