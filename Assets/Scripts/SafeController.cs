using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SafeController : MonoBehaviour
{
    public string correctCode = "1234"; // Código correcto para abrir la caja fuerte
    public GameObject safeCanvas;      // Referencia al Canvas emergente
    public GameObject door;            // Referencia a la puerta de la caja fuerte
    public float openAngle = 90f;      // Ángulo al que se abre la puerta
    private bool isOpen = false;       // Indica si la caja fuerte está abierta

    public TMP_InputField codeInputField;  // Referencia manual al InputField (asignar en el Inspector)
    private Quaternion closedRotation; // Rotación inicial (cerrada)
    private Quaternion openRotation;   // Rotación final (abierta)

    private void Start()
    {
        // Guardar la rotación inicial de la puerta
        closedRotation = door.transform.localRotation;
        openRotation = Quaternion.Euler(door.transform.localEulerAngles + new Vector3(0, openAngle, 0));

        // Verificar que el InputField esté asignado
        if (codeInputField == null)
        {
            Debug.LogError("No se encontró un InputField en el Canvas de la caja fuerte. Asígna uno en el Inspector.");
        }
    }

    /// <summary>
    /// Abre la caja fuerte si el código es correcto.
    /// </summary>
    public void TryOpenSafe()
    {
        if (isOpen) return; // Si ya está abierta, no hacer nada

        if (codeInputField == null)
        {
            Debug.LogError("El InputField no está asignado. No se puede verificar el código.");
            return;
        }

        string enteredCode = codeInputField.text;
        if (enteredCode == correctCode)
        {
            Debug.Log("Código correcto. Abriendo la caja fuerte...");
            OpenSafe();
        }
        else
        {
            Debug.Log("Código incorrecto. Intenta de nuevo.");
        }

        // Ocultar el Canvas después de intentar abrir la caja fuerte
        HideCodeCanvas();
    }

    /// <summary>
    /// Abre la puerta de la caja fuerte.
    /// </summary>
    private void OpenSafe()
    {
        door.transform.localRotation = openRotation;
        isOpen = true;
        Debug.Log("Caja fuerte abierta.");
    }

    /// <summary>
    /// Muestra el Canvas de entrada de código y habilita el cursor.
    /// </summary>
    public void ShowCodeCanvas()
    {
        if (safeCanvas != null)
        {
            safeCanvas.SetActive(true);
            EnableCursor(); // Habilitar el cursor
        }
    }

    /// <summary>
    /// Oculta el Canvas de entrada de código y deshabilita el cursor.
    /// </summary>
    public void HideCodeCanvas()
    {
        if (safeCanvas != null)
        {
            safeCanvas.SetActive(false);
            DisableCursor(); // Deshabilitar el cursor
        }
    }

    /// <summary>
    /// Habilita el cursor y detiene el movimiento de la cámara.
    /// </summary>
    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Desbloquear el cursor
        Cursor.visible = true;                 // Hacer visible el cursor
        PlayerController playerMovement = FindObjectOfType<PlayerController>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;    // Desactivar el movimiento del jugador
        }
    }

    /// <summary>
    /// Deshabilita el cursor y reanuda el movimiento de la cámara.
    /// </summary>
    private void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor
        Cursor.visible = false;                  // Ocultar el cursor
        PlayerController playerMovement = FindObjectOfType<PlayerController>();
        if (playerMovement != null)
        {
            playerMovement.enabled = true;      // Reactivar el movimiento del jugador
        }
    }
}