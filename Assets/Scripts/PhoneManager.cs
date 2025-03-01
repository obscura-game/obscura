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

    public void StartConversation1Wrapper()
    {
        StartCoroutine(StartConversation1());
    }

    IEnumerator StartConversation1()
    {
        yield return new WaitForSeconds(3);
        AddMessage("Al final has entrado?", false);
        PhoneCanvas.SetActive(true);
        yield return new WaitForSeconds(3);
        AddMessage("Si si, ha sido bastante fácil", true);
        yield return new WaitForSeconds(3);
        AddMessage("Pues va, ten cuidado y que no te pillen jajaja", false);
        yield return new WaitForSeconds(3);
        AddMessage("Si va a ser un paseo", true);
        
        yield return new WaitForSeconds(10);
        ClosePhone();
    }

    public void StartConversation2Wrapper()
    {
        StartCoroutine(StartConversation2());
    }

    IEnumerator StartConversation2()
    {
        yield return new WaitForSeconds(3);
        PhoneCanvas.SetActive(true);
        AddMessage("Tú, acabo de ver que hace unos años mataron a un pavo ahí", false);
        yield return new WaitForSeconds(3);
        AddMessage("Si te digo que se me acaba de ir la luz como te quedas?", true);
        yield return new WaitForSeconds(3);
        AddMessage("Que dices loco jajajaj", false);
        yield return new WaitForSeconds(3);
        AddMessage("Que te lo digo en serio", true);
        yield return new WaitForSeconds(3);
        AddMessage("Pues busca un generador o algo por el sotano", false);

        yield return new WaitForSeconds(3);
        ClosePhone();
    }

    public void StartConversation3Wrapper()
    {
        StartCoroutine(StartConversation3());
    }

    IEnumerator StartConversation3()
    {
        yield return new WaitForSeconds(3);
        PhoneCanvas.SetActive(true);
        AddMessage("Lo acabo de encontrar pero esto no se enciende", true);
        yield return new WaitForSeconds(3);
        AddMessage("No habrá por ahí mismo gasolina o algo?", false);
        yield return new WaitForSeconds(3);
        AddMessage("Yo que sé, voy a buscar algo", true);

        yield return new WaitForSeconds(3);
        ClosePhone();
    }

    public void StartConversation4Wrapper()
    {
        StartCoroutine(StartConversation4());
    }

    IEnumerator StartConversation4()
    {
        yield return new WaitForSeconds(3);
        PhoneCanvas.SetActive(true);
        AddMessage("Has encontrado algo?", false);
        yield return new WaitForSeconds(3);
        AddMessage("Aquí hay un bidón de gasolina", true);
        yield return new WaitForSeconds(3);
        AddMessage("Pillalo y ponsela al generador, a ver si funciona", false);

        yield return new WaitForSeconds(3);
        ClosePhone();
    }

    public void StartConversation5Wrapper()
    {
        StartCoroutine(StartConversation5());
    }

    IEnumerator StartConversation5()
    {
        yield return new WaitForSeconds(3);
        PhoneCanvas.SetActive(true);
        AddMessage("YA VAN LAS LUCES", true);
        yield return new WaitForSeconds(3);
        AddMessage("Bieen, venga que no te mueres hoy", false);

        yield return new WaitForSeconds(3);
        ClosePhone();
    }

    public void StartConversation6Wrapper()
    {
        StartCoroutine(StartConversation6());
    }

    IEnumerator StartConversation6()
    {
        yield return new WaitForSeconds(3);
        PhoneCanvas.SetActive(true);
        AddMessage("Voy a largarme de aquí que no me está dando buena espina", true);
        yield return new WaitForSeconds(3);
        AddMessage("Si si, igual casi que mejor, igual va alguien por lo de las luces", false);

        yield return new WaitForSeconds(3);
        ClosePhone();
    }

    public void StartConversation7Wrapper()
    {
        StartCoroutine(StartConversation7());
    }

    IEnumerator StartConversation7()
    {
        yield return new WaitForSeconds(1);
        PhoneCanvas.SetActive(true);
        AddMessage("Tú, que no se abren las puertas", true);
        yield return new WaitForSeconds(3);
        AddMessage("Va deja de vacilarme", false);
        yield return new WaitForSeconds(3);
        AddMessage("Que te lo digo en serio", true);
        yield return new WaitForSeconds(3);
        AddMessage("Yo que sé, yo me piro ya a sobar, busca algo por ahí para salir", false);

        yield return new WaitForSeconds(3);
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

        // Borra todos los mensajes del contenedor NPC (iterando de atrás hacia adelante)
        for (int i = chatContentNPC.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(chatContentNPC.GetChild(i).gameObject);
        }

        // Borra todos los mensajes del contenedor Player (iterando de atrás hacia adelante)
        for (int i = chatContentPlayer.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(chatContentPlayer.GetChild(i).gameObject);
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContent);
    }
}
