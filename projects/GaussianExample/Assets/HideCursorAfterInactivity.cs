using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursorAfterInactivity : MonoBehaviour
{
    public float idleTimeToHide = 2f; // Time in seconds before cursor hides
    private float timer = 0f;
    private Vector3 lastMousePosition;

    void Start()
    {
        lastMousePosition = Input.mousePosition;
        Cursor.visible = true;
    }

    void Update()
    {
        // Check if the mouse moved
        if (Input.mousePosition != lastMousePosition)
        {
            Cursor.visible = true;
            timer = 0f; // Reset the timer
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= idleTimeToHide)
            {
                Cursor.visible = false;
            }
        }

        lastMousePosition = Input.mousePosition;
    }
}
