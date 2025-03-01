using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Tooltip("�ngulo m�ximo de apertura de la puerta (en grados).")]
    public float maxAngle = 90f; // �ngulo de apertura (90 grados por defecto)

    [Tooltip("Velocidad de rotaci�n de la puerta.")]
    public float rotationSpeed = 2f; // Velocidad de apertura/cierre

    [Tooltip("Tiempo en segundos antes de que la puerta se cierre autom�ticamente.")]
    public float autoCloseDelay = 5f; // Tiempo antes de cerrarse autom�ticamente

    private bool isOpening = false; // Indica si la puerta est� abri�ndose
    private bool isClosing = false; // Indica si la puerta est� cerr�ndose
    private Quaternion closedRotation; // Rotaci�n inicial (cerrada)
    private Quaternion openRotation;  // Rotaci�n final (abierta)
    
    // Sonido abrir y cerrar la puerta
    public AudioSource doorSound;

    private void Start()
    {
        // Guarda la rotaci�n inicial de la puerta (cerrada)
        closedRotation = transform.rotation;
        // Calcula la rotaci�n final (abierta)
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, maxAngle, 0));
    }

    private void Update()
    {
        // Abre la puerta si est� en proceso de abrirse
        if (isOpening)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, openRotation, Time.deltaTime * rotationSpeed);
            // Detener el movimiento cuando la puerta est� completamente abierta
            if (Quaternion.Angle(transform.rotation, openRotation) < 1f)
            {
                isOpening = false;
            }
        }

        // Cierra la puerta si est� en proceso de cerrarse
        if (isClosing)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, Time.deltaTime * rotationSpeed);
            // Detener el movimiento cuando la puerta est� completamente cerrada
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
        doorSound.Play();
        if (!isOpening && !isClosing)
        {
            isOpening = true;
            isClosing = false;
            Debug.Log("Abriendo puerta...");

            // Programar el cierre autom�tico despu�s del tiempo especificado
            Invoke("AutoCloseDoor", autoCloseDelay);
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

        // Cancelar cualquier cierre autom�tico programado
        CancelInvoke("AutoCloseDoor");
    }

    /// <summary>
    /// Cierra la puerta autom�ticamente.
    /// </summary>
    private void AutoCloseDoor()
    {
        if (!isOpening && !isClosing)
        {
            isClosing = true;
            isOpening = false;
            Debug.Log("Cerrando puerta autom�ticamente...");
        }
    }

    /// <summary>
    /// Verifica si la puerta est� abierta.
    /// </summary>
    public bool IsDoorOpen()
    {
        return Quaternion.Angle(transform.rotation, openRotation) < 1f;
    }
}