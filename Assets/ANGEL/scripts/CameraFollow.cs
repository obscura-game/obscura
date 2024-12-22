using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public Vector3 offset;   // Offset desde el jugador

    void LateUpdate()
    {
        if (player != null)
        {
            // Mantén la cámara a una posición fija respecto al jugador
            transform.position = player.position + offset;
        }
    }
}
