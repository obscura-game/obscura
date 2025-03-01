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
        Debug.Log("Botón presionado. Índice actual: " + dialogoIndex);
        if (dialogoIndex < dialogos.Length - 1)
        {
            dialogoIndex++;
            historiaText.text = dialogos[dialogoIndex];
            Debug.Log("Nuevo diálogo: " + historiaText.text);
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