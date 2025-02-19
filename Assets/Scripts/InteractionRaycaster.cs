using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importa el namespace de TextMeshPro

public class InteractionRaycaster : MonoBehaviour
{
    public Transform cam;                  // Cámara del jugador
    public float interactionRange = 2f;    // Rango del raycast
    public Image crosshairImage;           // Referencia al crosshair
    public Sprite defaultCrosshair;        // Crosshair por defecto
    public Sprite pickupCrosshair;         // Crosshair para objetos recogibles
    public Sprite doorCrosshair;           // Crosshair para puertas
    public GameObject bidonGasolina;       // Referencia al bidón de gasolina que el jugador sostiene
    public TMP_Text mensajeUI;             // Referencia al texto de la UI (TextMeshPro)
    private GameObject detectedObject;     // Objeto detectado actualmente
    private int interactableLayerMask;     // Máscara de capa para objetos interactuables
    private bool generadorArreglado = false; // Indica si el generador ya ha sido arreglado

    public GameObject GetDetectedObject() => detectedObject;

    private void Start()
    {
        // Obtener el valor de la capa "Interactable"
        interactableLayerMask = LayerMask.GetMask("Interactable");

        // Validar que los componentes necesarios estén asignados
        if (cam == null)
        {
            Debug.LogError("La cámara no está asignada en el Inspector.");
        }

        if (crosshairImage == null)
        {
            Debug.LogError("El crosshair no está asignado en el Inspector.");
        }

        if (mensajeUI == null)
        {
            Debug.LogError("El componente TMP_Text no está asignado en el Inspector.");
        }
        else
        {
            // Desactivar el mensaje inicialmente
            mensajeUI.gameObject.SetActive(false);
        }

        // Asegurarse de que el crosshair esté activo y muestre el sprite por defecto
        if (crosshairImage != null)
        {
            crosshairImage.sprite = defaultCrosshair;
            crosshairImage.enabled = true;
        }
    }

    private void Update()
    {
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        // Realizar el raycast solo en la capa "Interactable"
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayerMask))
        {
            detectedObject = hit.transform.gameObject;

            // Cambiar el crosshair según el tag del objeto
            if (detectedObject.CompareTag("PickUp"))
                crosshairImage.sprite = pickupCrosshair;
            else if (detectedObject.CompareTag("Door"))
                crosshairImage.sprite = doorCrosshair;
            else
                crosshairImage.sprite = defaultCrosshair;

            // Verificar si el jugador hace clic y sostiene el bidón de gasolina
            if (Input.GetMouseButtonDown(0)) // 0 es el botón izquierdo del ratón
            {
                if (bidonGasolina != null && bidonGasolina.activeInHierarchy) // Si el bidón está activo (sostenido)
                {
                    if (detectedObject.CompareTag("Generador") && !generadorArreglado) // Verificar si el objeto es el generador y no ha sido arreglado
                    {
                        ArreglarGenerador();
                    }
                }
            }
        }
        else
        {
            // Si no se detecta un objeto interactuable, mostrar el crosshair por defecto
            detectedObject = null;
            crosshairImage.sprite = defaultCrosshair;
        }
    }

    private void ArreglarGenerador()
    {
        generadorArreglado = true; // Marcar el generador como arreglado
        Debug.Log("Generador arreglado");

        // Mostrar el mensaje en la UI usando TextMeshPro
        if (mensajeUI != null)
        {
            mensajeUI.text = "Generador arreglado";
            mensajeUI.gameObject.SetActive(true);

            // Ocultar el mensaje después de 2 segundos
            Invoke("OcultarMensaje", 2f);
        }

        // Encender las luces con el tag "GenLights"
        LightController lightController = FindObjectOfType<LightController>();
        if (lightController != null)
        {
            lightController.EncenderLuces();
        }
        else
        {
            Debug.LogError("No se encontró ningún LightController en la escena.");
        }
    }

    private void OcultarMensaje()
    {
        if (mensajeUI != null)
        {
            mensajeUI.gameObject.SetActive(false);
        }
    }
}