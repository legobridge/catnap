using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public float Speed = 3.5f;
    public float JumpAmount = 4.0f;
    private bool onGround = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(x * Speed, rb.velocity.y);

        float jump = Input.GetAxis("Jump");
        if (jump > 0 && onGround)
        {
            rb.AddForce(Vector2.up * JumpAmount, ForceMode2D.Impulse);
            onGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Game Over");
            // gameOver
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && collision.contacts[0].normal.y > 0.9f)
        {
            onGround = true;
        }
    }

    void FixedUpdate()
    {
    }
}
