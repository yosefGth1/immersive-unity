using UnityEngine;
using UnityEngine.UI;

public class buttons : MonoBehaviour
{
    private int MAXnumButtons = 6;
    public int numBtns = 3;
    public float buttonSpacing = 1.5f;
    public GameObject buttonPrefab;

    void Start()
    {
        if (numBtns <= MAXnumButtons)
        {
            for (int i = 0; i < numBtns; i++)
            {
                GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube);
                button.transform.position = new Vector3(i * buttonSpacing, 222, 0);

                button.AddComponent<BoxCollider>();
                button.AddComponent<Button>();
                // button.GetComponent<Renderer>().material.color = Color.green;
                button.transform.SetParent(transform);
            }
        }
        else
        {
            Debug.LogError($" ERROR numBtns is bigger than {MAXnumButtons}");
        }
    }
}

// Added new line for test