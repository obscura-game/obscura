using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform camTransform;
    private Vector3 originalPosition;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f; // Intensidad del shake
    private float dampingSpeed = 1.0f;   // Velocidad de atenuación

    void Awake()
    {
        camTransform = transform;
        originalPosition = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;

            // Reducir la duración del shake gradualmente
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPosition;
        }
    }

    // Método para activar el efecto shake
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}