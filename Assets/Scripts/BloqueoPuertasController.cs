using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueoPuertasController : MonoBehaviour
{
    private Collider objectCollider; // Referencia al Collider del objeto

    private void Start()
    {
        // Obtener el componente Collider del objeto
        objectCollider = GetComponent<Collider>();
        if (objectCollider == null)
        {
            Debug.LogError("No se encontró ningún Collider en este GameObject.");
        }
    }

    /// <summary>
    /// Desactiva el Collider del objeto.
    /// </summary>
    public void DisableCollider()
    {
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
            Debug.Log("Collider desactivado.");
        }
        else
        {
            Debug.LogError("El Collider no está asignado.");
        }
    }

    /// <summary>
    /// Activa el Collider del objeto.
    /// </summary>
    public void EnableCollider()
    {
        if (objectCollider != null)
        {
            objectCollider.enabled = true;
            Debug.Log("Collider activado.");
        }
        else
        {
            Debug.LogError("El Collider no está asignado.");
        }
    }
}
