using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    public GameObject sodaPrefab; // Prefab del refresco
    public Transform spawnPoint;  // Punto donde aparecerá el refresco
    public float cooldown = 5f;   // Tiempo de espera entre interacciones

    private bool canDispense = true; // Indica si se puede dispensar un refresco

    /// <summary>
    /// Dispensa un refresco en el punto de spawn.
    /// </summary>
    public void DispenseSoda()
    {
        if (!canDispense)
        {
            Debug.Log("Espera un momento antes de dispensar otro refresco.");
            return;
        }

        if (sodaPrefab != null && spawnPoint != null)
        {
            Debug.Log("Refresco dispensado.");
            Instantiate(sodaPrefab, spawnPoint.position, spawnPoint.rotation); // Spawnea el refresco
            StartCoroutine(DispenseCooldown());
        }
        else
        {
            Debug.LogError("Falta asignar el prefab del refresco o el punto de spawn.");
        }
    }

    /// <summary>
    /// Espera antes de permitir dispensar otro refresco.
    /// </summary>
    private System.Collections.IEnumerator DispenseCooldown()
    {
        canDispense = false; // Desactiva la dispensación
        yield return new WaitForSeconds(cooldown); // Espera el tiempo de cooldown
        canDispense = true; // Reactiva la dispensación
    }
}