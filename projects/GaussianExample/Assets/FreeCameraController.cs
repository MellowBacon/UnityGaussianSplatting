using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public float fastMovementSpeed = 50.0f;
    public float freeLookSensitivity = 3.0f;
    public float zoomSensitivity = 10.0f;
    public float fastZoomSensitivity = 50.0f;

    // New fields for inactivity reset
    public float inactivityResetTime = 10f; // Seconds of inactivity before reset
    private float inactivityTimer = 0f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool looking = false;

    void Start()
    {
        // Record the starting transform of the camera
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Handle movement
        var speed = Input.GetKey(KeyCode.LeftShift) ? fastMovementSpeed : movementSpeed;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= transform.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        // Handle zoom
        var zoomSpeed = Input.GetKey(KeyCode.LeftShift) ? fastZoomSensitivity : zoomSensitivity;
        transform.position += transform.forward * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        // Handle free look
        if (Input.GetMouseButtonDown(1))
        {
            StartLooking();
        }
        if (Input.GetMouseButtonUp(1))
        {
            StopLooking();
        }
        if (looking)
        {
            transform.Rotate(-Input.GetAxis("Mouse Y") * freeLookSensitivity, Input.GetAxis("Mouse X") * freeLookSensitivity, 0);
            var angles = transform.eulerAngles;
            angles.z = 0;
            transform.eulerAngles = angles;
        }

        // Check for user activity and update the inactivity timer
        if (IsUserActive())
        {
            inactivityTimer = 0f;
        }
        else
        {
            inactivityTimer += Time.deltaTime;
            if (inactivityTimer >= inactivityResetTime)
            {
                ResetCamera();
                inactivityTimer = 0f; // Reset the timer so the camera doesn't keep resetting
            }
        }
    }

    // Helper method to determine if the user is actively providing input
    bool IsUserActive()
    {
        return Input.anyKey ||
               Mathf.Abs(Input.GetAxis("Mouse X")) > 0.01f ||
               Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.01f ||
               Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.01f;
    }

    // Reset the camera to its initial transform
    void ResetCamera()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        StopLooking();  // Ensures the free look mode is turned off when resetting
    }

    void StartLooking()
    {
        looking = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void StopLooking()
    {
        looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
