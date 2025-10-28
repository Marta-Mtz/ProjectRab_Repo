using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{

    [Header("Editor References")]
    public Rigidbody playerRb; //Referencia al Rigidbody del player
    public AudioSource playerAudio; //Ref al emisor de sonidos del player

    [Header("Movement Parameters")]
    public float speed = 10;
    public float rotationSpeed = 10f;
    public Vector2 moveInput; //Almacén del input de movimiento de los periféricos que usamos para jugar

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

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.sleepThreshold = 0f; // Evita que la bola se duerma
    }

    void Update()
    {
        if (transform.position.y <= fallLimit)
        {
            Respawn();
        }
    }

    private void FixedUpdate()
    {
        PhysicalMovement();

        float smoothFactor = 0.1f;
        Vector3 targetVelocity = new Vector3(moveInput.x * speed, playerRb.linearVelocity.y, moveInput.y * speed);
        playerRb.linearVelocity = Vector3.Lerp(playerRb.linearVelocity, targetVelocity, smoothFactor);

        if (isJumping && Time.time - jumpStartTime >= maxJumpTime)
        {
            isJumping = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
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
            isGrounded = false;
        }
    }

    void CinematicMovement()
    {
        //Movimiento = (Dirección * velocidad * input)
        transform.Translate(Vector3.right * speed * moveInput.x * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * moveInput.y * Time.deltaTime);
    }

    void PhysicalMovement()
    {
        //Añadir una fuerza al rigidbody = (Dirección * velocidad * input)
        playerRb.AddForce(Vector3.right * speed * moveInput.x);
        playerRb.AddForce(Vector3.forward * speed * moveInput.y);
    }

    void Jump()
    {
        isGrounded = false;
        playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z);
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        PlaySFX(0);
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;
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
        if (context.performed && isGrounded)
        {
            isGrounded = false;
            isJumping = true;
            jumpStartTime = Time.time;

            Jump();
        }
    }

    #endregion
}