using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResetCameraAfterInactivity : MonoBehaviour
{
    public float idleTimeToReset = 5f; // Seconds of inactivity before reset
    public float returnSpeed = 2f;     // Speed of return to start position

    private Vector3 startPosition;
    private Quaternion startRotation;

    private float timer = 0f;
    private Vector3 lastPosition;

    private bool isReturning = false;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        lastPosition = transform.position;
    }

    void Update()
    {
        // Check for movement
        if (transform.position != lastPosition)
        {
            timer = 0f;
            isReturning = false;
        }
        else
        {
            timer += Time.deltaTime;

            if (timer >= idleTimeToReset)
            {
                isReturning = true;
            }
        }

        if (isReturning)
        {
            transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, Time.deltaTime * returnSpeed);
        }

        lastPosition = transform.position;
    }
}

