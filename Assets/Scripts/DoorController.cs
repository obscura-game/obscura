using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Tooltip("Ángulo máximo de apertura de la puerta (en grados).")]
    public float maxAngle = 90f; // Ángulo de apertura (90 grados por defecto)

    [Tooltip("Velocidad de rotación de la puerta.")]
    public float rotationSpeed = 2f; // Velocidad de apertura/cierre

    private bool isOpening = false; // Indica si la puerta está abriéndose
    private bool isClosing = false; // Indica si la puerta está cerrándose
    private Quaternion closedRotation; // Rotación inicial (cerrada)
    private Quaternion openRotation; // Rotación final (abierta)

    private void Start()
    {
        // Guarda la rotación inicial de la puerta (cerrada)
        closedRotation = transform.rotation;

        // Calcula la rotación final (abierta)
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, maxAngle, 0));
    }

    private void Update()
    {
        // Abre la puerta si está en proceso de abrirse
        if (isOpening)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, openRotation, Time.deltaTime * rotationSpeed);

            // Detener el movimiento cuando la puerta está completamente abierta
            if (Quaternion.Angle(transform.rotation, openRotation) < 1f)
            {
                isOpening = false;
            }
        }

        // Cierra la puerta si está en proceso de cerrarse
        if (isClosing)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, Time.deltaTime * rotationSpeed);

            // Detener el movimiento cuando la puerta está completamente cerrada
            if (Quaternion.Angle(transform.rotation, closedRotation) < 1f)
            {
                isClosing = false;
            }
        }
    }

    /// <summary>
    /// Abre la puerta.
    /// </summary>
    public void OpenDoor()
    {
        if (!isOpening && !isClosing)
        {
            isOpening = true;
            isClosing = false;
            Debug.Log("Abriendo puerta...");
        }
    }

    /// <summary>
    /// Cierra la puerta.
    /// </summary>
    public void CloseDoor()
    {
        if (!isOpening && !isClosing)
        {
            isClosing = true;
            isOpening = false;
            Debug.Log("Cerrando puerta...");
        }
    }

    /// <summary>
    /// Verifica si la puerta está abierta.
    /// </summary>
    public bool IsDoorOpen()
    {
        return Quaternion.Angle(transform.rotation, openRotation) < 1f;
    }
}