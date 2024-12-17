using UnityEngine;
using UnityEngine.UI;

public class sanity : MonoBehaviour
{
    public Slider sanitySlider; // Referencia al slider del HUD
    public Text sanityText;     // Referencia al texto que muestra la cordura

    private int maxSanity = 1000; // Valor máximo de cordura
    private int currentSanity;   // Cordura actual

    void Start()
    {
        // Inicializar la cordura al máximo
        currentSanity = maxSanity;
        UpdateSanityHUD();
    }

    // Método para reducir la cordura
    public void ReduceSanity(int amount)
    {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
        UpdateSanityHUD();
    }

    // Método para actualizar el HUD
    private void UpdateSanityHUD()
    {
        if (sanitySlider != null)
        {
            sanitySlider.value = (float)currentSanity / maxSanity;
        }

        if (sanityText != null)
        {
            sanityText.text = "Cordura: " + currentSanity;
        }
    }
}
