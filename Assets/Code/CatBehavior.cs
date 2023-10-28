using Code;
using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public float Speed = 3.5f;
    public float JumpAmount = 4.0f;
    public AudioClip MeowCilp;

    private bool onGround = false;
    private Rigidbody2D _rb;
    private AudioSource _audioSource;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(x * Speed, _rb.velocity.y);

        float jump = Input.GetAxis("Jump");
        if (jump > 0 && onGround)
        {
            _rb.AddForce(Vector2.up * JumpAmount, ForceMode2D.Impulse);
            onGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == "Pillow" && collision.contacts[0].normal.y > 0.9f)
        {
            Meow();
            UI.LandOnPillow();
        }
    }

    public void Meow()
    {
        _audioSource.PlayOneShot(MeowCilp);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tilemap" && collision.contacts[0].normal.y > 0.9f)
        {
            onGround = true;
        }
    }

}
