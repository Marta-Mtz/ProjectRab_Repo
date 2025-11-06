using UnityEngine;

public class FanZone : MonoBehaviour
{
    [Header("Configuración del viento")]
    public float liftForce = 10f;           // Intensidad de la fuerza del ventilador
    public float maxHeight = 10f;           // Altura máxima a la que puede flotar el jugador
    public float damping = 2f;              // Suaviza el movimiento

    private void OnTriggerStay(Collider other)
    {
        // Verifica si el objeto que está dentro tiene un Rigidbody
        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            // Solo aplica la fuerza si está por debajo de la altura máxima
            if (other.transform.position.y < maxHeight)
            {
                // Calcula una fuerza hacia arriba
                Vector3 lift = Vector3.up * liftForce;

                // Aplica la fuerza de forma suave
                rb.AddForce(lift - rb.velocity * damping, ForceMode.Acceleration);
            }
        }
    }
}

