using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpController : MonoBehaviour
{
    public Transform objectContainer, cam;
    public float pickUpRange = 2f;
    public float dropForwardForce, dropUpwardForce;

    private GameObject currentObject; // El objeto actualmente interactuado
    private Rigidbody currentRb;
    private bool equipped;
    public static bool slotFull;

    private void Update()
    {
        // Detectar si el jugador está mirando a un objeto interactuable
        DetectObject();

        // Recoger el objeto si "E" es presionado
        if (Input.GetKeyDown(KeyCode.E) && currentObject != null && !slotFull)
        {
            Debug.Log("Objeto detectado y está dentro del rango de pickup. Intentando recoger...");
            PickUp();
            Debug.Log("Equipado: " + equipped);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.LogWarning("No se detectó objeto o ya está equipado. No se puede recoger.");
        }

        // Soltar el objeto si "G" es presionado
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Intentando soltar el objeto...");
            Drop();
            Debug.Log("Equipado: " + equipped);
        }
    }

    private void DetectObject()
    {
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (!equipped && hit.transform.CompareTag("Interactable"))
            {
                currentObject = hit.transform.gameObject;
                Debug.Log("Objeto interactuable detectado: " + currentObject.name);
            }
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        // Obtener componentes del objeto actual
        currentRb = currentObject.GetComponent<Rigidbody>();
        var coll = currentObject.GetComponent<Collider>();

        // Hacer que el objeto sea hijo del contenedor y reposicionarlo
        currentObject.transform.SetParent(objectContainer);
        currentObject.transform.localPosition = Vector3.zero;
        currentObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        currentObject.transform.localScale = Vector3.one;

        // Configurar Rigidbody y Collider
        if (currentRb != null)
        {
            currentRb.isKinematic = false; // Desactivar isKinematic
            currentRb.useGravity = true;    // Activar gravedad
            currentRb.velocity = Vector3.zero; // Restablecer la velocidad

            Debug.Log("Rigidbody configurado correctamente.");
        }
        if (coll != null)
        {
            coll.isTrigger = false; // Desactivar isTrigger
            Debug.Log("Collider configurado correctamente.");
        }
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        Debug.Log("Eliminando relación padre-hijo del objeto.");
        // Eliminar la relación padre-hijo
        currentObject.transform.SetParent(null);

        // Restaurar Rigidbody y Collider
        if (currentRb != null)
        {
            currentRb.isKinematic = false;
            currentRb.useGravity = true;    // Activar gravedad
            currentRb.velocity = GetComponent<Rigidbody>().velocity; // Añadir momento del jugador

            // Añadir fuerzas para "lanzar" el objeto
            currentRb.AddForce(cam.forward * dropForwardForce, ForceMode.Impulse);
            currentRb.AddForce(cam.up * dropUpwardForce, ForceMode.Impulse);

            // Añadir rotación aleatoria
            float random = Random.Range(-1f, 1f);
            currentRb.AddTorque(new Vector3(random, random, random) * 10);

            Debug.Log("Rigidbody y fuerzas aplicadas correctamente.");
        }

        // Limpiar variables
        currentObject = null;
        currentRb = null;
    }
}