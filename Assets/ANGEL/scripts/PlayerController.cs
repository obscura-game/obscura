using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform; // Asigna la cámara aquí
    public float sensitivity = 300f;  // Sensibilidad del mouse
    public float speed = 5f;          // Velocidad de movimiento

    private float xRotation = 0f;     // Rotación en el eje X para la cámara

    void Start()
    {
        // Bloquear el cursor al inicio
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        // Obtener movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Rotar la cámara en el eje X (mirar hacia arriba/abajo)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotar el jugador en el eje Y (girar hacia los lados)
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        // Obtener las entradas de los ejes (Horizontal y Vertical)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Crear un vector de movimiento basado en las entradas
        Vector3 direction = transform.right * horizontalInput + transform.forward * verticalInput;

        // Aplicar el movimiento en la dirección que el jugador está mirando
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}