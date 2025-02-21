using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Vector3 hingeOffset = new Vector3(-0.5f, 0, 0); // Ajusta la posici�n de la bisagra seg�n el tama�o de la puerta
    public float openAngle = 90f; // �ngulo m�ximo de apertura
    public float speed = 2f; // Velocidad de apertura/cierre
    private bool isOpen = false;
    private float currentAngle = 0f;

    private Vector3 hingePosition;

    void Start()
    {
        // Calculamos la posici�n de la bisagra en el mundo
        hingePosition = transform.position + transform.TransformVector(hingeOffset);
    }

    void Update()
    {
        // Pulsar "E" para abrir/cerrar la puerta
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
        }

        // Calcular el �ngulo objetivo
        float targetAngle = isOpen ? openAngle : 0f;

        // Suavizar la rotaci�n
        float step = speed * Time.deltaTime;
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, step);

        // Rotar la puerta alrededor de la bisagra
        transform.RotateAround(hingePosition, Vector3.up, currentAngle - transform.eulerAngles.y);
    }
}
