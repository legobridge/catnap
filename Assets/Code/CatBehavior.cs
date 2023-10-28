using Code;
using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public float Speed = 3.5f;
    public float JumpAmount = 4.0f;
    public AudioClip MeowCilp;
    public AudioClip RoarCilp;

    private bool _hasRoared = false;
    private bool _onGround = false;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(x * Speed, _rb.velocity.y);

        float jump = Input.GetAxis("Jump");
        if (jump > 0 && _onGround)
        {
            _rb.AddForce(Vector2.up * JumpAmount, ForceMode2D.Impulse);
            _onGround = false;
        }

        bool roar = Input.GetButtonDown("Fire");
        if (!_hasRoared && roar)
        {
            Roar();
        }
        
        if (_rb.velocity.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }

    public void Meow()
    {
        _audioSource.PlayOneShot(MeowCilp);
    }

    public void Roar()
    {
        _hasRoared = true; // You only get one roar
        _audioSource.PlayOneShot(RoarCilp);
        var humans = FindObjectsOfType<HumanBehavior>();
        foreach (HumanBehavior human in humans)
        {
            human.Freeze();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tilemap" && collision.contacts[0].normal.y > 0.9f)
        {
            _onGround = true;
        }
        else if (collision.collider.gameObject.name == "Pillow" && collision.contacts[0].normal.y > 0.9f)
        {
            // Landed on the pillow
            if (!UI.IsGameOver())
            {
                Meow();
                UI.LandOnPillow();
            }
        }
    }

}
