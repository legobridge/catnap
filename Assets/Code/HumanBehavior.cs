using Code;
using UnityEngine;

public class HumanBehavior : MonoBehaviour
{
    public float RunawaySpeed = 2.4f;
    public float Acceleration = 0.18f;
    public float CatAvoidDistance = 2f;
    public float FreezeTime = 3f;
    public float PatrolDistance = 1f;

    private float _unfreezeAtTime;
    private float _lastSwapTime;
    private bool _moveLeft;
    private float _prevXPos;
    private GameObject _cat;
    private Vector2 _headingToCat;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _unfreezeAtTime = Time.time;
        _lastSwapTime = Time.time;
        _moveLeft = Random.value < 0.5;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _prevXPos = transform.position.x;
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
        // If too far from previous position or too long since last swap, change direction
        if (Mathf.Abs(_prevXPos - transform.position.x) > PatrolDistance || Time.time - _lastSwapTime > 2.5f)  
        {
            _moveLeft = !_moveLeft;
            _lastSwapTime = Time.time;
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            _rb.angularVelocity = 0;
            _prevXPos = transform.position.x;
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
