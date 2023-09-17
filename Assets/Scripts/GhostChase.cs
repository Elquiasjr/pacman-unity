using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class GhostChase : GhostBehavior
{


    private void OnDisable()
    {
        ghost.scatter.Enable();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        this.Chase(other, ghost);
    }



    public void Chase(Collider2D other, Ghost ghost)
    {
        switch (ghost.name)
        {
            case "Ghost_Blinky":
                this.ChaseAStar(other, ghost);
                break;
            case "Ghost_Pinky":
                this.ChaseFront(other, ghost);
                break;
            case "Ghost_Inky":
                this.GhostChaseDefault(other, ghost);
                break;
            case "Ghost_Clyde":
                this.ChaseFront(other, ghost);
                break;
        }
    }
    private void ChaseFront(Collider2D other, Ghost ghost)
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

    private void GhostChaseDefault(Collider2D other, Ghost ghost)
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

    private void ChaseAStar(Collider2D other, Ghost ghost)
    {
        Node node = other.GetComponent<Node>();

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        if (node != null && enabled && !ghost.frightened.enabled)
        {
            foreach (Vector2 avaliableDirection in node.availableDirections)
            {
                Debug.Log("Avaliable Direction: " + avaliableDirection);
                Node nextNode = node.GetNextNode(avaliableDirection);
                if (nextNode != null)
                {
                    openList.Add(nextNode);
                }
            }
        }
    }
}
