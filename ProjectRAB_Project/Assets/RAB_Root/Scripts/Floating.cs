using UnityEngine;

public class Floating : MonoBehaviour
{
    public float floatHeitgt = 0.5f;

    public float floatSpeed = 2f;

    private float baseHeight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseHeight = transform.position.y; 
    }

    // Update is called once per frame
    void Update()
    {
        float newY = baseHeight + Mathf.Sin(Time.time * floatSpeed) * floatHeitgt; 
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
