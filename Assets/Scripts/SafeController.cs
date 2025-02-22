using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SafeController : MonoBehaviour
{
    public string correctCode = "1234"; // C�digo correcto para abrir la caja fuerte
    public GameObject safeCanvas;      // Referencia al Canvas emergente
    public GameObject door;            // Referencia a la puerta de la caja fuerte
    public float openAngle = 90f;      // �ngulo al que se abre la puerta
    private bool isOpen = false;       // Indica si la caja fuerte est� abierta

    public TMP_InputField codeInputField;  // Referencia manual al InputField (asignar en el Inspector)
    private Quaternion closedRotation; // Rotaci�n inicial (cerrada)
    private Quaternion openRotation;   // Rotaci�n final (abierta)

    private void Start()
    {
        // Guardar la rotaci�n inicial de la puerta
        closedRotation = door.transform.localRotation;
        openRotation = Quaternion.Euler(door.transform.localEulerAngles + new Vector3(0, openAngle, 0));

        // Verificar que el InputField est� asignado
        if (codeInputField == null)
        {
            Debug.LogError("No se encontr� un InputField en el Canvas de la caja fuerte. As�gna uno en el Inspector.");
        }
    }

    /// <summary>
    /// Abre la caja fuerte si el c�digo es correcto.
    /// </summary>
    public void TryOpenSafe()
    {
        if (isOpen) return; // Si ya est� abierta, no hacer nada

        if (codeInputField == null)
        {
            Debug.LogError("El InputField no est� asignado. No se puede verificar el c�digo.");
            return;
        }

        string enteredCode = codeInputField.text;
        if (enteredCode == correctCode)
        {
            Debug.Log("C�digo correcto. Abriendo la caja fuerte...");
            OpenSafe();
        }
        else
        {
            Debug.Log("C�digo incorrecto. Intenta de nuevo.");
        }

        // Ocultar el Canvas despu�s de intentar abrir la caja fuerte
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
    /// Muestra el Canvas de entrada de c�digo y habilita el cursor.
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
    /// Oculta el Canvas de entrada de c�digo y deshabilita el cursor.
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
    /// Habilita el cursor y detiene el movimiento de la c�mara.
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
    /// Deshabilita el cursor y reanuda el movimiento de la c�mara.
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