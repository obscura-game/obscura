using UnityEngine;

public class PlayerPickUpController : MonoBehaviour
{
    // Asigna en el Inspector el ObjectContainer que es hijo del player y está en la posición deseada
    public Transform objectContainer;
    public float dropForwardForce = 2f;
    public float dropUpwardForce = 1f;

    public GameObject on;
    public GameObject off;
    private bool isOn;

    public GameObject currentObject;
    private Rigidbody currentRb;
    private Collider objCollider;
    private bool equipped;
    private InteractionRaycaster raycaster;

    // Guarda la escala original del objeto recogido
    private Vector3 objectOriginalScale;

    private void Start()
    {
        raycaster = GetComponent<InteractionRaycaster>();
        on.SetActive(false);
        off.SetActive(true);
        isOn = false;
    }

    private void Update()
    {
        // Presiona E para recoger (o intercambiar) objeto
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject detectedObject = raycaster.GetDetectedObject();
            if (detectedObject != null && detectedObject.CompareTag("PickUp"))
            {
                currentObject = detectedObject;
                Debug.Log(detectedObject.name);
                Debug.Log(currentObject.name);
                if (!equipped)
                {
                    PickUp(detectedObject);
                }
                else
                {
                    Drop();
                    PickUp(detectedObject);
                }
            }
        }

        // Cambia entre on/off con F
        if (Input.GetKeyDown(KeyCode.F) && equipped && (currentObject.name == "Flashlight"))
        {
            if (isOn)
            {
                on.SetActive(false);
                off.SetActive(true);
            }
            else
            {
                on.SetActive(true);
                off.SetActive(false);
            }
            isOn = !isOn;
        }

        // Presiona G para soltar el objeto
        if (Input.GetKeyDown(KeyCode.G) && equipped)
        {
            Drop();
        }

        // Si el objeto está recogido, lo posicionamos y orientamos según el ObjectContainer
        if (equipped && currentObject != null)
        {
            // Posición: exactamente la del ObjectContainer.
            currentObject.transform.position = objectContainer.position;

            // Rotación: usamos la rotación del container, pero reemplazamos el ángulo X por el de la cámara.
            Vector3 containerEuler = objectContainer.eulerAngles;
            containerEuler.x = raycaster.cam.eulerAngles.x;
            currentObject.transform.rotation = Quaternion.Euler(containerEuler);
        }

    }

    private void PickUp(GameObject obj)
    {
        equipped = true;
        currentObject = obj;
        currentRb = currentObject.GetComponent<Rigidbody>();

        // Guarda la escala original del objeto
        objectOriginalScale = currentObject.transform.localScale;

        // Deshabilita el collider para evitar colisiones mientras está en "modo recogido"
        objCollider = currentObject.GetComponent<Collider>();
        if (objCollider != null)
        {
            objCollider.enabled = false;
        }

        // Configura la física para que el objeto se mueva de forma controlada
        if (currentRb != null)
        {
            currentRb.isKinematic = true;
            currentRb.useGravity = false;
        }
    }

    private void Drop()
    {
        equipped = false;

        if (currentRb != null)
        {
            if (objCollider != null)
                objCollider.enabled = true;
            currentRb.isKinematic = false;
            currentRb.useGravity = true;
            currentRb.velocity = GetComponent<Rigidbody>().velocity;
            currentRb.AddForce(
                raycaster.cam.forward * dropForwardForce + raycaster.cam.up * dropUpwardForce,
                ForceMode.Impulse
            );
            currentRb.AddTorque(Random.onUnitSphere * 10, ForceMode.Impulse);
        }

        currentObject = null;
        currentRb = null;
    }
}
