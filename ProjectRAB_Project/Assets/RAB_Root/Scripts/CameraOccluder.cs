using UnityEngine;

public class CameraOccluder : MonoBehaviour
{
    public Camera cam;
    public LayerMask occluderLayer;
    private Renderer rend;

    void Update()
    {
        RaycastHit hit;
        Vector3 dir = (cam.transform.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, dir, out hit, Vector3.Distance(transform.position, cam.transform.position), occluderLayer))
        {
            rend = hit.collider.GetComponent<Renderer>();
            if (rend != null)
                rend.material.SetFloat("_Transparency", 0.2f); // Hace transparente
        }
        else if (rend != null)
        {
            rend.material.SetFloat("_Transparency", 1f); // Vuelve opaco
            rend = null;
        }
    }
}

