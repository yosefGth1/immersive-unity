using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public Camera[] touchCameras;
    public Canvas[] touchCanvases;

    void Update()
    {
        // Check if any touches occurred
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                // Iterate through each touch and process it
                for (int i = 0; i < touchCameras.Length; i++)
                {
                    Camera touchCamera = touchCameras[i];
                    Canvas touchCanvas = touchCanvases[i];

                    // Convert touch position to canvas coordinates
                    Vector2 touchPosition = touchCamera.ScreenToViewportPoint(touch.position);
                    Vector2 canvasPosition = new Vector2(touchPosition.x * touchCanvas.pixelRect.width, touchPosition.y * touchCanvas.pixelRect.height);

                    // Check if the touch is within the canvas bounds
                    if (RectTransformUtility.RectangleContainsScreenPoint(touchCanvas.GetComponent<RectTransform>(), canvasPosition, touchCamera))
                    {
                        // Get the button that was touched
                        Button touchedButton = GetButtonAtPosition(touchCanvas, canvasPosition);

                        // Handle the button click
                        if (touchedButton != null)
                        {
                            touchedButton.onClick.Invoke();
                        }
                    }
                }
            }
        }
    }

    Button GetButtonAtPosition(Canvas canvas, Vector2 position)
    {
        // Iterate through all buttons in the canvas and find the button at the given position
        Button[] buttons = canvas.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(buttonRect, position, canvas.worldCamera))
            {
                return button;
            }
        }
        return null;
    }
}
