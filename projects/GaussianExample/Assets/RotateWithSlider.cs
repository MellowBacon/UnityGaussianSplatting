using UnityEngine;
using UnityEngine.UI;

public class RotateWithSlider : MonoBehaviour
{
    public Slider rotationSpeedSlider;
    private float rotationSpeed;

    void Start()
    {
        if (rotationSpeedSlider != null)
        {
            // Set the initial value of the rotation speed
            rotationSpeed = rotationSpeedSlider.value;
            // Add a listener to detect changes in the slider value
            rotationSpeedSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    void Update()
    {
        // Rotate the object around its Y axis at the rotation speed
        transform.Rotate(Vector3.up, rotationSpeed* 10 * Time.deltaTime);
    }

    void OnSliderValueChanged(float value)
    {
        // Update the rotation speed when the slider value changes
        rotationSpeed = value;
    }
}
