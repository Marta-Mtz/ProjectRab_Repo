using UnityEngine;

public class BallJump : MonoBehaviour
{

    public float JumpForce = 7f;
    private Rigidbody rb;
    private bool isGrounded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) 
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);

        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "suelo_COL")
                {
            isGrounded = true;
                
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "suelo_COL")
        {
            isGrounded = false;

        }
    }

}
