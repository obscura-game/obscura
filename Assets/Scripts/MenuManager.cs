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
        "Esto ocurrió un 19 de Enero del 98, siempre he sido un chico al que le gustan las aventuras y más si hablamos de colarme en sitios abandonados...",
        "llevaba ya unos meses con el objetivo de colarme en el antiguo ambulatorio de Burjassot, ya que corría el rumor de que lo iban a derruir...",
        "y después de un par de días de comprobar si había algo de seguridad, decidí entrar...",
        "lo que me pasó allí nunca lo podré olvidar."
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