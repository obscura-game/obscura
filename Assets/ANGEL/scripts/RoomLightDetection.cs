using UnityEngine;

public class RoomLightDetection : MonoBehaviour
{
    public sanity sanityScript;          // Referencia al script de cordura
    public int sanityIncreaseAmount = 10; // Cantidad para aumentar la cordura si la luz est� encendida
    public int sanityDecreaseAmount = 10; // Cantidad para reducir la cordura si la luz est� apagada

    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si el jugador entra en la habitaci�n
        if (other.CompareTag("Player"))
        {
            Light roomLight = GetRoomLight();
            if (roomLight != null)
            {
                if (roomLight.enabled)
                {
                    // Si la luz est� encendida, aumenta la cordura
                    sanityScript.IncreaseSanity(sanityIncreaseAmount);
                }
                else
                {
                    // Si la luz est� apagada, reduce la cordura
                    sanityScript.ReduceSanity(sanityDecreaseAmount);
                }
            }
        }
    }

    // Obtiene el componente Light del objeto LightSpot en la jerarqu�a de esta habitaci�n
    private Light GetRoomLight()
    {
        // Busca el objeto LightSpot din�micamente desde el objeto de la habitaci�n
        Transform lightSpot = transform.Find("ceiling/light/lightSpot");
        if (lightSpot != null)
        {
            return lightSpot.GetComponent<Light>();
        }
        Debug.LogWarning("No se encontr� LightSpot en la habitaci�n: " + gameObject.name);
        return null;
    }
}
