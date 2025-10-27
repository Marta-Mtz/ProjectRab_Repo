using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerPrueba : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float speed = 10f;
    public float rotationSpeed = 10f;
    private Vector3 moveDirection;
    public Vector2 moveInput; //Almac�n del input de movimiento de los perif�ricos que usamos para jugar

    [Header("Editor References")]
    public Rigidbody playerRb; //Referencia al Rigidbody del player
    public AudioSource playerAudio; //Ref al emisor de sonidos del player
    public Transform cameraTransform; // Referencia para la cámara en el inspector

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
        playerRb.freezeRotation = true; // evita que rote con colisiones
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= fallLimit)
        {
            //Respawn();
        }
    }
    void PhysicalMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // direcciones de la cámara (solo en plano horizontal)
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // dirección de movimiento relativa a la cámara
        moveDirection = (forward * v + right * h).normalized;

        // rotar el jugador hacia la dirección de movimiento
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
        // Movimiento físico
        PhysicalMovement();

        playerRb.MovePosition(playerRb.position + moveDirection * speed * Time.fixedDeltaTime);

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
            isGrounded = false; // ❌ Cuando sales del suelo, ya no puede saltar
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
