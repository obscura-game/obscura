using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform; // Asigna la cámara aquí
    public float sensitivity = 300f;  // Sensibilidad del mouse
    public float speed = 5f;          // Velocidad de movimiento
    public AudioSource footstepAudioSource; // Arrastra aquí el AudioSource en el Inspector
    public float fadeSpeed = 10f;      // Velocidad de desvanecimiento del sonido

    private float xRotation = 0f;     // Rotación en el eje X para la cámara

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor
        footstepAudioSource.loop = true;         // Configurar el AudioSource para que se repita en bucle
        footstepAudioSource.volume = 0f;         // Comenzar con el sonido silenciado
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * horizontalInput + transform.forward * verticalInput;
        bool isMoving = direction.magnitude > 0.1f; // Comprobar si el jugador se está moviendo

        // Movimiento del jugador
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Control del sonido de las pisadas con transición de volumen
        if (isMoving)
        {
            if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Play(); // Iniciar el sonido si no está sonando
            }
            footstepAudioSource.volume = Mathf.Lerp(footstepAudioSource.volume, 0.2f, fadeSpeed * Time.deltaTime); // Aumentar el volumen suavemente
        }
        else
        {
            footstepAudioSource.volume = Mathf.Lerp(footstepAudioSource.volume, 0f, fadeSpeed * Time.deltaTime); // Reducir el volumen suavemente

            if (footstepAudioSource.volume < 0.01f && footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop(); // Detener el sonido cuando el volumen sea casi cero
            }
        }
    }
}