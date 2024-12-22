using System.Collections;
using UnityEngine;

public class RoomLightDetection : MonoBehaviour
{
    public Sanity sanityScript;          // Referencia al script de cordura
    public int sanityIncreaseAmount = 5; // Cantidad para aumentar la cordura si la luz está encendida
    public int sanityDecreaseAmount = 10; // Cantidad para reducir la cordura si la luz está apagada
    public float sanityChangeInterval = 0.5f; // Intervalo de tiempo para cambiar la cordura (en segundos)

    private Coroutine sanityCoroutine;   // Referencia a la corutina activa

    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si el jugador entra en la habitación
        if (other.CompareTag("Player"))
        {
            Light roomLight = GetRoomLight();
            if (roomLight != null)
            {
                // Inicia la corutina para cambiar la cordura según el estado de la luz
                sanityCoroutine = StartCoroutine(ChangeSanityOverTime(roomLight));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Comprobar si el jugador sale de la habitación
        if (other.CompareTag("Player") && sanityCoroutine != null)
        {
            // Detener la corutina cuando el jugador salga de la habitación
            StopCoroutine(sanityCoroutine);
            sanityCoroutine = null;
        }
    }

    // Corutina para cambiar la cordura continuamente según el estado de la luz
    private IEnumerator ChangeSanityOverTime(Light roomLight)
    {
        while (true)
        {
            if (roomLight.enabled)
            {
                // Si la luz está encendida, aumenta la cordura
                sanityScript.IncreaseSanity(sanityIncreaseAmount);
            }
            else
            {
                // Si la luz está apagada, reduce la cordura
                sanityScript.ReduceSanity(sanityDecreaseAmount);
            }

            // Esperar el intervalo antes del siguiente cambio
            yield return new WaitForSeconds(sanityChangeInterval);
        }
    }

    // Obtiene el componente Light del objeto LightSpot en la jerarquía de esta habitación
    private Light GetRoomLight()
    {
        // Busca el objeto LightSpot dinámicamente desde el objeto de la habitación
        Transform lightSpot = transform.Find("ceiling/light/lightSpot");
        if (lightSpot != null)
        {
            return lightSpot.GetComponent<Light>();
        }
        Debug.LogWarning("No se encontró LightSpot en la habitación: " + gameObject.name);
        return null;
    }
}
