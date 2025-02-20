using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public int estadoRequerido = 0;  // Estado requerido para activar este trigger
    public bool aumentarEstado = true; // Determina si este trigger aumenta el estado al ser activado
    private bool activado = false;   // Para asegurarse de que el evento solo se active una vez

    [Tooltip("El evento que se activar� cuando el jugador entre en el trigger.")]
    public UnityEvent onPlayerEnter; // Evento personalizable desde el Inspector

    /// <summary>
    /// Activa el trigger manualmente desde el Inspector.
    /// </summary>
    [ContextMenu("Activar Trigger Manualmente")]
    public void ActivarManualmente()
    {
        if (!activado && TriggerManager.instance.EstaEnEstado(estadoRequerido))
        {
            Debug.Log($"Trigger activado manualmente para estado {estadoRequerido}...");
            activado = true;

            // Invoca el evento configurado
            if (onPlayerEnter != null)
            {
                onPlayerEnter.Invoke();
                Debug.Log("Evento activado manualmente.");
            }
            else
            {
                Debug.LogWarning("No se ha asignado ning�n evento a este TriggerZone.");
            }

            // Avanza al siguiente estado solo si est� configurado para hacerlo
            if (aumentarEstado)
            {
                TriggerManager.instance.AvanzarEstado();
                Debug.Log("Estado avanzado manualmente.");
            }
            else
            {
                Debug.Log("Este trigger no avanza el estado.");
            }
        }
        else
        {
            Debug.Log("El trigger ya fue activado o el estado requerido no se cumple.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            // Verifica si el jugador est� en el estado requerido
            if (!activado && TriggerManager.instance.EstaEnEstado(estadoRequerido))
            {
                Debug.Log($"Jugador detectado en la zona. Activando evento para estado {estadoRequerido}...");
                activado = true;

                // Invoca el evento configurado
                if (onPlayerEnter != null)
                {
                    onPlayerEnter.Invoke();
                    Debug.Log("Evento activado.");
                }
                else
                {
                    Debug.LogWarning("No se ha asignado ning�n evento a este TriggerZone.");
                }

                // Avanza al siguiente estado solo si est� configurado para hacerlo
                if (aumentarEstado)
                {
                    TriggerManager.instance.AvanzarEstado();
                    Debug.Log("Estado avanzado.");
                }
                else
                {
                    Debug.Log("Este trigger no avanza el estado.");
                }
            }
            else
            {
                Debug.Log($"Jugador detectado, pero no cumple con el estado requerido ({estadoRequerido}).");
            }
        }
    }
}