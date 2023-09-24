using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float speed = 8.0f;
    public float speedMultplier = 1.0f;

    public Vector2 initialDirection;
    public LayerMask obstacleLayer;
    public Rigidbody2D rigidBody { get; private set; }

    public CircleCollider2D collider2d { get; private set; }

    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }

    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<CircleCollider2D>();
        startingPosition = transform.position;
    }

    public void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        speedMultplier = 1.0f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rigidBody.isKinematic = false;
        enabled = true;
    }

    private void Update()
    {
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidBody.position;
        Vector2 translation = direction * speed * speedMultplier * Time.fixedDeltaTime;
        rigidBody.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }

    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0.0f, direction, 1.5f, obstacleLayer);
        return hit.collider != null;
    }

}
