using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Puntos de recorrido")]
    public Transform[] waypoints;        // Puntos por donde se moverá la plataforma
    public float speed = 2f;             // Velocidad del movimiento
    public bool loop = true;             // Si vuelve al inicio o no

    private int currentIndex = 0;
    private int direction = 1;           // 1 = hacia adelante, -1 = hacia atrás

    void Update()
    {
        if (waypoints.Length < 2) return;

        // Mover hacia el siguiente punto
        Transform target = waypoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Si llegó al punto, cambia de objetivo
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            if (loop)
            {
                currentIndex = (currentIndex + 1) % waypoints.Length;
            }
            else
            {
                // Rebote tipo ping-pong
                if (currentIndex == waypoints.Length - 1)
                    direction = -1;
                else if (currentIndex == 0)
                    direction = 1;

                currentIndex += direction;
            }
        }
    }

    // --- OPCIONAL ---
    // Para que el jugador se mueva junto con la plataforma
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}

