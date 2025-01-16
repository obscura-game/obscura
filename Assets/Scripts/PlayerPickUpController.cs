using UnityEngine;

public class PlayerPickUpController : MonoBehaviour
{
    public Transform objectContainer;
    public float dropForwardForce = 2f;
    public float dropUpwardForce = 1f;

    private GameObject currentObject;
    private Rigidbody currentRb;
    private bool equipped;
    private InteractionRaycaster raycaster;

    private void Start()
    {
        raycaster = GetComponent<InteractionRaycaster>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject detectedObject = raycaster.GetDetectedObject();

            if (detectedObject != null && detectedObject.CompareTag("PickUp"))
            {
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (equipped)
            {
                Drop();
            }
        }

        // Mantener la rotación del objeto recogido alineada con la cámara
        if (equipped && currentObject != null)
        {
            currentObject.transform.rotation = Quaternion.Euler(
                raycaster.cam.eulerAngles.x,
                raycaster.cam.eulerAngles.y,
                0
            );
        }
    }

    private void PickUp(GameObject obj)
    {
        equipped = true;
        currentObject = obj;
        currentRb = currentObject.GetComponent<Rigidbody>();

        // Configurar el objeto recogido
        currentObject.transform.SetParent(objectContainer);
        currentObject.transform.localPosition = Vector3.zero;
        currentObject.transform.localRotation = Quaternion.identity;
        currentObject.transform.localScale = Vector3.one;

        if (currentRb != null)
        {
            currentRb.isKinematic = true;
            currentRb.useGravity = false;
        }
    }

    private void Drop()
    {
        equipped = false;
        currentObject.transform.SetParent(null);

        if (currentRb != null)
        {
            currentRb.isKinematic = false;
            currentRb.useGravity = true;
            currentRb.velocity = GetComponent<Rigidbody>().velocity;
            currentRb.AddForce(raycaster.cam.forward * dropForwardForce + raycaster.cam.up * dropUpwardForce, ForceMode.Impulse);
            currentRb.AddTorque(Random.onUnitSphere * 10, ForceMode.Impulse);
        }

        currentObject = null;
        currentRb = null;
    }
}