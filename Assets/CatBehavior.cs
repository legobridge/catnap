using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public float Speed = 3.0f;
    public float JumpAmount = 1000.0f;
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
        rb.velocity = new Vector2(x * Speed, 0);

        float jump = Input.GetAxis("Jump");
        if (jump > 0 && onGround)
        {
            Debug.Log(jump);
            rb.AddForce(Vector2.up * JumpAmount, ForceMode2D.Impulse);
            onGround = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    void FixedUpdate()
    {
    }
}
