using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("El objeto que será activado (por ejemplo, un enemigo).")]
    public GameObject enemyToSpawn; // Referencia al objeto que se activará

    [Tooltip("Tiempo en segundos antes de que el enemigo aparezca (opcional).")]
    public float spawnDelay = 0f; // Retardo antes de activar el objeto (0 = inmediato)

    /// <summary>
    /// Activa el objeto asignado.
    /// </summary>
    public void SpawnEnemy()
    {
        if (enemyToSpawn != null)
        {
            if (spawnDelay > 0)
            {
                Debug.Log($"Enemigo programado para aparecer en {spawnDelay} segundos.");
                Invoke("ActivateEnemy", spawnDelay); // Programa la aparición
            }
            else
            {
                ActivateEnemy(); // Activa el enemigo inmediatamente
            }
        }
        else
        {
            Debug.LogError("No se ha asignado ningún enemigo a este spawner.");
        }
    }

    /// <summary>
    /// Activa el objeto del enemigo.
    /// </summary>
    private void ActivateEnemy()
    {
        enemyToSpawn.SetActive(true); // Activa el objeto
        Debug.Log("Enemigo aparecido.");
    }
}