using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostsHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }

    public LayerMask obstacleLayer;
    public GhostFrightened frightened { get; private set; }

    public List<Vector2> path { get; private set; } = new List<Vector2>();
    public GhostBehavior initialBehavior;
    public Transform target;

    public int points = 200;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostsHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior)
        {
            home.Disable();
        }
        if (initialBehavior != null)
        {
            initialBehavior.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else
            {
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }

    public void BackHomeAStar(Node node)
    {
        List<Node> openList = new();
        List<Node> closedList = new();
        Vector3 outsideTransform = home.outsideTransform.transform.position;

        Node current = node;
        int i = 0;

        if (node != null && enabled)
        {
            while (i < 50)
            {
                current.fCost = 0 + Vector2.Distance(current.transform.position, home.insideTransform.transform.position);

                foreach (Vector2 avPosition in current.availableDirections)
                {
                    Node nextNode = current.GetNextNode(avPosition);
                    if (nextNode != null)
                    {
                        if (nextNode.transform.position == outsideTransform)
                        {
                            Debug.Log(name + " Found " + nextNode.transform.position);
                            Debug.Break();
                            path.Add(avPosition);
                            path.Add(Vector2.down);
                            return;
                        }
                        else
                        {
                            nextNode.directionTo = avPosition;
                            float hCost = Vector2.Distance(nextNode.transform.position, home.insideTransform.transform.position);
                            float gCost = Vector2.Distance(nextNode.transform.position, current.transform.position);

                            nextNode.fCost = hCost + gCost;
                            openList.Add(nextNode);
                        }
                    }
                }

                openList.Sort((x, y) => x.fCost.CompareTo(y.fCost));

                current = openList[0];
                path.Add(openList[0].directionTo);
                openList.Remove(current);

                i++;
            }

        }
    }

}
