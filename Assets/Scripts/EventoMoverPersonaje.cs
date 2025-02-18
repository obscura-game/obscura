using UnityEngine;

public class MoverPersonaje : MonoBehaviour
{
    public float velocidad = 2.0f; // Velocidad de movimiento
    public float segundosParaDestruir = 3.0f; // Tiempo en segundos antes de destruir el personaje
    private bool mover = false;    // Controla si el personaje debe moverse
    private Animator animator;     // Referencia al Animator del personaje

    private void Start()
    {
        // Obtén el componente Animator del personaje
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("El personaje no tiene un Animator asignado.");
        }
    }

    /// <summary>
    /// Activa el movimiento y programa la destrucción del personaje después de un tiempo.
    /// </summary>
    public void ActivarMovimiento()
    {
        mover = true;
        Debug.Log("Movimiento activado. Reproduciendo animación DrunkMove.");

        // Activa directamente la animación DrunkMove
        if (animator != null)
        {
            animator.Play("DrunkMove"); // Reproduce la animación por su nombre
        }

        // Programa la destrucción del personaje después de los segundos especificados
        Invoke("DestruirPersonaje", segundosParaDestruir);
    }

    private void Update()
    {
        if (mover)
        {
            // Mueve al personaje hacia adelante
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        }
    }

    /// <summary>
    /// Detiene el movimiento y reproduce una animación Idle (opcional).
    /// </summary>
    public void DetenerMovimiento()
    {
        mover = false;
        Debug.Log("Movimiento detenido. Volviendo a Idle.");

        // Detiene el movimiento y reproduce una animación Idle (opcional)
        if (animator != null)
        {
            animator.Play("Idle"); // Reproduce la animación Idle por su nombre
        }
    }

    /// <summary>
    /// Destruye el objeto del personaje.
    /// </summary>
    private void DestruirPersonaje()
    {
        Debug.Log("El personaje ha sido destruido.");
        Destroy(gameObject); // Destruye el objeto del personaje
    }
}