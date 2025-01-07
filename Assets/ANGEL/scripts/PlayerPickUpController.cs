using UnityEngine;

public class PlayerPickUpController : MonoBehaviour
{
    public Transform objectContainer, cam;
    public float pickUpRange = 2f;
    public float dropForwardForce, dropUpwardForce;

    private GameObject currentObject;
    private Rigidbody currentRb;
    private bool equipped;

    private void Update()
    {
        DetectObject();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!equipped && currentObject != null)
                PickUp();
            else if (equipped)
                Debug.LogWarning("Ya tienes un objeto equipado.");
            else
                Debug.LogWarning("No hay ningún objeto interactuable cerca.");
        }

        if (Input.GetKeyDown(KeyCode.G) && equipped)
            Drop();

        if (equipped && currentObject != null)
        {
            currentObject.transform.rotation = Quaternion.Euler(
                cam.eulerAngles.x + 90, 
                cam.eulerAngles.y, 
                0
            );
        }
    }

    private void DetectObject()
    {
        if (equipped || currentObject != null) return;

        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, pickUpRange) && hit.transform.CompareTag("Interactable"))
        {
            currentObject = hit.transform.gameObject;
            Debug.Log("Objeto interactuable detectado: " + currentObject.name);
        }
    }

    private void PickUp()
    {
        equipped = true;
        currentRb = currentObject.GetComponent<Rigidbody>();
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
            currentRb.AddForce(cam.forward * dropForwardForce + cam.up * dropUpwardForce, ForceMode.Impulse);
            currentRb.AddTorque(Random.onUnitSphere * 10, ForceMode.Impulse);
        }

        currentObject = null;
        currentRb = null;
    }
}