using UnityEngine;

public class EnemyDisappear : MonoBehaviour, ITriggerAction
{
    [Tooltip("Tiempo en segundos antes de que el enemigo desaparezca.")]
    public float timeToDisappear = 5f; // Tiempo por defecto

    public void Activate()
    {
        Debug.Log($"Desaparición activada. El enemigo desaparecerá en {timeToDisappear} segundos.");
        Invoke("Disappear", timeToDisappear); // Programa la desaparición
    }

    private void Disappear()
    {
        Debug.Log("El enemigo ha desaparecido.");
        gameObject.SetActive(false); // Desactiva el objeto del enemigo
    }
}