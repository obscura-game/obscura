using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public static TriggerManager instance; // Singleton para acceder al estado desde otros scripts

    private int estadoActual = 0; // Estado actual del jugador

    private void Awake()
    {
        // Aseg�rate de que solo haya una instancia de este script
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // M�todo para avanzar al siguiente estado
    public void AvanzarEstado()
    {
        estadoActual++;
        Debug.Log($"Avanzando al estado {estadoActual}");
    }

    // M�todo para verificar si el jugador est� en un estado espec�fico
    public bool EstaEnEstado(int estadoRequerido)
    {
        return estadoActual >= estadoRequerido;
    }
}