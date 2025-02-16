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
    public AudioSource notificationSound;
    public PlayerController playerController;

    private bool isPhoneActive = false;
    private bool firstPlayerResponse = false;

    void Start()
    {
        PhoneCanvas.SetActive(false);
        isPhoneActive = false;
        StartCoroutine(DelayedNPCMessage());
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

        if (isPhoneActive && Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
        }
    }

    public void SendMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            string playerMessage = inputField.text;
            AddMessage(playerMessage, true);
            inputField.text = "";
            inputField.ActivateInputField();

            if (!firstPlayerResponse && playerMessage.ToLower().Contains("si ya estoy aqui"))
            {
                firstPlayerResponse = true;
                StartCoroutine(NPCResponse("Vale, necesito que hagas lo siguiente"));
            }
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

        if (!isPlayer && notificationSound != null)
        {
            notificationSound.Play();
        }
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        chatScroll.verticalNormalizedPosition = 0f;
    }

    IEnumerator DelayedNPCMessage()
    {
        yield return new WaitForSeconds(10);
        AddMessage("Ya has llegado?", false);
        if (notificationSound != null)
        {
            notificationSound.Play();
        }
    }

    IEnumerator NPCResponse(string response)
    {
        yield return new WaitForSeconds(Random.Range(3, 8));
        AddMessage(response, false);
        if (notificationSound != null)
        {
            notificationSound.Play();
        }
    }
}
