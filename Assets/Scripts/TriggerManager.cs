using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public static TriggerManager instance; // Singleton para acceder al estado desde otros scripts

    private int estadoActual = 0; // Estado actual del jugador

    private void Awake()
    {
        // Asegúrate de que solo haya una instancia de este script
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para avanzar al siguiente estado
    public void AvanzarEstado()
    {
        estadoActual++;
        Debug.Log($"Avanzando al estado {estadoActual}");
    }

    // Método para verificar si el jugador está en un estado específico
    public bool EstaEnEstado(int estadoRequerido)
    {
        return estadoActual >= estadoRequerido;
    }
}