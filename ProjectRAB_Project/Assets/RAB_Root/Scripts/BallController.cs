using UnityEngine;

public class BallController : MonoBehaviour

{

    [Header("Editor References")]

    [ Header("Respawn System")]
    public float fallLimit = -10f;
    public Transform respawnPoint;

    [Header("Movement Parameters")]
    public float speed = 7f;

    [Header("Editor References")]
    private Rigidbody rigid;

    [Header("Jump Parameters")]
    public float jumpForce = 6;



    private void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            rigid.AddForce(Vector3.right * speed);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rigid.AddForce(-Vector3.right * speed);
        }

        if (Input.GetAxis("Vertical") > 0 )
        {
            rigid.AddForce(Vector3.forward * speed);
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            rigid.AddForce(-Vector3.forward * speed);
        }

        rigid = GetComponent<Rigidbody>();
        rigid.sleepThreshold = 0f; // Evita que el Rigidbody se duerma

        if (transform.position.y <= fallLimit)
        {
            Respawn();
        }
    }


    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Respawn si choca con un obstáculo
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        // Teletransportar al punto de respawn
        transform.position = respawnPoint.position;

        // Resetear velocidad para evitar que siga cayendo
        rigid.linearVelocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }


}
