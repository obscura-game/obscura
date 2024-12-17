using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simple_movement : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento

    void Update()
    {
        // Obtener las entradas de los ejes (Horizontal y Vertical)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Crear un vector de movimiento basado en las entradas
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        // Aplicar el movimiento al objeto
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
