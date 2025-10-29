using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Editor References")]
    public Rigidbody playerRb; //Referencia al Rigidbody del player
    public AudioSource playerAudio; //Ref al emisor de sonidos del player

    [Header("Movement Parameters")]
    public float speed = 10;
    public float rotationSpeed = 10f;
    public Vector2 moveInput; //Almac�n del input de movimiento de los perif�ricos que usamos para jugar

    [Header("Jump Parameters")]
    public float jumpForce = 6;
    public bool isGrounded = true;
    public float maxJumpTime = 0.3f; // duración máxima del salto
    private bool isJumping = false;
    private float jumpStartTime = 0f;

    [Header("PowerUp Settings")]
    public float powerUpJumpForce = 20f; // Fuerza del salto del PowerUp

    [Header("Respawn System")]
    public float fallLimit = -10;
    public Transform respawnPoint;

    [Header("Sound Configuration")]
    public AudioClip[] soundCollection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.sleepThreshold = 0f; // Evita que la bola se duerma
    }

    // Update is called once per frame
    void Update()
    {
        //CinematicMovement();
        //Respawn por altura
        if (transform.position.y <= fallLimit)
        {
            Respawn();
        }
    }

    private void FixedUpdate()
    {
        PhysicalMovement();

        if (isJumping && Time.time - jumpStartTime >= maxJumpTime)
        {
            isJumping = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; //Devuelve la capacidad de saltar
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Respawn();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {

            playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z);
            playerRb.AddForce(Vector3.up * powerUpJumpForce, ForceMode.Impulse);


        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Cuando sales del suelo, ya no puede saltar
        }
    }



    void CinematicMovement()
    {
        //Movimiento = (Direcci�n * velocidad * input)
        //Necesitais multiplicar el movimiento por Time.deltaTime
        transform.Translate(Vector3.right * speed * moveInput.x * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * moveInput.y * Time.deltaTime);
    }

    void PhysicalMovement()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Aplica torque (rotación física)
        playerRb.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x) * speed);

        // Limita la velocidad máxima para que no se dispare
        if (playerRb.linearVelocity.magnitude > speed)
        {
            playerRb.linearVelocity = playerRb.linearVelocity.normalized * speed;
        }
    }


    void Jump()
    {
        isGrounded = false; // ya no puede saltar otra vez hasta tocar el suelo
        playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z); // reinicia velocidad vertical
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        PlaySFX(0);
    }

    void Respawn()
    {
        //Sustituir el transform.position del player por el del punto de respawn
        transform.position = respawnPoint.position;
        //Resetear el valor de aceleraci�n del rigidbody
        playerRb.linearVelocity = Vector3.zero;
        PlaySFX(2);
    }

    public void PlaySFX(int soundToPlay)
    {
        playerAudio.PlayOneShot(soundCollection[soundToPlay]);
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {

        if (context.performed && isGrounded == true)
        {
            isGrounded = false;
            isJumping = true;
            jumpStartTime = Time.time;
            isGrounded = false;

            Jump();

        }


    }




    #endregion
}