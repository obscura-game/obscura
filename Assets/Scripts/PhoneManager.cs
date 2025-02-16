using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhoneManager : MonoBehaviour
{
    public GameObject PhoneCanvas;
    public ScrollRect chatScroll;
    public Transform chatContent;
    public TMP_InputField inputField;
    public GameObject PlayerMessagePrefab;
    public GameObject NPCMessagePrefab;
    public AudioSource npcNotificationSound;
    public AudioSource playerNotificationSound;
    public PlayerController playerController;

    private bool isPhoneActive = false;

    void Start()
    {
        PhoneCanvas.SetActive(false);
        isPhoneActive = false;
        StartCoroutine(StartConversation());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isPhoneActive = !isPhoneActive;
            PhoneCanvas.SetActive(isPhoneActive);

            if (isPhoneActive)
            {
                inputField.ActivateInputField();
            }
        }
    }

    IEnumerator StartConversation()
    {
        yield return new WaitForSeconds(2);
        AddMessage("¿Has podido entrar?", false);
        yield return new WaitForSeconds(5);
        AddMessage("Sí, ha sido más fácil de lo que pensaba.", true);
        yield return new WaitForSeconds(5);
        AddMessage("Perfecto, ten cuidado.", false);
        yield return new WaitForSeconds(5);
        AddMessage("No hay nada de qué preocuparse, es un simple hospital abandonado.", true);
    }

    public void SendMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            AddMessage(inputField.text, true);
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

    void AddMessage(string text, bool isPlayer)
    {
        GameObject message = Instantiate(isPlayer ? PlayerMessagePrefab : NPCMessagePrefab, chatContent);
        TMP_Text messageText = message.GetComponentInChildren<TMP_Text>();
        messageText.text = text;
        messageText.enableAutoSizing = true;
        messageText.alignment = isPlayer ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContent.GetComponent<RectTransform>());
        StartCoroutine(ScrollToBottom());

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
        chatScroll.verticalNormalizedPosition = 0f;
    }
}
