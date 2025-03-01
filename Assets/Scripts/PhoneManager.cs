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
    public Transform chatContentNPC, chatContentPlayer;
    public TMP_InputField inputField;
    public GameObject PlayerMessagePrefab;
    public GameObject NPCMessagePrefab;
    public AudioSource npcNotificationSound;
    public AudioSource playerNotificationSound;
    public PlayerController playerController;

    private bool isPhoneActive = false;
    private bool isScrolling = false;

    void Start()
    {
        PhoneCanvas.SetActive(false);
        ControlsCanvas.SetActive(true);
        isPhoneActive = false;
        StartCoroutine(HideControlsAndStartConversation());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !inputField.isFocused)
        {
            isPhoneActive = !isPhoneActive;
            PhoneCanvas.SetActive(isPhoneActive);

            if (isPhoneActive)
            {
                inputField.ActivateInputField();
                inputField.text = "";
                if (playerController != null)
                {
                    playerController.enabled = false;
                }
            }
            else
            {
                inputField.DeactivateInputField();
                if (playerController != null)
                {
                    playerController.enabled = true;
                }
            }
        }
    }

    IEnumerator HideControlsAndStartConversation()
    {
        yield return new WaitForSeconds(20); 
        ControlsCanvas.SetActive(false);
        StartCoroutine(StartConversation());
    }

    IEnumerator StartConversation()
    {
        yield return new WaitForSeconds(10);
        AddMessage("¿Has podido entrar?", false);
        yield return new WaitForSeconds(5);
        AddMessage("Sí, ha sido más fácil de lo que pensaba.", true);
        yield return new WaitForSeconds(5);
        AddMessage("Perfecto, ten cuidado.", false);
        yield return new WaitForSeconds(4);
        AddMessage("No hay nada de qué preocuparse, es un simple hospital abandonado", true);
    }

    public void SendMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            AddMessage(inputField.text, true);
            inputField.text = "";
            inputField.ActivateInputField();
            StartCoroutine(ScrollToBottom());
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

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentPlayer.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentNPC.GetComponent<RectTransform>());
        
        if (!isScrolling) // Solo hacer scroll si el usuario no está navegando manualmente
        {
            StartCoroutine(ScrollToBottom());
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

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        if (!isScrolling)
        {
            chatScroll.verticalNormalizedPosition = 0f;
        }
    }

    public void OnBeginScroll()
    {
        isScrolling = true;
    }

    public void OnEndScroll()
    {
        isScrolling = false;
    }
}
