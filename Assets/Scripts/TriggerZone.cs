using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public UnityEvent onPlayerEnter; // Evento que se activará cuando el jugador entre
    public int estadoRequerido = 0;  // Estado requerido para activar este trigger
    public bool aumentarEstado = true; // Determina si este trigger aumenta el estado al ser activado
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

                // Invoca el evento
                onPlayerEnter.Invoke();

                // Si necesitas acceder directamente al personaje, puedes hacerlo aquí
                MoverPersonaje moverPersonaje = other.GetComponent<MoverPersonaje>();
                if (moverPersonaje != null)
                {
                    moverPersonaje.ActivarMovimiento();
                }

                // Avanza al siguiente estado solo si está configurado para hacerlo
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