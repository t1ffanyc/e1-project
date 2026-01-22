using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float movementX;
    float movementY;
    [SerializeField] float speed = 5.0f;
    [SerializeField] float dashForce = 12f;
    Rigidbody2D rb;
    bool isGrounded;
    bool isDashing;
    int score = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // float movementDistanceX = movementX * speed * Time.deltaTime;
        // float movementDistanceY = movementY * speed * Time.deltaTime;
        // transform.position = new Vector2(transform.position.x + movementDistanceX, transform.position.y + movementDistanceY);
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(movementX * speed, rb.linearVelocity.y);
        }

        if (movementY > 0 && isGrounded)
        {
            rb.AddForce(new Vector2(0, 100));
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        movementX = v.x;
        movementY = v.y;
        Debug.Log("Movement X = " + movementX);
        Debug.Log("Movement Y = " + movementY);
    }

    void OnDash(InputValue value)
    {
        isDashing = true;

        Vector2 dashDir = new Vector2(movementX, 0);

        if (dashDir == Vector2.zero)
            dashDir = Vector2.right * transform.localScale.x;

        rb.AddForce(dashDir.normalized * dashForce, ForceMode2D.Impulse);
        Debug.Log("dash");

        Invoke(nameof(EndDash), 0.15f);
    }

    void EndDash()
    {
        isDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Collectible"))
        {
            score++;
            collision.gameObject.SetActive(false);
            Debug.Log("Score: " + score);
        }
    }
}
