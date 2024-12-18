using UnityEngine;
using UnityEngine.UI;
using TMPro; // Necesario para usar TextMeshPro

public class sanity : MonoBehaviour
{
    public Slider sanitySlider;        // Referencia al slider del HUD
    public TextMeshProUGUI sanityText; // Referencia al texto que muestra la cordura

    private int maxSanity = 1000;      // Valor máximo de cordura
    public int currentSanity;         // Cordura actual

    void Start()
    {
        // Inicializar la cordura al máximo
        currentSanity = maxSanity;
        UpdateSanityHUD();
    }

    void Update()
    {
        UpdateSanityHUD();
    }

    // Método para aumentar la cordura
    public void IncreaseSanity(int amount)
    {
        if (currentSanity < maxSanity)
        {
            currentSanity += amount;
            currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
            UpdateSanityHUD();
        }
    }

    // Método para reducir la cordura
    public void ReduceSanity(int amount)
    {
        if (currentSanity > 0)
        {
            currentSanity -= amount;
            currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
            UpdateSanityHUD();
        }
    }

    // Método para actualizar el HUD
    private void UpdateSanityHUD()
    {
        // Controla que no pueda ser menor a 0 o mayor a maxSanity
        if (currentSanity < 0)
        {
            currentSanity = 0;
        }
        else if (currentSanity > maxSanity)
        {
            currentSanity = maxSanity;
        }

        // Actualiza Slider
        if (sanitySlider != null)
        {
            sanitySlider.value = (float)currentSanity / maxSanity;
        }

        // Actualiza el número
        if (sanityText != null)
        {
            sanityText.text = currentSanity.ToString();
        }
    }
}
