using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ClickableObject : MonoBehaviour
{
    private void OnMouseDown()
    {
        // This method is called when the object is clicked
        Debug.Log("Object clicked!");
    }
}

