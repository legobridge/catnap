using Code;
using UnityEngine;

public class HumanBehavior : MonoBehaviour
{
    public float RunawaySpeed = 2.5f;
    public float Acceleration = 0.05f;
    public float DirectionSwapInterval = 1.5f;
    public float CatAvoidDistance = 2f;

    private bool _moveLeft = true;
    private float _nextSwapTime;
    private GameObject _cat;
    private Vector2 _headingToCat;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _nextSwapTime = Random.value;
        _cat = FindObjectOfType<CatBehavior>().gameObject;
    }

    void Update()
    {

        Vector2 catDir = _cat.transform.position - transform.position;
        _headingToCat = catDir.normalized;
        var horizontalDistanceToCat = Mathf.Abs(catDir.x);
        if (catDir.magnitude < CatAvoidDistance && horizontalDistanceToCat < CatAvoidDistance)
        {
            MoveAwayFromCat();
        }
        else
        {
            MoveRandomly();
        }
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
