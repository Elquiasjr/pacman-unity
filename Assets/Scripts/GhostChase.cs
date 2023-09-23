using System;
using UnityEngine;

public class GhostChase : GhostBehavior
{

    public Ghost reference;
    private void OnDisable()
    {
        ghost.scatter.Enable();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Chase(other);
    }



    public void Chase(Collider2D other)
    {
        switch (ghost.name)
        {
            case "Ghost_Blinky":
                GhostChaseDefault(other);
                break;
            case "Ghost_Pinky":
                ChaseFront(other);
                break;
            case "Ghost_Inky":
                ChaseTangent(other);
                break;
            case "Ghost_Clyde":
                ChaseRadius(other);
                break;
        }
    }
    private void ChaseFront(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 avaliableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(avaliableDirection.x, avaliableDirection.y, 0f);



                Vector3 frontOfTarget = ghost.target.position + new Vector3(ghost.target.GetComponent<Movement>().direction.x * 6f, ghost.target.GetComponent<Movement>().direction.y * 6f, 0f);

                float distance = (frontOfTarget - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    minDistance = distance;
                    direction = avaliableDirection;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

    private void ChaseTangent(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled && !ghost.frightened.enabled && reference != null)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 avaliableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(avaliableDirection.x, avaliableDirection.y, 0f);

                Vector3 frontOfTarget = ghost.target.position + new Vector3(ghost.target.GetComponent<Movement>().direction.x * 6f, ghost.target.GetComponent<Movement>().direction.y * 6f, 0f);

                float xDistance = frontOfTarget.x - reference.transform.position.x;
                float yDistance = frontOfTarget.y - reference.transform.position.y;

                Vector3 point = new Vector3(frontOfTarget.x + xDistance, frontOfTarget.y + yDistance, 0f);

                float distance = (point - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    minDistance = distance;
                    direction = avaliableDirection;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

    private void GhostChaseDefault(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 avaliableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(avaliableDirection.x, avaliableDirection.y, 0f);

                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    minDistance = distance;
                    direction = avaliableDirection;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

    private void ChaseRadius(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 avaliableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(avaliableDirection.x, avaliableDirection.y, 0f);


                float distanceToPacman = Vector3.Distance(ghost.target.position, newPosition);

                float distance;

                Debug.Log(distanceToPacman);

                if (distanceToPacman < 8f)
                {
                    distance = (ghost.home.transform.position - newPosition).sqrMagnitude;
                }
                else
                {
                    distance = (ghost.target.position - newPosition).sqrMagnitude;
                }


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
