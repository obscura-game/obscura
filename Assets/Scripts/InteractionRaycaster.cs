using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionRaycaster : MonoBehaviour
{
    public Transform cam;                  // Cámara del jugador
    public float interactionRange = 2f;    // Rango del raycast
    public Image crosshairImage;           // Referencia al crosshair
    public Sprite defaultCrosshair;        // Crosshair por defecto
    public Sprite pickupCrosshair;         // Crosshair para objetos recogibles
    public Sprite doorCrosshair;           // Crosshair para puertas
    public GameObject bidonGasolina;       // Referencia al bidón de gasolina
    private GameObject detectedObject;     // Objeto detectado actualmente
    private int interactableLayerMask;     // Máscara de capa para objetos interactuables
    private bool generadorArreglado = false; // Indica si el generador ya ha sido arreglado

    public GameObject GetDetectedObject() => detectedObject;
    private PlayerPickUpController playerPickUpController; // Referencia al script de recogida

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

        // Asegurarse de que el crosshair esté activo y muestre el sprite por defecto
        if (crosshairImage != null)
        {
            crosshairImage.sprite = defaultCrosshair;
            crosshairImage.enabled = true;
        }

        // Obtener el script PlayerPickUpController del jugador
        playerPickUpController = GetComponent<PlayerPickUpController>();
        if (playerPickUpController == null)
        {
            Debug.LogError("No se encontró el script PlayerPickUpController en el jugador.");
        }
    }

    private void Update()
    {
        // Verificar si el jugador presiona E para interactuar
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Verificar si hay un Canvas activo de alguna caja fuerte
            SafeController activeSafe = FindObjectOfType<SafeController>();
            if (activeSafe != null && activeSafe.safeCanvas != null && activeSafe.safeCanvas.activeInHierarchy)
            {
                // Si el Canvas está activo, ocúltalo
                activeSafe.HideCodeCanvas();
                return; // Salir del método para evitar otras interacciones
            }
        }

        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        // Realizar el raycast solo en la capa "Interactable"
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayerMask))
        {
            detectedObject = hit.transform.gameObject;

            // Buscar un hijo con el tag "Door" si el objeto detectado no tiene el tag
            if (!detectedObject.CompareTag("Door"))
            {
                Transform doorChild = detectedObject.transform.GetComponentInChildren<Transform>(true); // Busca en los hijos
                if (doorChild != null && doorChild.CompareTag("Door"))
                {
                    detectedObject = doorChild.gameObject;
                }
            }

            // Cambiar el crosshair según el tag del objeto
            if (detectedObject.CompareTag("PickUp"))
                crosshairImage.sprite = pickupCrosshair;
            else if (detectedObject.CompareTag("Door"))
                crosshairImage.sprite = doorCrosshair;
            else if (detectedObject.CompareTag("VendingMachine"))
                crosshairImage.sprite = doorCrosshair; // Usa el mismo crosshair o uno diferente
            else if (detectedObject.CompareTag("Generador"))
                crosshairImage.sprite = doorCrosshair;
            else if (detectedObject.CompareTag("Safe"))
                crosshairImage.sprite = doorCrosshair; // Usa el mismo crosshair o uno diferente
            else
                crosshairImage.sprite = defaultCrosshair;

            // Verificar si el jugador presiona E para interactuar
            if (Input.GetKeyDown(KeyCode.E)) // Detectar la tecla E
            {
                if (detectedObject.CompareTag("Door"))
                {
                    InteractWithDoor(detectedObject); // Interactuar con la puerta
                }
                else if (detectedObject.CompareTag("VendingMachine"))
                {
                    InteractWithVendingMachine(detectedObject); // Interactuar con la máquina expendedora
                }
                else if (detectedObject.CompareTag("Generador") && !generadorArreglado)
                {
                    // Verificar si el jugador tiene el bidón de gasolina en la mano
                    if (playerPickUpController != null && playerPickUpController.currentObject == bidonGasolina)
                    {
                        ArreglarGenerador(); // Interactuar con el generador
                    }
                    else
                    {
                        Debug.Log("No tienes el bidón de gasolina en la mano.");
                    }
                }
                else if (detectedObject.CompareTag("Safe"))
                {
                    InteractWithSafe(detectedObject);
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

    private void InteractWithSafe(GameObject safe)
    {
        SafeController safeController = safe.GetComponent<SafeController>();
        if (safeController != null)
        {
            if (safeController.safeCanvas.activeInHierarchy)
            {
                // Si el Canvas está activo, ocúltalo
                safeController.HideCodeCanvas();
            }
            else
            {
                // Si el Canvas no está activo, muéstralo
                safeController.ShowCodeCanvas();
            }
        }
        else
        {
            Debug.LogWarning("La caja fuerte no tiene un componente SafeController.");
        }
    }

    private void InteractWithDoor(GameObject door)
    {
        DoorController doorController = door.GetComponentInChildren<DoorController>();
        if (doorController != null)
        {
            if (doorController.IsDoorOpen())
            {
                Debug.Log("Cerrando la puerta...");
                doorController.CloseDoor(); // Cierra la puerta
            }
            else
            {
                Debug.Log("Abriendo la puerta...");
                doorController.OpenDoor(); // Abre la puerta
            }
        }
        else
        {
            Debug.LogWarning("La puerta detectada no tiene un componente DoorController.");
        }
    }

    /// <summary>
    /// Interactúa con una máquina expendedora.
    /// </summary>
    private void InteractWithVendingMachine(GameObject vendingMachine)
    {
        VendingMachine vendingMachineScript = vendingMachine.GetComponent<VendingMachine>();
        if (vendingMachineScript != null)
        {
            Debug.Log("Interactuando con la máquina expendedora...");
            vendingMachineScript.DispenseSoda(); // Dispensar un refresco
        }
        else
        {
            Debug.LogWarning("La máquina expendedora no tiene un componente VendingMachine.");
        }
    }

    /// <summary>
    /// Arregla el generador.
    /// </summary>
    private void ArreglarGenerador()
    {
        generadorArreglado = true; // Marcar el generador como arreglado
        Debug.Log("Generador arreglado");

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

        // Consumir el bidón de gasolina
        if (bidonGasolina != null)
        {
            bidonGasolina.SetActive(false); // Desactivar el bidón de gasolina
        }
    }
}