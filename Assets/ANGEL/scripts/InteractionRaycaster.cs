using UnityEngine;
using UnityEngine.UI;

public class InteractionRaycaster : MonoBehaviour
{
    public Transform cam;                  // Cámara del jugador
    public float interactionRange = 2f;    // Rango del raycast
    public Image crosshairImage;           // Referencia al crosshair
    public Sprite defaultCrosshair;        // Crosshair por defecto
    public Sprite pickupCrosshair;         // Crosshair para objetos recogibles
    public Sprite doorCrosshair;           // Crosshair para puertas

    private GameObject detectedObject;     // Objeto detectado actualmente
    private int interactableLayerMask;     // Máscara de capa para objetos interactuables

    public GameObject GetDetectedObject() => detectedObject;

    private void Start()
    {
        // Obtener el valor de la capa "Interactable"
        interactableLayerMask = LayerMask.GetMask("Interactable");
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

            crosshairImage.enabled = true;
        }
        else
        {
            // Si no se detecta un objeto interactuable
            detectedObject = null;
            crosshairImage.sprite = defaultCrosshair;
            crosshairImage.enabled = true;
        }
    }
}