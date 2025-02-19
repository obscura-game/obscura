using UnityEngine;

public class LightController : MonoBehaviour
{
    // Método para activar todas las luces con el tag "GenLights"
    public void EncenderLuces()
    {
        // Buscar todos los objetos con el tag "GenLights"
        GameObject[] luces = GameObject.FindGameObjectsWithTag("GenLights");

        foreach (GameObject luz in luces)
        {
            // Obtener el componente Light del objeto
            Light lightComponent = luz.GetComponent<Light>();

            if (lightComponent != null)
            {
                lightComponent.enabled = true; // Activar la luz
                Debug.Log($"Luz activada: {luz.name}");
            }
            else
            {
                Debug.LogWarning($"El objeto {luz.name} tiene el tag 'GenLights', pero no tiene un componente Light.");
            }
        }
    }

    // Método para apagar todas las luces con el tag "GenLights" (opcional)
    public void ApagarLuces()
    {
        // Buscar todos los objetos con el tag "GenLights"
        GameObject[] luces = GameObject.FindGameObjectsWithTag("GenLights");

        foreach (GameObject luz in luces)
        {
            // Obtener el componente Light del objeto
            Light lightComponent = luz.GetComponent<Light>();

            if (lightComponent != null)
            {
                lightComponent.enabled = false; // Desactivar la luz
                Debug.Log($"Luz desactivada: {luz.name}");
            }
            else
            {
                Debug.LogWarning($"El objeto {luz.name} tiene el tag 'GenLights', pero no tiene un componente Light.");
            }
        }
    }
}