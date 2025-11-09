using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    [Header("Configuración del pegamento")]
    [Range(0f, 1f)]
    public float slowMultiplier = 0.4f; // 0.4 = 40% de velocidad

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Opcional: reduce la velocidad directamente
            rb.linearVelocity *= slowMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Restaurar drag normal
            rb.linearDamping = 0f;
        }
    }
}
