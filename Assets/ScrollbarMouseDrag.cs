using UnityEngine;
using UnityEngine.UI;

public class ScrollbarMouseDrag : MonoBehaviour
{
    private Scrollbar scrollbar;
    private bool isDragging = false;
    private Vector2 lastMousePosition;

    private void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
    }

    private void Update()
    {
        if (isDragging)
        {
            // Calculate the change in mouse position
            Vector2 currentMousePosition = Input.mousePosition;
            float deltaY = currentMousePosition.y - lastMousePosition.y;

            // Convert the change in mouse position to scrollbar value change
            float scrollbarChange = deltaY / Screen.height;

            // Update the scrollbar value
            scrollbar.value += scrollbarChange;

            // Clamp the scrollbar value between 0 and 1
            scrollbar.value = Mathf.Clamp01(scrollbar.value);

            // Update the last mouse position
            lastMousePosition = currentMousePosition;
        }
    }

    public void StartDragging()
    {
        isDragging = true;
        lastMousePosition = Input.mousePosition;
    }

    public void StopDragging()
    {
        isDragging = false;
    }
}