using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public UnityEvent onPlayerEnter; // Evento que se activará cuando el jugador entre
    public int estadoRequerido = 0;  // Estado requerido para activar este trigger
    private bool activado = false;   // Para asegurarse de que el evento solo se active una vez

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            // Verifica si el jugador está en el estado requerido
            if (!activado && TriggerManager.instance.EstaEnEstado(estadoRequerido))
            {
                Debug.Log($"Jugador detectado en la zona. Activando evento para estado {estadoRequerido}...");
                activado = true;
                onPlayerEnter.Invoke(); // Activa el evento

                // Avanza al siguiente estado
                TriggerManager.instance.AvanzarEstado();
            }
            else
            {
                Debug.Log($"Jugador detectado, pero no cumple con el estado requerido ({estadoRequerido}).");
            }
        }
    }
}