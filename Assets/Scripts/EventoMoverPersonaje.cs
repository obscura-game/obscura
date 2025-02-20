using UnityEngine;

public class MoverPersonaje : MonoBehaviour
{
    public float velocidad = 2.0f; // Velocidad de movimiento
    public float segundosParaDestruir = 3.0f; // Tiempo en segundos antes de destruir el personaje
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
        Debug.Log("Movimiento activado. Reproduciendo animaci�n Walking.");

        if (animator != null)
        {
            animator.SetBool("IsWalking", true); // Activa la animaci�n Walking usando un par�metro
        }

        // Programa la destrucci�n del personaje despu�s de los segundos especificados
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
    /// Detiene el movimiento y detiene la animaci�n Walking.
    /// </summary>
    public void DetenerMovimiento()
    {
        mover = false;
        Debug.Log("Movimiento detenido. Deteniendo animaci�n Walking.");

        if (animator != null)
        {
            animator.SetBool("IsWalking", false); // Detiene la animaci�n Walking
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