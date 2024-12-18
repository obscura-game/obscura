using UnityEngine;

public class RoomLightDetection : MonoBehaviour
{
    public sanity sanityScript;          // Referencia al script de cordura
    public int sanityIncreaseAmount = 10; // Cantidad para aumentar la cordura si la luz está encendida
    public int sanityDecreaseAmount = 10; // Cantidad para reducir la cordura si la luz está apagada

    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si el jugador entra en la habitación
        if (other.CompareTag("Player"))
        {
            Light roomLight = GetRoomLight();
            if (roomLight != null)
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
            }
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
