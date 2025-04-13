using UnityEngine;
using System.Collections;

public class FreeCameraController : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public float fastMovementSpeed = 50.0f;
    public float freeLookSensitivity = 3.0f;
    public float zoomSensitivity = 10.0f;
    public float fastZoomSensitivity = 50.0f;

    // New fields for inactivity reset.
    public float inactivityResetTime = 10f; // Seconds of inactivity before reset
    private float inactivityTimer = 0f;

    // New flag to determine if the user has actively taken control.
    private bool userHasControlled = false;

    // Store the initial local transform (if the camera is parented).
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    private bool looking = false;

    void Start()
    {
        // Record the starting local transform of the camera if it's parented;
        // otherwise record the world transform.
        if (transform.parent != null)
        {
            initialLocalPosition = transform.localPosition;
            initialLocalRotation = transform.localRotation;
        }
        else
        {
            initialLocalPosition = transform.position;
            initialLocalRotation = transform.rotation;
        }
    }

    void Update()
    {
        // Handle movement.
        var speed = Input.GetKey(KeyCode.LeftShift) ? fastMovementSpeed : movementSpeed;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            transform.position += transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            transform.position -= transform.forward * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            transform.position -= transform.right * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            transform.position += transform.right * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q))
            transform.position -= transform.up * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E))
            transform.position += transform.up * speed * Time.deltaTime;

        // Handle zoom.
        var zoomSpeed = Input.GetKey(KeyCode.LeftShift) ? fastZoomSensitivity : zoomSensitivity;
        transform.position += transform.forward * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        // Handle free look.
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
            // Zero out any unwanted roll.
            var angles = transform.eulerAngles;
            angles.z = 0;
            transform.eulerAngles = angles;
        }

        // Check if the user has taken control.
        // You can extend this condition with additional input checks as needed.
        if (Input.GetMouseButtonDown(1) ||
            Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D) ||
            Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.01f)
        {
            userHasControlled = true;
            inactivityTimer = 0f; // Reset timer on input.
        }

        // Only start the inactivity timer if the user has taken control before.
        if (userHasControlled)
        {
            if (!IsUserActive())
            {
                inactivityTimer += Time.deltaTime;
                if (inactivityTimer >= inactivityResetTime)
                {
                    ResetCamera();
                    // Reset the control flag after the reset.
                    userHasControlled = false;
                    inactivityTimer = 0f;
                }
            }
            else
            {
                inactivityTimer = 0f; // Reset the timer if user activity is detected.
            }
        }
    }

    // Helper method to determine if the user is actively providing input.
    bool IsUserActive()
    {
        return Input.anyKey ||
               Mathf.Abs(Input.GetAxis("Mouse X")) > 0.01f ||
               Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.01f ||
               Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.01f;
    }

    // Initiates a smooth reset of the camera's transform.
    void ResetCamera()
    {
        StartCoroutine(SmoothResetCamera(1.0f));  // Adjust the duration (in seconds) as desired.
    }

    // Coroutine for smoothly resetting the camera's local transform.
    IEnumerator SmoothResetCamera(float duration)
    {
        float elapsedTime = 0f;
        // Use local positions and rotations so that the reset accounts for the parent's transform.
        Vector3 startingLocalPos = transform.localPosition;
        Quaternion startingLocalRot = transform.localRotation;

        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Lerp(startingLocalPos, initialLocalPosition, elapsedTime / duration);
            transform.localRotation = Quaternion.Slerp(startingLocalRot, initialLocalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure an exact reset at the end.
        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;
        StopLooking();  // Turn off free look mode after resetting.
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
