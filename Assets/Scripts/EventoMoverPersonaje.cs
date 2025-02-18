using UnityEngine;

public class MoverPersonaje : MonoBehaviour
{
    public float velocidad = 2.0f; // Velocidad de movimiento
    private bool mover = false;    // Controla si el personaje debe moverse
    private Animator animator;     // Referencia al Animator del personaje

    private void Start()
    {
        // Obt�n el componente Animator del personaje
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("El personaje no tiene un Animator asignado.");
        }
    }

    public void ActivarMovimiento()
    {
        mover = true;
        Debug.Log("Movimiento activado. Reproduciendo animaci�n DrunkMove.");

        // Activa directamente la animaci�n DrunkMove
        if (animator != null)
        {
            animator.Play("Walking"); // Reproduce la animaci�n por su nombre
        }
    }

    private void Update()
    {
        if (mover)
        {
            // Mueve al personaje hacia adelante
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        }
    }

    public void DetenerMovimiento()
    {
        mover = false;
        Debug.Log("Movimiento detenido. Volviendo a Idle.");

        // Detiene el movimiento y reproduce una animaci�n Idle (opcional)
        if (animator != null)
        {
            animator.Play("Idle"); // Reproduce la animaci�n Idle por su nombre
        }
    }
}