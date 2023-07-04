using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class Buttons2Img : FetchHalalim
{
    public Button buttonPrefab;
    JArray Json;
    // int index = 0;
    public Vector3 fixedPos = new Vector3(1,1,2);
    private int btnCount = 21;
    float x = -650f;
    float y = 230f;
    public Canvas canvases;
    string info;
    JArray jsonData;
    // public static JArray jsData;
    // int startIndex;

     

    async void Start()
    {
        print(jsonUrl2);
        string data  = await MakeRequest3(jsonUrl2);
        
        this.jsonData = JArray.Parse(data);


        int targetDisplay = canvases.rootCanvas.targetDisplay;  
        switch(targetDisplay) 
        {
            case 0:

                // startIndex = 0;
                // btnCount = 21;
                ImageHandler(0);
                break;

            case 1:

                // startIndex = 21;
                // btnCount = 42;
                ImageHandler(21);
                break;

            case 2:

                ImageHandler(42);
                // startIndex = 42;
                // btnCount = 63;
                break;

            default:
                Console.WriteLine("############# DEFAult #############");
                break;
        
        }
        

    }
    
     

    /*string ReadJsonFile(string filePath)
    {
        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Read the contents of the file
            using (StreamReader reader = new StreamReader(filePath))
            {
                string jsonString = reader.ReadToEnd();
                return jsonString;
            }
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + filePath);
            return null;
        }
    }

     async void Awake()
    {
        // TextAsset jsonFile = Resources.Load<TextAsset>("C:\\Users\\lani2\\Desktop\\halalim_project\\json_output.json");

        // Parse the JSON data into a JObject
        //string jsonString = ReadJsonFile("C:\\Users\\lani2\\Desktop\\halalim_project\\output_json.json"); //File.ReadAllText("C:\\Users\\lani2\\Desktop\\halalim_project\\json_output.json");

        // Serialize the JSON string to another string
        // string serializedJson = JsonConvert.SerializeObject(jsonString);


        this.jsonData = await MakeRequest(jsonUrl);
        

        //JArray jsonData = JArray.Parse(jsonString);
        // jsData = jsonData;
        // string jsonData = serializedJson;
        // print(jsonData);

        //return jsonData;


    }
    */


    void ImageHandler(int startIndex)
    {
        
        Json = this.jsonData;
        //print(Json);
        for (int i = 0; i < btnCount; i++)
        {
            
            Button btn = Instantiate(buttonPrefab,fixedPos,Quaternion.identity);
            btn.transform.SetParent(transform);
            RectTransform trans = btn.GetComponent<RectTransform>();
            RawImage img2 = btn.GetComponent<RawImage>();
            StartCoroutine(DownloadImage(orderJson(Json,startIndex + i),img2));
            // Debug.Log(i);
            
            if ( i % 7 == 0 &&  i != 0)
            {
                trans.anchoredPosition = new Vector3(x-=960,y-=205,0);
                
            } else {

                trans.anchoredPosition = new Vector3(x+=160,y,0);
            }
            JToken ENTITY = Json[startIndex+i];
            
            string halalName = $"{(string)Json[startIndex+i]["FirstName"]} {(string)Json[startIndex+i]["LastName"]}" ;   //$"{(string)Json["data"][startIndex + i]["_source"]["firstname"]} {(string)Json["data"][startIndex + i]["_source"]["lastname"]}";
            
            //info = halalName; 
            //string dates  = $" ( {(string)Json[startIndex+i]["DateLeda"]} -- {(string)Json[startIndex+i]["DateNefila"]} )" ;
            //float LAT = (float)Json[startIndex+i]["MekomNefilaLat"];
            //float LNG = (float)Json[startIndex+i]["MekomNefilaLng"];
            
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text =  halalName;
            
            btn.onClick.AddListener(() => {

                OnButtonClick(halalName,img2,ENTITY);
            });
            
        }
    }


    string orderJson(JArray JsonData1,int index)
    {
        // string objectKey = (string)JsonData1["data"][index]["_source"]["profilePicture"]["objectKey"];
        // string ImgURL = $"http://10.0.0.10:30040/profile-images/{objectKey}";
        // Debug.Log(JsonData1["data"][index]["_source"]["profilePicture"]["objectKey"]);  
        // index++;

        string ImgURL = (string)JsonData1[index]["Cloud_URL"];
        return ImgURL;

    }


    public void OnButtonClick(string name, RawImage img,JToken entity)
    {
        print("button Clicked");
        //spawner.halalNamex =  name;
        print(entity);
        spawner.imgx = img;
        // Map.Lat = lat;
        // Map.Lng = lng;
        Map.entity = entity;
        spawner.ENTITY = entity;
        SceneManager.LoadScene("level03");
    }

}
    // private void Update() 
    // {

    //     int targetDisplay = canvases.rootCanvas.targetDisplay;  
    //     switch(targetDisplay) 
    //     {
    //         case 0:

    //             // startIndex = 0;
    //             // btnCount = 21;
    //             ImageHandler(0);
    //             break;

    //         case 1:

    //             // startIndex = 21;
    //             // btnCount = 42;
    //             ImageHandler(21);
    //             break;

    //         case 2:

    //             ImageHandler(42);
    //             // startIndex = 42;
    //             // btnCount = 63;
    //             break;

    //         default:
    //             Console.WriteLine("############# DEFAult #############");
    //             break;
        
    //     }
        

    // }
        
