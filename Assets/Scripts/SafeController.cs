using UnityEngine;
using UnityEngine.UI;

public class SafeController : MonoBehaviour
{
    public string correctCode = "1234"; // Código correcto para abrir la caja fuerte
    public GameObject safeCanvas;      // Referencia al Canvas emergente
    public GameObject door;            // Referencia a la puerta de la caja fuerte
    public float openAngle = 90f;      // Ángulo al que se abre la puerta
    private bool isOpen = false;       // Indica si la caja fuerte está abierta

    private InputField codeInputField; // Referencia al campo de entrada
    private Quaternion closedRotation; // Rotación inicial (cerrada)
    private Quaternion openRotation;   // Rotación final (abierta)

    private void Start()
    {
        // Guardar la rotación inicial de la puerta
        closedRotation = door.transform.localRotation;
        openRotation = Quaternion.Euler(door.transform.localEulerAngles + new Vector3(0, openAngle, 0));

        // Obtener el InputField del Canvas
        if (safeCanvas != null)
        {
            codeInputField = safeCanvas.GetComponentInChildren<InputField>();
            if (codeInputField == null)
            {
                Debug.LogError("No se encontró un InputField en el Canvas de la caja fuerte.");
            }
        }
    }

    /// <summary>
    /// Abre la caja fuerte si el código es correcto.
    /// </summary>
    public void TryOpenSafe()
    {
        if (isOpen) return; // Si ya está abierta, no hacer nada

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
    /// Muestra el Canvas de entrada de código.
    /// </summary>
    public void ShowCodeCanvas()
    {
        if (safeCanvas != null)
        {
            safeCanvas.SetActive(true);
        }
    }

    /// <summary>
    /// Oculta el Canvas de entrada de código.
    /// </summary>
    public void HideCodeCanvas()
    {
        if (safeCanvas != null)
        {
            safeCanvas.SetActive(false);
        }
    }
}