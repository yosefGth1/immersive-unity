using System;
using System.Collections;
using System.Net.Http;
// using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TMPro;
using Microsoft.Maps.Unity;
//using static UnityEditor.VersionControl.Message;
//using Unity.VisualScripting;

public class FetchHalalim : MonoBehaviour
{

    private HttpClient client = new HttpClient();
    public string apiUrl = "http://10.0.0.10:30031/entities?from=51&size=517";
    public string jsonUrl2 = "https://raw.githubusercontent.com/yosefGth1/some_hosted_files/main/json_output2.json";
    public string _response;
    private JObject JsonData1 = new JObject();
    // public RawImage imagePrefab;
    // int Bindex = 0;
    public TextMeshProUGUI halal_name_text;
    TextMeshProUGUI halal_BP;
    string halalName;
    public float MaxImgH = 3.0f;
    public float MaxImgW = 5.0f;

    // Start is called before the first frame update
    void Start()
    {

        
       // this.JsonData1 = await MakeRequest(this.apiUrl);
    }


    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     orderJson(JsonData1);
        //     this.halalName = $"{(string)JsonData1["data"][Bindex]["_source"]["firstname"]} {(string)JsonData1["data"][Bindex]["_source"]["lastname"]} נולד ב";
        //     string  BD = (string)JsonData1["data"][Bindex]["_source"]["birthDate"]["international"];
        //     halal_name_text.text = halalName;
        //     string DD = (string)JsonData1["data"][Bindex]["_source"]["deathDate"]["international"];
        //     halal_BP.text = $"{BD.Substring(0,10)} -  {DD.Substring(0,10)}";
            
        //     // StartCoroutine(DownloadImage(ImgURL));
        //     Bindex++;

        // }

    }

   
    public IEnumerator MakeRequest2(string URL)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                yield break;
            }

            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);

            // Process the response data here
        }
    }

    public void OnWebRequestComplete(UnityWebRequest request)
    {
        if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("HTTP Error: " + request.error);
        }
        else
        {
            _response = request.downloadHandler.text;
        }
    }

    async public Task<string> MakeRequest3(string URL)
    {
        UnityWebRequest request = UnityWebRequest.Get(URL);

        // Send the web request asynchronously.
        await request.SendWebRequest();

        // Check if the web request was successful.
        if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("HTTP Error: " + request.error);
            return "ERROR IN REQUEST";
        }
        else
        {
            _response = request.downloadHandler.text;
            //print(_response);
            return _response;
        }
    }

    public async Task<JArray> MakeRequest(string URL)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync(URL);
            response.EnsureSuccessStatusCode();
            string resBody = await response.Content.ReadAsStringAsync();
            print(resBody);
            JArray jsonData2 = JArray.Parse(resBody);
            Debug.Log("SUCCESS");
            Debug.Log(jsonData2);
            return jsonData2;

        }
        catch (HttpRequestException e)
        {
            Debug.LogError(e);
            Debug.LogError("Error with http in file loadjson");
            return null;
        }
    }


    public IEnumerator DownloadImage(string URL,RawImage newImage)
    {
    
        // RawImage newImage = preFab;
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            newImage.texture = texture;
            newImage.SetNativeSize();
            float newWidth = 90f;//((float)JsonData1["data"]["memes"][this.index]["height"] / (float)JsonData1["data"]["memes"][this.index]["width"]) * 2.5f;
            float newHeight = 100f; //((float)JsonData1["data"]["memes"][this.index]["width"] / (float)JsonData1["data"]["memes"][this.index]["height"]) * 2.5f;
            
            // Debug.Log($" (B) H = {newHeight} , W = {newWidth}");

            
            RectTransform rectTransform = newImage.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(newWidth, newHeight);

            // make sure the image does not reapts itself 
            newImage.uvRect = new Rect(0f, 0f, 1.0f, 1.0f);
            
            // Debug.Log($" (A) H = {newHeight} , W = {newWidth}");
            
        }
        else
        {
            Debug.Log(request.error);
        }



    }

    // public string orderJson(JObject JsonData1)
    // {
    //     string objectKey = (string)JsonData1["data"][Bindex]["_source"]["profilePicture"]["objectKey"];
    //     string ImgURL = $"http://10.0.0.10:30040/profile-images/{objectKey}";
    //     Debug.Log(JsonData1["data"][Bindex]["_source"]["profilePicture"]["objectKey"]);  
    //     Bindex++;
    //     return ImgURL;
    // }


}

