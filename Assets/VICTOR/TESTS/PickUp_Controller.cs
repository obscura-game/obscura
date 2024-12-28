using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp_Controller : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, objectContainer, cam;

    public float pickUpRange = 2f;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private Vector3 initialScale;

    public void Start()
    {
        // Store the initial scale of the object
        initialScale = transform.localScale;

        // Setup
        if (!equipped) 
        {
            rb.isKinematic = false;
            coll.isTrigger = false;
        }

        if (equipped)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    private void Update()
    {
        // Check if the player is looking at the object and "E" is pressed
        if (!equipped && Input.GetKeyDown(KeyCode.E) && !slotFull && IsPlayerLookingAtObject())
        {
            PickUp();
        }

        // Drop if equipped and "G" is pressed
        if (equipped && Input.GetKeyDown(KeyCode.G))
        {
            Drop();
        }
    }

    private bool IsPlayerLookingAtObject()
    {
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            return hit.transform == transform;
        }
        return false;
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        // Make object a child of the camera and move it to default position
        transform.SetParent(objectContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        // Check the tag and apply the appropriate rotation
        if (CompareTag("Flashlight"))
        {
            transform.localRotation = Quaternion.Euler(0, 0, 5);
        }
        else if (CompareTag("Object"))
        {
            transform.localRotation = Quaternion.Euler(-90, 0, -175);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        // Restore initial scale
        transform.localScale = initialScale;

        // Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        // Set parent to null
        transform.SetParent(null);

        // Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        // Restore initial scale
        transform.localScale = initialScale;

        // Object carries momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        // AddForce
        rb.AddForce(cam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(cam.up * dropUpwardForce, ForceMode.Impulse);

        // Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
    }
}
