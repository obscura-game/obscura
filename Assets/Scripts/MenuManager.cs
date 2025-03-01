using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuCanvas; // Canvas del men� principal
    public GameObject historiaCanvas; // Canvas de la historia
    public TextMeshProUGUI historiaText; // Texto de la historia
    public Button nextButton; // Bot�n para avanzar el di�logo

    private int dialogoIndex = 0;
    private string[] dialogos = new string[]
    {
        "En el a�o 1987, un hospital fue abandonado tras una serie de eventos inexplicables...",
        "Muchos dicen que los pasillos a�n guardan secretos ocultos...",
        "Pero nadie que ha entrado, ha salido para contar la verdad...",
        "Ahora, eres t� quien ha decidido entrar..."
    };

    void Start()
    {
        historiaCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        
        // Asegurar que el bot�n tiene el listener correctamente asignado
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(AvanzarDialogo);
        }
    }

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false);
        historiaCanvas.SetActive(true);
        dialogoIndex = 0;
        historiaText.text = dialogos[dialogoIndex];
    }

    public void AvanzarDialogo()
    {
        Debug.Log("Bot�n presionado. �ndice actual: " + dialogoIndex);
        if (dialogoIndex < dialogos.Length - 1)
        {
            dialogoIndex++;
            historiaText.text = dialogos[dialogoIndex];
            Debug.Log("Nuevo di�logo: " + historiaText.text);
        }
        else
        {
            StartCoroutine(CargarEscenaJuego());
        }
    }

    IEnumerator CargarEscenaJuego()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Juego"); // Asegurar que "Juego" es el nombre exacto de la escena
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}