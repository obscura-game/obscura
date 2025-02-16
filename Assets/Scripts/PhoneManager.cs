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

    private bool isPhoneActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isPhoneActive = !isPhoneActive;
            PhoneCanvas.SetActive(isPhoneActive);
        }
    }

    public void SendMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            AddMessage(inputField.text, true);
            inputField.text = "";
            StartCoroutine(NPCResponse());
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
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        chatScroll.verticalNormalizedPosition = 0f;
    }

    IEnumerator NPCResponse()
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
        AddMessage("Estoy en camino...", false);
        if (notificationSound != null)
        {
            notificationSound.Play();
        }
    }
}
