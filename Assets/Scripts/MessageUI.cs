using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageUI : MonoBehaviour
{
    public MaskableGraphic messageText; // Puede ser Text o TMP_Text
    public float displayTime = 2f; // Tiempo que el mensaje estará visible

    private void Start()
    {
        if (messageText == null)
        {
            Debug.LogError("No se ha asignado un componente Text o TMP_Text al MessageUI.");
            return;
        }

        // Asegurarse de que el mensaje esté oculto al inicio
        HideMessage();
    }

    /// <summary>
    /// Muestra un mensaje en la UI durante un tiempo específico.
    /// </summary>
    /// <param name="message">El mensaje que se mostrará.</param>
    /// <param name="time">Tiempo en segundos que el mensaje estará visible (opcional).</param>
    public void ShowMessage(string message, float time = 2f)
    {
        if (messageText != null)
        {
            if (messageText is Text textComponent)
            {
                textComponent.text = message; // Asignar el mensaje si es Text
            }
            else if (messageText is TMP_Text tmpTextComponent)
            {
                tmpTextComponent.text = message; // Asignar el mensaje si es TMP_Text
            }

            messageText.gameObject.SetActive(true); // Mostrar el texto

            // Cancelar cualquier invocación anterior de HideMessage
            CancelInvoke("HideMessage");

            // Ocultar el mensaje después del tiempo especificado
            Invoke("HideMessage", time);
        }
        else
        {
            Debug.LogWarning("No se ha asignado un componente Text o TMP_Text al MessageUI.");
        }
    }

    /// <summary>
    /// Oculta el mensaje de la UI.
    /// </summary>
    private void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false); // Ocultar el texto
        }
    }
}