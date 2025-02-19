using UnityEngine;

public class EnemyDisappear : MonoBehaviour, ITriggerAction
{
    [Tooltip("Tiempo en segundos antes de que el enemigo desaparezca.")]
    public float timeToDisappear = 5f; // Tiempo por defecto

    public void Activate()
    {
        Debug.Log($"Desaparici�n activada. El enemigo desaparecer� en {timeToDisappear} segundos.");
        Invoke("Disappear", timeToDisappear); // Programa la desaparici�n
    }

    private void Disappear()
    {
        Debug.Log("El enemigo ha desaparecido.");
        gameObject.SetActive(false); // Desactiva el objeto del enemigo
    }
}