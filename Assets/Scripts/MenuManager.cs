using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuCanvas; // Canvas del menú principal
    public GameObject historiaCanvas; // Canvas de la historia
    public TextMeshProUGUI historiaText; // Texto de la historia
    public Button nextButton; // Botón para avanzar el diálogo

    private int dialogoIndex = 0;
    private string[] dialogos = new string[]
    {
        "En el año 1987, un hospital fue abandonado tras una serie de eventos inexplicables...",
        "Muchos dicen que los pasillos aún guardan secretos ocultos...",
        "Pero nadie que ha entrado, ha salido para contar la verdad...",
        "Ahora, eres tú quien ha decidido entrar..."
    };

    void Start()
    {
        historiaCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        
        // Asegurar que el botón tiene el listener correctamente asignado
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(AvanzarDialogo);
        }
        else
        {
            Debug.LogError("NextButton no está asignado en el Inspector.");
        }

        if (historiaText == null)
        {
            Debug.LogError("HistoriaText no está asignado en el Inspector.");
        }
    }

    public void StartGame()
    {
        Debug.Log("Juego iniciado");
        mainMenuCanvas.SetActive(false);
        historiaCanvas.SetActive(true);
        dialogoIndex = 0;
        if (historiaText != null)
        {
            historiaText.text = dialogos[dialogoIndex];
            Debug.Log("Primer texto mostrado: " + historiaText.text);
        }
    }

    public void AvanzarDialogo()
{
    Debug.Log("Botón presionado. Índice actual antes de cambiar: " + dialogoIndex);

    if (dialogoIndex < dialogos.Length - 1)
    {
        dialogoIndex++;
        if (historiaText != null)
        {
            historiaText.text = dialogos[dialogoIndex];
            Debug.Log("Nuevo diálogo mostrado: " + historiaText.text);
        }
    }
    else
    {
        Debug.Log("Último diálogo alcanzado, cambiando de escena.");
        StartCoroutine(CargarEscenaJuego());
    }
}


    IEnumerator CargarEscenaJuego()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main"); // Asegurar que "Juego" es el nombre exacto de la escena
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}