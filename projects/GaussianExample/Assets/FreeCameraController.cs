using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public float fastMovementSpeed = 50.0f;
    public float freeLookSensitivity = 3.0f;
    public float zoomSensitivity = 10.0f;
    public float fastZoomSensitivity = 50.0f;

    private bool looking = false;

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
