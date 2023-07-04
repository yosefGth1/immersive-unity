using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Candel : MonoBehaviour
{
    public TMP_Text buttonText;
    // public Button button;// = GetComponent<TMP_Button>();
    public int time_preesed = 0;
    public GameObject prefabC;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rct = GetComponent<RectTransform>();
        rct.transform.SetParent(transform);
        // rct.sizeDelta = new Vector2(324,345);
        rct.anchoredPosition = new Vector3(1981,1337,-55);
        // Button btn =  GetComponentInChildren<Button>();
        GameObject candle = GameObject.Find("candle02");
        buttonText.text = "1";
        // candle.AddComponent<ClickableObject>();
        GameObject instantiatedObject = Instantiate(this.prefabC, new Vector3(1,1,1),Quaternion.identity);
    //     foreach (Transform child in instantiatedObject.transform)
    // {
    //     // Perform operations on each child
    //     // For example, you can set their positions, apply materials, or access their components
    //     child.position = new Vector3(1f, 1f, 1f);
    // }

        // Attach the ClickableObject script to the instantiated object
        // ClickableObject clickableObject = instantiatedObject.AddComponent<ClickableObject>();
        // candle.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick()
    {
        Debug.Log("Button clicked!");
        time_preesed++;
        buttonText.text = time_preesed.ToString();
        // SceneManager.LoadScene("level02");

    }

    
}