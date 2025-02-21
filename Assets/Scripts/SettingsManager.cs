using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Canvas settingsCanvas; // Referencia al Canvas de ajustes
    public Slider volumeSlider;   // Referencia al slider de volumen
    public Slider sensitivitySlider; // Referencia al slider de sensibilidad

    private void Start()
    {
        // Cargar los valores guardados
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("GameVolume", 0.5f); // Valor por defecto: 0.5
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 2f); // Valor por defecto: 2
        }

        // Desactivar el canvas al inicio
        if (settingsCanvas != null)
        {
            settingsCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Detectar la tecla T para alternar el canvas
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsCanvas();
        }
    }

    /// <summary>
    /// Alterna la visibilidad del canvas de ajustes.
    /// </summary>
    private void ToggleSettingsCanvas()
    {
        if (settingsCanvas != null)
        {
            bool isActive = settingsCanvas.gameObject.activeSelf;
            settingsCanvas.gameObject.SetActive(!isActive);

            // Activar/desactivar el cursor del ratón
            Cursor.visible = !isActive; // Muestra/oculta el cursor
            Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;

            // Pausar/reanudar el juego
            Time.timeScale = isActive ? 1f : 0f;

            Debug.Log($"Canvas activado: {!isActive}. Cursor visible: {Cursor.visible}. TimeScale: {Time.timeScale}");

            // Guardar los ajustes cuando se cierra el canvas
            if (isActive)
            {
                SaveSettings();
            }
        }
    }

    /// <summary>
    /// Guarda los ajustes actuales en PlayerPrefs.
    /// </summary>
    private void SaveSettings()
    {
        if (volumeSlider != null)
        {
            PlayerPrefs.SetFloat("GameVolume", volumeSlider.value);
            Debug.Log($"Volumen guardado: {volumeSlider.value}");
        }

        if (sensitivitySlider != null)
        {
            PlayerPrefs.SetFloat("MouseSensitivity", sensitivitySlider.value);
            Debug.Log($"Sensibilidad guardada: {sensitivitySlider.value}");
        }

        PlayerPrefs.Save(); // Guarda los cambios en disco
    }
}