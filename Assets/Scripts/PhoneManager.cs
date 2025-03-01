using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhoneManager : MonoBehaviour
{
    public GameObject PhoneCanvas;
    public GameObject ControlsCanvas;
    public ScrollRect chatScroll;
    public RectTransform chatContent;
    public Transform chatContentNPC, chatContentPlayer;
    public TMP_InputField inputField;
    public GameObject PlayerMessagePrefab;
    public GameObject NPCMessagePrefab;
    public AudioSource npcNotificationSound;
    public AudioSource playerNotificationSound;
    public PlayerController playerController;

    private bool isPhoneActive = false;
    private bool isUserScrolling = false;

    void Start()
    {
        PhoneCanvas.SetActive(false);
        ControlsCanvas.SetActive(true);
        isPhoneActive = false;
        StartCoroutine(HideControlsAndStartConversation());

        // Suscribirse a eventos de scroll
        chatScroll.onValueChanged.AddListener(OnScroll);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !inputField.isFocused)
        {
            isPhoneActive = !isPhoneActive;
            PhoneCanvas.SetActive(isPhoneActive);

            if (isPhoneActive)
            {
                inputField.DeactivateInputField(); // No activar el InputField automáticamente
                if (playerController != null)
                {
                    playerController.enabled = false;
                }
            }
            else
            {
                if (playerController != null)
                {
                    playerController.enabled = true;
                }
            }
        }

        // Si el teléfono está activo y se presiona Enter sin estar escribiendo, activar el InputField
        if (isPhoneActive && !inputField.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            inputField.text = "";
            inputField.ActivateInputField();
        }

        // Permitir enviar mensaje con Enter cuando el InputField está enfocado
        if (isPhoneActive && inputField.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
            inputField.DeactivateInputField(); // Desactivar el focus después de enviar el mensaje
        }
    }

    IEnumerator HideControlsAndStartConversation()
    {
        yield return new WaitForSeconds(20); 
        ControlsCanvas.SetActive(false);
        StartCoroutine(StartConversation1());
    }

    IEnumerator StartConversation1()
    {
        yield return new WaitForSeconds(1);
        AddMessage("¿Has podido entrar?", false);
        PhoneCanvas.SetActive(true);
        yield return new WaitForSeconds(4);
        AddMessage("Sí, ha sido más fácil de lo que pensaba.", true);
        yield return new WaitForSeconds(1);
        AddMessage("Perfecto, ten cuidado.", false);
        yield return new WaitForSeconds(1);
        AddMessage("No hay nada de qué preocuparse, es un simple hospital abandonado", true);

        yield return new WaitForSeconds(1);
        AddMessage("¿Has podido entrar?", false);
        yield return new WaitForSeconds(1);
        AddMessage("Sí, ha sido más fácil de lo que pensaba.", true);
        yield return new WaitForSeconds(1);
        AddMessage("Perfecto, ten cuidado.", false);
        yield return new WaitForSeconds(1);
        AddMessage("No hay nada de qué preocuparse, es un simple hospital abandonado", true);
        yield return new WaitForSeconds(1);
        AddMessage("¿Has podido entrar?", false);
        yield return new WaitForSeconds(1);
        AddMessage("Sí, ha sido más fácil de lo que pensaba.", true);
        yield return new WaitForSeconds(1);
        AddMessage("Perfecto, ten cuidado.", false);
        yield return new WaitForSeconds(1);
        AddMessage("No hay nada de qué preocuparse, es un simple hospital abandonado", true);
    }

    public void SendMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            AddMessage(inputField.text, true);
            inputField.text = ""; // Limpiar el InputField después de enviar
            inputField.DeactivateInputField(); // Desactivar InputField después de enviar
            StartCoroutine(AutoScroll());
        }
    }

    void AddMessage(string text, bool isPlayer)
    {
        GameObject message = Instantiate(isPlayer ? PlayerMessagePrefab : NPCMessagePrefab);
        TMP_Text messageText = message.GetComponentInChildren<TMP_Text>();
        messageText.text = text;

        if (isPlayer)
        {
            message.transform.SetParent(chatContentPlayer, false);
        }
        else
        {
            message.transform.SetParent(chatContentNPC, false);
        }

        messageText.enableAutoSizing = true;

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContent);
        
        if (!isUserScrolling) // Solo hacer scroll si el usuario no está navegando manualmente
        {
            StartCoroutine(AutoScroll());
        }

        if (isPlayer && playerNotificationSound != null)
        {
            playerNotificationSound.Play();
        }
        else if (!isPlayer && npcNotificationSound != null)
        {
            npcNotificationSound.Play();
        }
    }

    IEnumerator AutoScroll()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        
        float chatHeight = chatContent.rect.height;
        float scrollHeight = chatScroll.viewport.rect.height;
        
        if (chatHeight > scrollHeight) // Si el contenido es más grande que la vista, hacer scroll
        {
            chatScroll.verticalNormalizedPosition = Mathf.Clamp01(1 - (scrollHeight / chatHeight));
        }
    }

    public void OnScroll(Vector2 position)
    {
        // Detectar si el usuario está desplazándose manualmente
        isUserScrolling = chatScroll.verticalNormalizedPosition > 0.01f;
    }
}