using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("El objeto que ser� activado (por ejemplo, un enemigo).")]
    public GameObject enemyToSpawn; // Referencia al objeto que se activar�

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
                Invoke("ActivateEnemy", spawnDelay); // Programa la aparici�n
            }
            else
            {
                ActivateEnemy(); // Activa el enemigo inmediatamente
            }
        }
        else
        {
            Debug.LogError("No se ha asignado ning�n enemigo a este spawner.");
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