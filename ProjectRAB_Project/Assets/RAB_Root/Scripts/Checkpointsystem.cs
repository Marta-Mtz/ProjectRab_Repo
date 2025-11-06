using UnityEngine;

public class ChangeColorOnTouch : MonoBehaviour
{
    public string targetTag = "Player"; // Tag del objeto que debe tocarlo
    public Color newColor = Color.green; // Color al tocarlo
    private Renderer objRenderer;

    void Start()
    {
        // Busca el renderer en este objeto o en sus hijos
        objRenderer = GetComponent<Renderer>();
        if (objRenderer == null)
            objRenderer = GetComponentInChildren<Renderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si el objeto que colisiona tiene el tag correcto, cambia de color
        if (collision.gameObject.CompareTag(targetTag))
        {
            objRenderer.material.color = newColor;
        }
    }
}
