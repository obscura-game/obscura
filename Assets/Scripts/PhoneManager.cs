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
    public GameObject Trigger;

    private bool isPhoneActive = false;

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
                inputField.DeactivateInputField();
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

        if (isPhoneActive && !inputField.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            inputField.text = "";
            inputField.ActivateInputField();
        }

        if (isPhoneActive && inputField.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
            inputField.DeactivateInputField();
        }
    }

    IEnumerator HideControlsAndStartConversation()
    {
        yield return new WaitForSeconds(10); 
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
        
        yield return new WaitForSeconds(10);
        ClosePhone();
    }

    public IEnumerator StartConversation2()
    {
        yield return new WaitForSeconds(3);
        PhoneCanvas.SetActive(true);
        AddMessage("Se han apagado las luces", true);
        yield return new WaitForSeconds(3);
        AddMessage("¿Qué ves?", false);
        yield return new WaitForSeconds(3);
        AddMessage("Las luces parpadean y escucho ruidos extraños...", true);

        yield return new WaitForSeconds(10);
        ClosePhone();
    }

    IEnumerator StartConversation3()
    {
        yield return new WaitForSeconds(3);
        PhoneCanvas.SetActive(true);
        AddMessage("Estoy dentro, pero algo no está bien...", true);
        yield return new WaitForSeconds(3);
        AddMessage("¿Qué ves?", false);
        yield return new WaitForSeconds(3);
        AddMessage("Las luces parpadean y escucho ruidos extraños...", true);

        yield return new WaitForSeconds(10);
        ClosePhone();
    }

    IEnumerator StartConversation4()
    {
        yield return new WaitForSeconds(3);
        PhoneCanvas.SetActive(true);
        AddMessage("Estoy dentro, pero algo no está bien...", true);
        yield return new WaitForSeconds(3);
        AddMessage("¿Qué ves?", false);
        yield return new WaitForSeconds(3);
        AddMessage("Las luces parpadean y escucho ruidos extraños...", true);

        yield return new WaitForSeconds(10);
        ClosePhone();
    }

    IEnumerator StartConversation5()
    {
        yield return new WaitForSeconds(3);
        PhoneCanvas.SetActive(true);
        AddMessage("Estoy dentro, pero algo no está bien...", true);
        yield return new WaitForSeconds(3);
        AddMessage("¿Qué ves?", false);
        yield return new WaitForSeconds(3);
        AddMessage("Las luces parpadean y escucho ruidos extraños...", true);

        yield return new WaitForSeconds(10);
        ClosePhone();
    }

    IEnumerator StartConversation6()
    {
        yield return new WaitForSeconds(3);
        PhoneCanvas.SetActive(true);
        AddMessage("Estoy dentro, pero algo no está bien...", true);
        yield return new WaitForSeconds(3);
        AddMessage("¿Qué ves?", false);
        yield return new WaitForSeconds(3);
        AddMessage("Las luces parpadean y escucho ruidos extraños...", true);

        yield return new WaitForSeconds(10);
        ClosePhone();
    }

    public void SendMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            AddMessage(inputField.text, true);
            inputField.text = "";
            inputField.DeactivateInputField();
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
        chatScroll.verticalNormalizedPosition = 0f;
    }

    public void ClosePhone()
    {
        PhoneCanvas.SetActive(false);
        foreach (Transform child in chatContent)
        {
            Destroy(child.gameObject);
        }
    }
  
}
