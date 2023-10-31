using Code;
using UnityEngine;

public class HumanBehavior : MonoBehaviour
{
    public float RunawaySpeed = 2.5f;
    public float Acceleration = 0.32f;
    public float DirectionSwapInterval = 1.2f;
    public float CatAvoidDistance = 2f;
    public float FreezeTime = 3f;

    private float _unfreezeAtTime = 0.0f;
    private bool _moveLeft = true;
    private float _nextSwapTime;
    private GameObject _cat;
    private Vector2 _headingToCat;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _nextSwapTime = Random.value / 2;
        _cat = FindObjectOfType<CatBehavior>().gameObject;
    }

    void Update()
    {
        Vector2 catDir = _cat.transform.position - transform.position;
        _headingToCat = catDir.normalized;
        var horizontalDistanceToCat = Mathf.Abs(catDir.x);

        // If not frozen, move
        if (_unfreezeAtTime < Time.time)
        {
            if (catDir.magnitude < CatAvoidDistance && horizontalDistanceToCat < CatAvoidDistance)
            {
                MoveAwayFromCat();
            }
            else
            {
                MoveRandomly();
            }
        }

        if (_moveLeft)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }

    public void Freeze()
    {
        _unfreezeAtTime = Time.time + FreezeTime;
        _rb.velocity = new Vector2(0, _rb.velocity.y);
        _rb.angularVelocity = 0;
    }

    private void MoveAwayFromCat()
    {
        if (_headingToCat.x < 0)  // Cat is to the left, so run right
        {
            _rb.velocity = new Vector2(RunawaySpeed, _rb.velocity.y);
        }
        else  // Cat is to the right, so run left
        {  
            _rb.velocity = new Vector2(-RunawaySpeed, _rb.velocity.y);
        }
    }

    private void MoveRandomly()
    {
        if (Time.time > _nextSwapTime)  // Change direction
        {
            _moveLeft = !_moveLeft;
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            _rb.angularVelocity = 0;
            _nextSwapTime += DirectionSwapInterval;
        }
        if (_moveLeft)
        {
            _rb.AddForce(Vector2.left * Acceleration);
        }
        else
        {
            _rb.AddForce(Vector2.right * Acceleration);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<CatBehavior>() != null)
        {
            _cat.GetComponent<CatBehavior>().Meow();
            UI.IncrementPets();
            Destroy(gameObject);
        }
    }
}
