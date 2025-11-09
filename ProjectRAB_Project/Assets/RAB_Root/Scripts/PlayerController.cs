using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Editor References")]
    public Rigidbody playerRb; //Referencia al Rigidbody del player
    public AudioSource playerAudio; //Ref al emisor de sonidos del player
    public Transform cameraTransform;

    [Header("Movement Parameters")]
    public float speed = 10;
<<<<<<< Updated upstream
    public Vector2 moveInput; //Almac駭 del input de movimiento de los perif駻icos que usamos para jugar
=======
    public Vector2 moveInput; //Almac�n del input de movimiento de los perif�ricos que usamos para jugar
>>>>>>> Stashed changes

    [Header("Jump Parameters")]
    public float jumpForce = 6;
    public bool isGrounded = true;
    public float powerUpJumpForce = 20f; // Fuerza del salto del PowerUp
    private bool hasPowerUp = false;

    [Header("Respawn System")]
    public float fallLimit = -10;
    public Transform respawnPoint;

    [Header("Checkpoint System")]
    public Transform currentCheckpoint; // El último checkpoint alcanzado

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
<<<<<<< Updated upstream
        //Update para calcular movimientos f﨎icos
=======
        //Update para calcular movimientos f�sicos
>>>>>>> Stashed changes
        PhysicalMovement();
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
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            JumpExtra();
        }

        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;
            PlaySFX(3);

        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Mientras toque el suelo, puede saltar
        }
    }


    void CinematicMovement()
    {
<<<<<<< Updated upstream
        //Movimiento = (Direcci * velocidad * input)
=======
        //Movimiento = (Direcci�n * velocidad * input)
>>>>>>> Stashed changes
        //Necesitais multiplicar el movimiento por Time.deltaTime
        transform.Translate(Vector3.right * speed * moveInput.x * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * moveInput.y * Time.deltaTime);
    }

    void PhysicalMovement()
    {
<<<<<<< Updated upstream
        // Direcciones de la c�mara en el plano XZ
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Ignorar componente vertical (para no moverse hacia arriba/abajo)
        camForward.y = 0f;
        camRight.y = 0f;

        // Normalizar
        camForward.Normalize();
        camRight.Normalize();

        // Direcci�n final de movimiento seg�n input y c�mara
        Vector3 moveDir = (camRight * moveInput.x + camForward * moveInput.y).normalized;

        // Aplicar fuerza en esa direcci�n
        playerRb.AddForce(moveDir * speed, ForceMode.Force);
        //Adir una fuerza al rigidbody = (Direcci * velocidad * input)
=======
        //A�adir una fuerza al rigidbody = (Direcci�n * velocidad * input)
>>>>>>> Stashed changes
        playerRb.AddForce(Vector3.right * speed * moveInput.x);
        playerRb.AddForce(Vector3.forward * speed * moveInput.y);
    }

    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        PlaySFX(0);
    }

    void JumpExtra()
    {
        float jumpStrength = hasPowerUp ? powerUpJumpForce : jumpForce;
        playerRb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        hasPowerUp = false;
    }

    void Respawn()
    {
        //Sustituir el transform.position del player por el del punto de respawn
<<<<<<< Updated upstream
        //transform.position = respawnPoint.position;
        //Resetear el valor de aceleraci del rigidbody
        playerRb.linearVelocity = new Vector3(0,0,0);
=======
        transform.position = respawnPoint.position;
        //Resetear el valor de aceleraci�n del rigidbody
        playerRb.linearVelocity = Vector3.zero;
>>>>>>> Stashed changes
        PlaySFX(2);

        Transform respawnTarget = currentCheckpoint != null ? currentCheckpoint : respawnPoint;
        transform.position = respawnTarget.position;

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
            Jump();
        }
    }




    #endregion
}
