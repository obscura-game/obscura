using UnityEngine;

public class SodaController : MonoBehaviour
{
    public GameObject canvas; // Referencia al Canvas
    public int sanityBoost = 100; // Cuánto aumenta la cordura al beber (entero)

    private bool isHeld = false; // Indica si la soda está siendo llevada por el jugador

    /// <summary>
    /// Recoge la soda y la coloca en la mano del jugador.
    /// </summary>
    public void PickUp(Transform player)
    {
        isHeld = true;
        Debug.Log("Soda recogida.");
    }

    private void Update()
    {
        // Consumir la soda con la tecla F si está en la mano
        if (isHeld && Input.GetKeyDown(KeyCode.F))
        {
            Consume();
        }
    }

    /// <summary>
    /// Consume la soda y aumenta la cordura.
    /// </summary>
    private void Consume()
    {
        if (canvas != null)
        {
            Sanity sanityScript = canvas.GetComponent<Sanity>(); // Obtener el script Sanity del Canvas
            if (sanityScript != null)
            {
                sanityScript.IncreaseSanity(sanityBoost); // Aumentar la cordura
                Debug.Log($"Soda consumida. Cordura aumentada en {sanityBoost}.");
            }
            else
            {
                Debug.LogError("No se encontró el script Sanity en el Canvas.");
            }
        }
        else
        {
            Debug.LogError("No se asignó ningún Canvas en el Inspector.");
        }

        Destroy(gameObject); // Destruir la soda después de consumirla
        isHeld = false;
    }
}