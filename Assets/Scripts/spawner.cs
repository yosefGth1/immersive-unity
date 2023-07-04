using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class spawner : MonoBehaviour
{
    // Start is called before the first frame update
    ///public static string halalNamex;
    public static RawImage imgx;
    public static JToken ENTITY;
    public RawImage newImgx;
    Vector3 pos = new Vector3(1,1,2);
    // float x = -150f;
    // float y = 30f;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Date;
    public TextMeshProUGUI Biography;
    private long DateNefila;
    private long DateLeda;



    void Start() 
    {

        /* DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)ENTITY["DateNefila"] / 1000);
         DateTime dateTime = dateTimeOffset.UtcDateTime;
         string formattedDate = dateTime.ToString("yyyy"); //"yyyy-MM-dd"
        */

       /* if ((ENTITY["DateLeda"].Type) != JTokenType.Null)
        {
            //this.DateLeda = (long)ENTITY["DateLeda"];
        }
        else if ((ENTITY["DateNefila"].Type) != JTokenType.Null)
        {
            this.DateNefila = (long)ENTITY["DateNefila"];
        }
        else
        {   
            this.DateNefila = 4102444800000;
            this.DateLeda = -5364662400000;
        }
       */
        //print(ENTITY);

       // string formattedDate = $" {FromStamp2Date(DateLeda)} -- {FromStamp2Date(DateNefila)}";
        //string biography = $"נולד ב{(string)ENTITY["YeshuvLedaID"]} {(string)ENTITY["SibatMavetID"]} באזור {(string)ENTITY["MekomNefilaID"]} השאיר אחריו {(string)ENTITY["Sheerim"]} בן {(string)ENTITY["NefilaAge"]} היה בנפלו לאחר נפילתו הועלה לדרגת {(string)ENTITY["DargaID"]}";
        string biography = (string)ENTITY["ShortBio"];
        print("#$#$#$##$$$$$$$$$$$$$$$$$$$$$$$$$$$");

        Name.text = $"{(string)ENTITY["FirstName"]} {(string)ENTITY["LastName"]}" ;

        //Date.text = (string)formattedDate;
        Date.text = $"({(string)ENTITY["DateLeda"]} - {(string)ENTITY["DateNefila"]})";
        Texture sourceTexture = imgx.texture;
        newImgx.texture = imgx.texture;//sourceTexture;
        Biography.text = biography;
    
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }

    string FromStamp2Date(long stamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(stamp / 10000);
        DateTime dateTime = dateTimeOffset.UtcDateTime;
        string formattedDate = dateTime.ToString("yyyy");
        print(formattedDate);
        if (formattedDate != null)
        {
            return formattedDate;

        }else
        {
            return "1800";
        }

    }
    // void OnBecameInvisible()
    // {
    //     // Destroy the game object when it becomes invisible
    // }

    public void onClickDo()
    {
        SceneManager.LoadScene("Home");
    }
}
