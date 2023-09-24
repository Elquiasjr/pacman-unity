using System;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;

    private bool isHome;
    public SpriteRenderer white;

    public bool eaten { get; private set; }

    public override void Enable(float duration)
    {
        if (eaten) { return; }
        base.Enable(duration);
        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        Invoke(nameof(Flash), duration / 2f);
    }

    public override void Disable()
    {
        if (eaten && !isHome)
        {
            return;
        }
        base.Disable();

        Physics2D.IgnoreCollision(ghost.target.gameObject.GetComponent<Pacman>().movement.collider2d, ghost.movement.collider2d, false);
        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        isHome = false;
        white.enabled = false;

    }

    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;

            white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    private void Eaten()
    {
        eaten = true;


        ghost.scatter.Disable();
        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;

    }

    private void OnEnable()
    {
        ghost.movement.speedMultplier = 0.5f;
    }

    private void OnDisable()
    {
        ghost.movement.speedMultplier = 1f;
        eaten = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled && !eaten)
            {
                Eaten();
                ghost.movement.speedMultplier = 2f;
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Pacman>().movement.collider2d, ghost.movement.collider2d);

                float minDistance = float.MaxValue;
                Vector2 direction = Vector2.zero;
                foreach (Vector2 avaliableDirection in avaliableDirections())
                {
                    Vector3 newPosition = transform.position + new Vector3(avaliableDirection.x, avaliableDirection.y, 0f);

                    float distance = (ghost.home.insideTransform.position - newPosition).sqrMagnitude;

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        direction = avaliableDirection;
                    }
                }
                ghost.movement.SetDirection(direction);

            }
        }
    }

    private List<Vector2> avaliableDirections()
    {
        List<Vector2> availableDirections = new List<Vector2>();

        RaycastHit2D hitUp = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, Vector2.up, 1.0f, ghost.movement.obstacleLayer);
        RaycastHit2D hitDown = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, Vector2.down, 1.0f, ghost.movement.obstacleLayer);
        RaycastHit2D hitLeft = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, Vector2.left, 1.0f, ghost.movement.obstacleLayer);
        RaycastHit2D hitRight = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, Vector2.right, 1.0f, ghost.movement.obstacleLayer);

        if (hitUp.collider == null)
        {
            availableDirections.Add(Vector2.up);
        }
        if (hitDown.collider == null)
        {
            availableDirections.Add(Vector2.down);
        }
        if (hitLeft.collider == null)
        {
            availableDirections.Add(Vector2.left);
        }
        if (hitRight.collider == null)
        {
            availableDirections.Add(Vector2.right);
        }

        return availableDirections;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null)
        {
            node.ResetState();
        }

        if (node != null && enabled && !eaten)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 avaliableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(avaliableDirection.x, avaliableDirection.y, 0f);

                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    direction = avaliableDirection;
                }
            }

            ghost.movement.SetDirection(direction);
        }

        if (node != null && eaten)
        {
            if (node.name == "Outside")
            {
                ghost.transform.position = new Vector3(ghost.home.insideTransform.position.x, ghost.home.insideTransform.position.y, -1f);
                isHome = true;
                ghost.movement.SetDirection(Vector2.up, true);
                Disable();
                ghost.home.Enable(duration);

                Physics2D.IgnoreCollision(ghost.target.gameObject.GetComponent<Pacman>().movement.collider2d, ghost.movement.collider2d, false);
                return;
            }

        }
        if (eaten && ghost.path.Count == 0 && !isHome && enabled)
        {
            ghost.BackHomeAStar(node);
        }


        if (eaten && ghost.path.Count > 0 && node.gameObject.layer == LayerMask.NameToLayer("Node"))
        {
            if (node.availableDirections.Contains(ghost.path[0]))
            {
                ghost.movement.SetDirection(ghost.path[0]);
                ghost.path.RemoveAt(0);
            }
        }
    }

}
