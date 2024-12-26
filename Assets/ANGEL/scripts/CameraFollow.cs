using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public Vector3 offset;   // Offset desde el jugador

    // Variables para el shake
    private Vector3 originalOffset;  // Posición inicial del offset
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.5f;
    private float dampingSpeed = 2.0f;

    void Awake()
    {
        // Guarda el offset original
        originalOffset = offset;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Si hay un shake activo, aplica el movimiento aleatorio
            Vector3 shakeOffset = Vector3.zero;
            if (shakeDuration > 0)
            {
                shakeOffset = new Vector3(
                    Random.Range(-1f, 1f) * shakeMagnitude,
                    Random.Range(-1f, 1f) * shakeMagnitude,
                    0); // Sólo en los ejes X e Y

                shakeDuration -= Time.deltaTime * dampingSpeed;

                if (shakeDuration <= 0)
                {
                    shakeDuration = 0f;
                    offset = originalOffset; // Restaura el offset original
                }
            }

            // Actualiza la posición de la cámara con el offset y el shake
            transform.position = player.position + offset + shakeOffset;
        }
    }

    // Método para activar el shake
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}